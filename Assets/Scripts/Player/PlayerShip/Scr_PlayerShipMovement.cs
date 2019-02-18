using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Scr_PlayerShipMovement : MonoBehaviour
{
    [Header("Ship State")]
    [SerializeField] public PlayerShipState playerShipState;

    [Header("Taking Off Parameters")]    
    [SerializeField] private float takeOffDistance;
    [SerializeField] private float targetVelocity;
    [SerializeField] private float takingOffTime;
    [SerializeField] private float bulletTime;

    [Header("Landed Parameters")]
    [SerializeField] private float canControlTimer;
    [SerializeField] private float shipOrientationSpeed;

    [Header("Landing Parameters")]
    [SerializeField] private bool manualLanding;
    [SerializeField] public float landDistance;
    [SerializeField] private float landingTime;
    [SerializeField] public LayerMask planetLayer;
    [SerializeField] private float landingAccuracy;

    [Header("In Space Parameters")]
    [SerializeField] private float rotationDelay;

    [Header("Speed System")]
    [SerializeField] public float maxSpeed;
    [SerializeField] private float normalSpeed;
    [SerializeField] private float boostSpeed;
    [SerializeField] private float limitUnits;

    [Header("Warming System")]
    [SerializeField] public float warmingAmount;
    [SerializeField] public float warmingSpeed;

    [Header("Interface")]
    [SerializeField] private string warmingMessage;
    [SerializeField] private string getInOutMessage;
    [SerializeField] private string takeOffMessage;
    [SerializeField] private string damagedMessage;

    [Header("References")]
    [SerializeField] private Transform endOfShip;
    [SerializeField] private Animator warmingSliderAnim;
    [SerializeField] private Animator messageTextAnim;
    [SerializeField] private Animator undercarriageAnim;
    [SerializeField] private CapsuleCollider2D mouseCheckTrigger;
    [SerializeField] public Camera mainCamera;
    [SerializeField] private TextMeshProUGUI limiterText;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Slider speedSlider;
    [SerializeField] private Slider limitSlider;
    [SerializeField] private Scr_AstronautMovement astronautMovement;
    [SerializeField] private GameObject leftLander;
    [SerializeField] private GameObject rightLander;

    [HideInInspector] public bool astronautOnBoard;
    [HideInInspector] public bool onGround;
    [HideInInspector] public bool takingOff;
    [HideInInspector] public bool landing;
    [HideInInspector] public bool dead;
    [HideInInspector] public bool canRotateShip;
    [HideInInspector] public bool canControlShip;
    [HideInInspector] public bool landedOnce;
    [HideInInspector] public bool damaged;
    [HideInInspector] public GameObject currentPlanet;
    [HideInInspector] public Rigidbody2D rb;

    private bool countDownToMove;
    private bool slow;
    private bool takingOffSlow;
    private float maxSpeedSaved;
    private float currentSpeed;
    private float canControlTimerSaved;
    private float checkingDistance;
    private float initialBulletTime;
    
    private Vector3 landingOrientationVector;
    private Vector3 targetTakingOff;
    private Vector3 targetLanding;    
    private Scr_PlayerShipStats playerShipStats;
    private Scr_PlayerShipActions playerShipActions;
    private Scr_PlayerShipEffects playerShipEffects;
    private Scr_PlayerShipDeathCheck playerShipDeathCheck;
    private Scr_PlayerShipPrediction playerShipPrediction;

    public enum PlayerShipState
    {
        landing,
        takingOff,
        landed,
        inSpace
    }

    private void Start()
    {        
        rb = GetComponent<Rigidbody2D>();
        playerShipStats = GetComponent<Scr_PlayerShipStats>();
        playerShipActions = GetComponent<Scr_PlayerShipActions>();
        playerShipEffects = GetComponent<Scr_PlayerShipEffects>();
        playerShipPrediction = GetComponent<Scr_PlayerShipPrediction>();
        playerShipDeathCheck = GetComponentInChildren<Scr_PlayerShipDeathCheck>();

        canControlTimerSaved = canControlTimer;
        maxSpeedSaved = maxSpeed;
        limitSlider.maxValue = maxSpeedSaved;
        speedSlider.maxValue = maxSpeedSaved;
        messageText.text = "";
        canControlShip = false;
        onGround = true;
        checkingDistance = 100f;
        initialBulletTime = bulletTime;
    }

    private void Update()
    {
        Timers();
        BulletTime();
        SpeedLimiter();
        MessageTextManager();
        PlayerShipStateCheck();

        playerShipEffects.OnGroundEffects();

        if (playerShipState == PlayerShipState.inSpace)
        {
            ShipControl();
            playerShipEffects.InSpaceEffects();
        }

        if (playerShipState == PlayerShipState.landing)
            ShipControl();

        if (playerShipState == PlayerShipState.landed)
            playerShipEffects.WarmingSliderColor();
    }

    private void FixedUpdate()
    {
        UpdateShipRotationWhenLanded();
    }

    float messageTimer1 = 2f;

    private void MessageTextManager()
    {
        if (damaged)
        {
            messageText.fontSize = 6;
            messageText.text = damagedMessage;
            messageTextAnim.SetBool("Show", true);
        }

        else
        {
            if (astronautMovement.canEnterShip)
            {
                if (!astronautOnBoard)
                {
                    messageText.fontSize = 14;
                    messageText.text = getInOutMessage;
                    messageTextAnim.SetBool("Show", true);
                }

                else
                    messageTextAnim.SetBool("Show", false);
            }

            else
                messageTextAnim.SetBool("Show", false);

            if (playerShipState == PlayerShipState.landed && astronautOnBoard)
            {
                messageTimer1 -= Time.deltaTime;

                if (messageTimer1 <= 0)
                {
                    messageText.fontSize = 6;
                    messageText.text = warmingMessage;
                    messageTextAnim.SetBool("Show", true);

                    if (playerShipEffects.warmingSlider.value > 0)
                    {
                        messageTextAnim.SetBool("Show", false);

                        if (playerShipEffects.warmingSlider.value >= 0.9f * playerShipEffects.warmingSlider.maxValue)
                        {
                            messageText.text = takeOffMessage;
                            messageTextAnim.SetBool("Show", true);
                        }
                    }
                }
            }
        }
    }

    private void Timers()
    {
        if (countDownToMove)
        {
            canControlTimerSaved -= Time.deltaTime;

            canControlShip = false;

            if (canControlTimerSaved <= 0)
            {
                canControlTimerSaved = canControlTimer;

                canControlShip = true;
                countDownToMove = false;
            }
        }
    }

    private void BulletTime()
    {
        if (currentPlanet == null && !takingOffSlow && !slow)
        {
            GetComponent<Scr_PlayerShipPrediction>().enabled = false;
            GetComponent<LineRenderer>().enabled = false;

            Time.timeScale = 0f;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            slow = true;
        }

        else if (Time.timeScale < 1 && slow == true && !takingOffSlow)
        {
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
            Time.timeScale += 0.25f * Time.unscaledDeltaTime;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;

            if (Time.timeScale >= 1)
            {
                Time.timeScale = 1;
                GetComponent<Scr_PlayerShipPrediction>().enabled = true;
                GetComponent<LineRenderer>().enabled = true;
                slow = false;
                takingOffSlow = true;
            }
        }
    }

    private void UpdateShipRotationWhenLanded()
    {
        RaycastHit2D leftLanderHit = Physics2D.Raycast(leftLander.transform.position, -transform.up, Mathf.Infinity, planetLayer);
        RaycastHit2D rightLanderHit = Physics2D.Raycast(rightLander.transform.position, -transform.up, Mathf.Infinity, planetLayer);

        Debug.DrawLine(leftLander.transform.position, leftLanderHit.point, Color.red);

        if (playerShipState == PlayerShipState.landing)
        {
            if (Vector2.Distance(leftLander.transform.position, leftLanderHit.point) <= landingAccuracy)
            {
                currentPlanet = leftLanderHit.collider.gameObject;
                astronautMovement.currentPlanet = leftLanderHit.collider.gameObject;

                if (landedOnce)
                    countDownToMove = true;

                onGround = true;

                playerShipActions.startExitDelay = true;
            }

            else if (Vector2.Distance(rightLander.transform.position, rightLanderHit.point) <= landingAccuracy)
            {
                currentPlanet = rightLanderHit.collider.gameObject;
                astronautMovement.currentPlanet = rightLanderHit.collider.gameObject;

                if (landedOnce)
                    countDownToMove = true;

                onGround = true;

                playerShipActions.startExitDelay = true;
            }
        }
        
        if (currentPlanet != null && playerShipState == PlayerShipState.landed)
        {
            transform.rotation = Quaternion.LookRotation(transform.forward, Vector2.Perpendicular(rightLanderHit.point - leftLanderHit.point));
        }
    }

    private void PlayerShipStateCheck()
    {
        if (!dead)
        {
            if (currentPlanet != null)
            {
                playerShipPrediction.predictionTime = 0;

                RaycastHit2D beginLandingHit = Physics2D.Raycast(endOfShip.position, -endOfShip.up, landDistance, planetLayer);

                if (beginLandingHit && playerShipState == PlayerShipState.inSpace)
                {
                    mainCamera.GetComponent<Scr_MainCamera>().smoothRotation = true;
                    mainCamera.GetComponent<Scr_MainCamera>().CameraShake();

                    maxSpeedSaved = maxSpeed;

                    Landing();
                }
            }

            else
            {
                playerShipState = PlayerShipState.inSpace;
                checkingDistance = 100;
                landedOnce = true;

                if (playerShipActions.currentAsteroid == null)
                    playerShipPrediction.predictionTime = 6;
            }

            if (Input.GetKey(KeyCode.LeftShift) && playerShipState == PlayerShipState.landed && canControlShip && !damaged)
            {
                playerShipEffects.warmingSlider.gameObject.SetActive(true);
                playerShipEffects.warmingSlider.value += Time.deltaTime * warmingSpeed;

                if (playerShipEffects.warmingSlider.value >= (0.95f * playerShipEffects.warmingSlider.maxValue) && Input.GetMouseButtonDown(0))
                {
                    playerShipEffects.warmingSlider.gameObject.SetActive(false);

                    mainCamera.GetComponent<Scr_MainCamera>().smoothRotation = false;
                    mainCamera.GetComponent<Scr_MainCamera>().CameraShake();

                    TakingOff();
                }
            }

            else
            {
                playerShipEffects.warmingSlider.value -= Time.deltaTime;

                if (playerShipEffects.warmingSlider.value <= 0)
                    playerShipEffects.warmingSlider.gameObject.SetActive(false);
            }

            if (onGround)
            {
                playerShipState = PlayerShipState.landed;

                Landed();
                playerShipEffects.LandingEffects(false);
            }

            if (takingOff)
            {
                playerShipState = PlayerShipState.takingOff;
                canRotateShip = false;
                canControlShip = false;

                rb.velocity = Vector2.Lerp(rb.velocity, targetVelocity * transform.up, Time.deltaTime * takingOffTime);

                playerShipEffects.TakingOffEffects(true);

                if (currentPlanet == null)
                {
                    transform.SetParent(null);
                    playerShipEffects.TakingOffEffects(false);

                    canRotateShip = true;
                    canControlShip = true;

                    takingOff = false;
                }
            }

            if (landing)
            {
                Vector3 planetDirection = new Vector3(transform.position.x - currentPlanet.transform.position.x, transform.position.y - currentPlanet.transform.position.y, transform.position.z - currentPlanet.transform.position.z);

                playerShipState = PlayerShipState.landing;
                canRotateShip = true;

                RaycastHit2D checkingDistanceHit = Physics2D.Raycast(endOfShip.position, -endOfShip.up, Mathf.Infinity, planetLayer);

                if (checkingDistanceHit)
                {
                    if (checkingDistanceHit.distance < checkingDistance)
                        checkingDistance = checkingDistanceHit.distance;
                }

                Debug.DrawRay(endOfShip.position, -endOfShip.up * checkingDistance, Color.yellow);

                RaycastHit2D landingProperlyHit = Physics2D.Raycast(endOfShip.position, -endOfShip.up, checkingDistance + 0.1f, planetLayer);

                if (landingProperlyHit)
                {
                    if (manualLanding && Input.GetMouseButton(0))
                    {
                        rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.deltaTime * landingTime);

                        playerShipDeathCheck.CheckLandingTime(false);
                        playerShipEffects.LandingEffects(true);
                    }

                    else
                    {
                        rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.deltaTime * landingTime);

                        playerShipDeathCheck.CheckLandingTime(false);
                        playerShipEffects.LandingEffects(true);
                    }
                }

                if (onGround)
                {
                    playerShipEffects.LandingEffects(false);

                    landing = false;
                }
            }
        }
    }

    private void Landed()
    {
        transform.SetParent(currentPlanet.transform);
        takingOffSlow = false;

        if (rb.velocity != Vector2.zero)
            rb.velocity = Vector2.zero;
    }

    void TakingOff()
    {
        targetTakingOff = transform.position + new Vector3(0, takeOffDistance, 0);
        undercarriageAnim.SetBool("PickUp", true);
        takingOff = true;
        onGround = false;
        playerShipActions.startExitDelay = false;
    }

    private void Landing()
    {
        transform.SetParent(currentPlanet.transform);
        undercarriageAnim.SetBool("PickUp", false);
        landing = true;
    }

    private void SpeedLimiter()
    {
        int speed = (int)(rb.velocity.magnitude * 10);

        speedSlider.value = speed;
        limitSlider.value = maxSpeedSaved;

        speedText.text = speed.ToString();
        speedText.transform.localPosition = new Vector3(0, speed * 3, 0);
        limiterText.text = maxSpeedSaved.ToString();

        if (playerShipState == PlayerShipState.inSpace && !mainCamera.GetComponent<Scr_MainCamera>().mining)
            maxSpeedSaved += Input.GetAxis("Mouse ScrollWheel") * limitUnits;

        maxSpeedSaved = Mathf.Clamp(maxSpeedSaved, 0f, maxSpeed);
        maxSpeedSaved = (float)((int)maxSpeedSaved);

        if ((rb.velocity.magnitude * 10) > maxSpeedSaved)
            rb.AddForce(-rb.velocity.normalized * currentSpeed * Time.fixedDeltaTime);
    }

    private void ShipControl()
    {
        if (canControlShip && !onGround && !damaged)
        {
            if (playerShipStats.currentFuel > 0)
            {
                if (Input.GetMouseButton(0) && playerShipState == PlayerShipState.inSpace)
                {
                    playerShipEffects.thrusterParticles.Play();

                    if (Input.GetButton("Boost"))
                    {
                        playerShipStats.FuelConsumption(true);
                        currentSpeed = boostSpeed;
                        rb.AddForce(transform.up * currentSpeed * Time.fixedDeltaTime);

                        playerShipEffects.thrusterParticles2.Play();
                        playerShipEffects.thrusterParticles3.Play();
                    }

                    else
                    {
                        playerShipStats.FuelConsumption(false);
                        currentSpeed = normalSpeed;
                        rb.AddForce(transform.up * currentSpeed * Time.fixedDeltaTime);

                        playerShipEffects.thrusterParticles2.Stop();
                        playerShipEffects.thrusterParticles3.Stop();
                    }
                }

                else if (playerShipState == PlayerShipState.inSpace)
                {
                    playerShipEffects.thrusterParticles.Stop();

                    playerShipEffects.thrusterParticles2.Stop();
                    playerShipEffects.thrusterParticles3.Stop();
                }
            }

            else
                playerShipEffects.thrusterParticles.Stop();

            if (Input.GetButtonDown("Boost"))
            {
                playerShipEffects.thrusterParticles2.Stop();
                playerShipEffects.thrusterParticles3.Stop();
            }
        }

        if (canRotateShip && (playerShipState == PlayerShipState.inSpace || playerShipState == PlayerShipState.landing))
        {
            Vector3 difference = (mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position);
            difference.Normalize();
            float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0f, 0f, rotationZ - 90), rotationDelay);
        }
    }
}