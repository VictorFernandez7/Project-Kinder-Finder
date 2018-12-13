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

    [Header("References")]
    [SerializeField] private Transform endOfShip;
    [SerializeField] private Animator warmingSliderAnim;
    [SerializeField] private Animator messageTextAnim;
    [SerializeField] private Animator undercarriageAnim;

    [HideInInspector] public bool astronautOnBoard;
    [HideInInspector] public bool onGround;
    [HideInInspector] public bool takingOff;
    [HideInInspector] public bool landing;
    [HideInInspector] public bool dead;
    [HideInInspector] public bool canRotateShip;
    [HideInInspector] public bool canControlShip;
    [HideInInspector] public bool landedOnce;
    [HideInInspector] public Camera mainCamera;
    [HideInInspector] public GameObject currentPlanet;
    [HideInInspector] public Rigidbody2D rb;

    private bool countDownToMove;
    private bool slow;
    private float maxSpeedSaved;
    private float currentSpeed;
    private float canControlTimerSaved;
    private float checkingDistance;
    private float initialBulletTime;
    private Slider speedSlider;
    private Slider limitSlider;
    private Vector3 landingOrientationVector;
    private Vector3 targetTakingOff;
    private Vector3 targetLanding;
    private TrailRenderer trailRenderer;
    private TextMeshProUGUI limiterText;
    private TextMeshProUGUI speedText;
    private TextMeshProUGUI messageText;
    private Scr_PlayerShipStats playerShipStats;
    private Scr_PlayerShipActions playerShipActions;
    private Scr_AstronautMovement astronautMovement;
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
        mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        limiterText = GameObject.Find("Limiter").GetComponent<TextMeshProUGUI>();
        speedText = GameObject.Find("Speed").GetComponent<TextMeshProUGUI>();
        messageText = GameObject.Find("MessageText").GetComponent<TextMeshProUGUI>();
        astronautMovement = GameObject.Find("Astronaut").GetComponent<Scr_AstronautMovement>();
        speedSlider = GameObject.Find("SpeedSlider").GetComponent<Slider>();
        limitSlider = GameObject.Find("LimitSlider").GetComponent<Slider>();

        rb = GetComponent<Rigidbody2D>();
        playerShipStats = GetComponent<Scr_PlayerShipStats>();
        playerShipActions = GetComponent<Scr_PlayerShipActions>();
        playerShipEffects = GetComponent<Scr_PlayerShipEffects>();
        playerShipPrediction = GetComponent<Scr_PlayerShipPrediction>();
        playerShipDeathCheck = GetComponentInChildren<Scr_PlayerShipDeathCheck>();
        trailRenderer = GetComponent<TrailRenderer>();

        canControlTimerSaved = canControlTimer;
        maxSpeedSaved = maxSpeed;
        limitSlider.maxValue = maxSpeedSaved;
        speedSlider.maxValue = maxSpeedSaved;
        messageText.text = "";
        canControlShip = false;
        checkingDistance = 100f;
        initialBulletTime = bulletTime;
    }

    private void Update()
    {
        Timers();
        //BulletTime();
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
        //UpdateShipRotationWhenLanded();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (playerShipState == PlayerShipState.landing || playerShipState == PlayerShipState.landed)
        {
            if (collision.gameObject.tag == "Planet" && !dead)
            {
                currentPlanet = collision.gameObject;
                astronautMovement.currentPlanet = collision.gameObject;

                if (landedOnce)
                    countDownToMove = true;

                onGround = true;

                playerShipActions.startExitDelay = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (playerShipState == PlayerShipState.landing || playerShipState == PlayerShipState.landed || playerShipState == PlayerShipState.takingOff)
        {
            if (collision.gameObject.tag == "Planet" && !dead)
            {
                onGround = false;

                playerShipActions.startExitDelay = false;
            }
        }
    }

    float messageTimer1 = 2f;

    private void MessageTextManager()
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
        if (playerShipState == PlayerShipState.inSpace)
        {
            initialBulletTime -= Time.deltaTime;

            if (initialBulletTime <= 0 && !slow)
            {
                Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);

                if (Time.timeScale == 1)
                {
                    Time.timeScale = 0f;
                    Time.fixedDeltaTime = Time.timeScale * 0.02f;
                }

                Time.timeScale += 0.25f * Time.unscaledDeltaTime;
                Time.fixedDeltaTime = Time.timeScale * 0.02f;

                if (Time.timeScale >= 1)
                {
                    Time.timeScale = 1;
                    playerShipPrediction.enabled = true;
                    initialBulletTime = bulletTime;
                    slow = true;
                }
            }
        }

        if (playerShipState == PlayerShipState.landed)
            slow = false;
    }

    private void UpdateShipRotationWhenLanded()
    {
        if (currentPlanet != null && playerShipState == PlayerShipState.landed)
        {
            Vector3 currentRootation = new Vector3(transform.localRotation.x, transform.localRotation.y, transform.localRotation.y);
            landingOrientationVector = transform.position - currentPlanet.transform.position;
            transform.localRotation = Quaternion.Euler(Vector3.Lerp(currentRootation, landingOrientationVector, Time.deltaTime * shipOrientationSpeed));
        }
    }

    private void PlayerShipStateCheck()
    {
        if (!dead)
        {
            if (currentPlanet != null)
            {
                trailRenderer.enabled = false;
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
                {
                    trailRenderer.enabled = true;
                    playerShipPrediction.predictionTime = 6;
                }
            }

            if (Input.GetKey(KeyCode.LeftShift) && playerShipState == PlayerShipState.landed && canControlShip)
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

        if (rb.velocity != Vector2.zero)
            rb.velocity = Vector2.zero;
    }

    void TakingOff()
    {
        targetTakingOff = transform.position + new Vector3(0, takeOffDistance, 0);
        undercarriageAnim.SetBool("PickUp", true);
        takingOff = true;
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
        if (canControlShip && !onGround)
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
                    }

                    else
                    {
                        playerShipStats.FuelConsumption(false);
                        currentSpeed = normalSpeed;
                        rb.AddForce(transform.up * currentSpeed * Time.fixedDeltaTime);
                    }
                }

                else if (playerShipState == PlayerShipState.inSpace)
                    playerShipEffects.thrusterParticles.Stop();
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