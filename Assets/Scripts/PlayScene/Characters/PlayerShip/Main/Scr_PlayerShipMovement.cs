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
    [SerializeField] private float timeToTakeOff;
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
    [SerializeField] public Animator undercarriageAnim;
    [SerializeField] private CapsuleCollider2D mouseCheckTrigger;
    [SerializeField] public Camera mainCamera;
    [SerializeField] private TextMeshProUGUI limiterText;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Slider speedSlider;
    [SerializeField] private Slider limitSlider;
    [SerializeField] private Scr_AstronautMovement astronautMovement;
    [SerializeField] private GameObject leftLander;
    [SerializeField] private GameObject rightLander;
    [SerializeField] private Scr_GameManager gameManager;

    [HideInInspector] public bool astronautOnBoard;
    [HideInInspector] public bool onGround;
    [HideInInspector] public bool takingOff;
    [HideInInspector] public bool landing;
    [HideInInspector] public bool canRotateShip;
    [HideInInspector] public bool canControlShip;
    [HideInInspector] public bool landedOnce;
    [HideInInspector] public bool damaged;
    [HideInInspector] public GameObject currentPlanet;
    [HideInInspector] public Rigidbody2D rb;

    private bool countDownToMove;
    private bool slow;
    private bool takingOffSlow;
    private bool firstTakeOff;
    private float maxSpeedSaved;
    private float currentSpeed;
    private float canControlTimerSaved;
    private float checkingDistance;
    private float initialBulletTime;
    private float savedTimeToTakeOff;

    private Vector3 landingOrientationVector;
    private Vector3 targetTakingOff;
    private Vector3 targetLanding;    
    private Scr_PlayerShipStats playerShipStats;
    private Scr_PlayerShipEffects playerShipEffects;
    private Scr_PlayerShipActions playerShipActions;
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
        playerShipEffects = GetComponent<Scr_PlayerShipEffects>();
        playerShipActions = GetComponent<Scr_PlayerShipActions>();
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
        savedTimeToTakeOff = timeToTakeOff;
    }

    private void Update()
    {
        Timers();
        //BulletTime();
        SpeedLimiter();
        PlayerShipStateCheck();

        if (playerShipState == PlayerShipState.inSpace)
            ShipControl();

        if (playerShipState == PlayerShipState.landing)
            ShipControl();
    }

    private void FixedUpdate()
    {
        UpdateShipRotationWhenLanded();
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
                if (landedOnce)
                    countDownToMove = true;

                onGround = true;

                playerShipActions.startExitDelay = true;
            }

            else if (Vector2.Distance(rightLander.transform.position, rightLanderHit.point) <= landingAccuracy)
            {
                if (landedOnce)
                    countDownToMove = true;

                onGround = true;

                playerShipActions.startExitDelay = true;
            }
        }
        
        if (currentPlanet != null && playerShipState == PlayerShipState.landed)
            transform.rotation = Quaternion.LookRotation(transform.forward, Vector2.Perpendicular(rightLanderHit.point - leftLanderHit.point));
    }

    private void PlayerShipStateCheck()
    {
        if (!Scr_PlayerData.dead)
        {
            if (currentPlanet != null)
            {
                playerShipPrediction.predictionTime = 0;

                RaycastHit2D beginLandingHit = Physics2D.Raycast(endOfShip.position, -endOfShip.up, landDistance, planetLayer);

                if (beginLandingHit && playerShipState == PlayerShipState.inSpace)
                {
                    mainCamera.GetComponent<Scr_MainCamera>().smoothRotation = true;
                    mainCamera.GetComponent<Scr_MainCamera>().CameraShake(0.25f, 5, 12);

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

            if (Input.GetKeyDown(KeyCode.LeftShift) && playerShipState == PlayerShipState.landed && canControlShip && !damaged && playerShipStats.currentFuel > (playerShipStats.maxFuel * 0.15f))
            {
                if (playerShipStats.currentFuel == playerShipStats.maxFuel || firstTakeOff)
                    savedTimeToTakeOff = timeToTakeOff;
            }

            if (Input.GetKey(KeyCode.LeftShift) && playerShipState == PlayerShipState.landed && canControlShip && !damaged && playerShipStats.currentFuel > (playerShipStats.maxFuel * 0.15f))
            {
                if (playerShipStats.currentFuel == playerShipStats.maxFuel || firstTakeOff)
                {
                    firstTakeOff = true;

                    playerShipEffects.WarmingEffects(true);

                    savedTimeToTakeOff -= Time.deltaTime;

                    if (savedTimeToTakeOff <= 0 && Input.GetMouseButtonDown(0))
                    {
                        playerShipEffects.WarmingEffects(false);
                        playerShipEffects.warming = false;
                        playerShipEffects.PlayParticleSystem(playerShipEffects.takingOffSlam);

                        mainCamera.GetComponent<Scr_MainCamera>().smoothRotation = false;
                        mainCamera.GetComponent<Scr_MainCamera>().CameraShake(0.25f, 5, 12);

                        TakingOff();
                    }
                }
            }

            if (Input.GetKeyUp(KeyCode.LeftShift) && playerShipState == PlayerShipState.landed)
                playerShipEffects.WarmingEffects(false);

            if (onGround)
            {
                playerShipState = PlayerShipState.landed;

                Landed();
            }

            if (takingOff)
            {
                playerShipState = PlayerShipState.takingOff;
                canRotateShip = false;
                canControlShip = false;

                rb.velocity = Vector2.Lerp(rb.velocity, targetVelocity * transform.up, Time.deltaTime * takingOffTime);

                if (currentPlanet == null)
                {
                    transform.SetParent(null);

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
                    }

                    else
                    {
                        rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.deltaTime * landingTime * (currentPlanet.transform.GetComponentInChildren<Renderer>().bounds.size.y / gameManager.initialPlanet.GetComponentInChildren<Renderer>().bounds.size.y));

                        playerShipDeathCheck.CheckLandingTime(false);
                    }
                }

                if (onGround)
                {
                    playerShipEffects.StopParticleSystem(playerShipEffects.mainThruster);
                    playerShipEffects.PlayParticleSystem(playerShipEffects.landingSlam);
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

        Scr_PlayerData.checkpointPlanet = currentPlanet.transform;
        Scr_PlayerData.checkpointPlayershipPosition = transform.localPosition;
        Scr_PlayerData.checkpointPlayershipRotation = transform.localRotation;
        Scr_PlayerData.checkpointFuel = playerShipStats.currentFuel;
        Scr_PlayerData.checkpointShield = playerShipStats.currentShield;
    }

    void TakingOff()
    {
        targetTakingOff = transform.position + new Vector3(0, takeOffDistance, 0);
        undercarriageAnim.SetBool("PickUp", true);
        takingOff = true;
        onGround = false;
        playerShipActions.startExitDelay = false;

        Scr_PlayerData.checkpointFuel = playerShipStats.currentFuel;
        Scr_PlayerData.checkpointShield = playerShipStats.currentShield;
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
                    if (Input.GetButton("Boost"))
                    {
                        playerShipStats.FuelConsumption(true);
                        currentSpeed = boostSpeed;
                        rb.AddForce(transform.up * currentSpeed * Time.fixedDeltaTime);

                        playerShipEffects.ThrusterEffects(true, true);
                    }

                    else
                    {
                        playerShipStats.FuelConsumption(false);
                        currentSpeed = normalSpeed;
                        rb.AddForce(transform.up * currentSpeed * Time.fixedDeltaTime);

                        playerShipEffects.ThrusterEffects(true, false);
                    }
                }

                if (Input.GetMouseButtonUp(0) && playerShipState == PlayerShipState.inSpace)
                    playerShipEffects.ThrusterEffects(false, false);
            }

            else
                playerShipEffects.StopParticleSystem(playerShipEffects.mainThruster);
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