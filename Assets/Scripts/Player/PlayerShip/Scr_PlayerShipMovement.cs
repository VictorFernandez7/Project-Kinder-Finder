using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Scr_PlayerShipActions))]
[RequireComponent(typeof(Scr_PlayerShipPrediction))]
[RequireComponent(typeof(Scr_PlayerShipStats))]

public class Scr_PlayerShipMovement : MonoBehaviour
{
    [Header("Movement Properties")]
    [SerializeField] private float rotationDelay;

    [Header("Speed Properties")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float limitUnits;
    [SerializeField] private float boostSpeed;
    [SerializeField] private float boostSpeedPlanet;
    [SerializeField] private float normalSpeed;
    [SerializeField] private float deathSpeed;

    [Header("Landing Properties")]
    [SerializeField] private bool landingAssistance;
    [SerializeField] private float landTimer;
    [SerializeField] private float landDistance;
    [SerializeField] private float landingVelocityLerpSpeed; // Sin usar
    [SerializeField] private LayerMask collisionMask;

    [Header("TakeOff Properties")]
    [SerializeField] private float takeOffTimer;
    [SerializeField] private float dustMultiplier;
    [SerializeField] private float timeToDestroyParticles;

    [Header("References FX")]
    [SerializeField] public ParticleSystem thrusterParticles;
    [SerializeField] private GameObject dustParticles;

    [Header("References")]
    [SerializeField] private GameObject mapVisuals;
    [SerializeField] private Transform endOfShip;
    [SerializeField] private Animator planetPanel;

    [HideInInspector] public bool onBoard;
    [HideInInspector] public bool onGround;
    [HideInInspector] public bool insideAtmosphere = true;
    [HideInInspector] public bool canMove = true;
    [HideInInspector] public bool dead;
    [HideInInspector] public bool takingOffParticles;
    [HideInInspector] public GameObject currentPlanet;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Camera mainCamera;
    [HideInInspector] public Vector3 initialVelocity; // Sin usar

    private bool canRotateShip = true;
    private bool landing;
    private bool takingOff;
    private float takeOffTimerSaved;
    private bool countDownToControl;
    private float speedLimit;
    private float currentSpeed;
    private float landTimerSaved;
    private TextMeshProUGUI limiterText;
    private TextMeshProUGUI speedText;

    private Scr_PlayerShipStats playerShipStats;
    private Scr_PlayerShipPrediction playerShipPrediction;
    private Scr_PlayerShipActions playerShipActions;
    private Scr_AstronautMovement astronautMovement;

    private void Start()
    {
        mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        limiterText = GameObject.Find("LimiterText").GetComponent<TextMeshProUGUI>();
        speedText = GameObject.Find("SpeedText").GetComponent<TextMeshProUGUI>();
        astronautMovement = GameObject.Find("Astronaut").GetComponent<Scr_AstronautMovement>();

        rb = GetComponent<Rigidbody2D>();
        playerShipPrediction = GetComponent<Scr_PlayerShipPrediction>();
        playerShipStats = GetComponent<Scr_PlayerShipStats>();
        playerShipActions = GetComponent<Scr_PlayerShipActions>();

        mapVisuals.SetActive(true);

        landTimerSaved = landTimer;
        speedLimit = maxSpeed;
        takeOffTimerSaved = takeOffTimer;
    }

    private void Update()
    {
        limiterText.text = speedLimit.ToString();
        speedText.text = ((int)(rb.velocity.magnitude * 10)).ToString();

        if (insideAtmosphere)
        {
            if (takingOff)
            {
                ShipTakeOff();
                LandingEffects(true);
            }

            else
            {
                RaycastHit2D hit = Physics2D.Raycast(endOfShip.position, -endOfShip.up, landDistance, collisionMask);

                if (hit)
                {
                    ShipLanding();
                    LandingEffects(false);

                    mainCamera.GetComponent<Scr_CameraFollow>().smoothRotation = true;
                }
            }
        }

        else
        {
            takingOff = false;
        }

        if (countDownToControl)
        {
            canRotateShip = false;
            takeOffTimerSaved -= Time.deltaTime;

            if (takeOffTimerSaved <= 0)
            {
                canRotateShip = true;
                takeOffTimerSaved = takeOffTimer;
                countDownToControl = false;
            }
        }
    }

    void FixedUpdate()
    {
        ShipControl();
        SpeedLimiter();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Planet" && !dead)
        {
            currentPlanet = collision.gameObject;
            astronautMovement.currentPlanet = collision.gameObject;
            canMove = false;
            onGround = true;
            playerShipActions.canExitShip = true;

            if ((rb.velocity.magnitude * 10) >= deathSpeed)
                playerShipStats.Death();

            takingOffParticles = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Planet" && !dead)
        {
            playerShipActions.canExitShip = false;
            countDownToControl = true;
            onGround = false;

            ShipTakeOff();
        }
    }

    private void ShipLanding()
    {
        if (landingAssistance)
        {
            Vector3 landDirection = (currentPlanet.gameObject.transform.position - transform.position);
            landDirection.Normalize();
            float rotationZ = Mathf.Atan2(-landDirection.y, -landDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotationZ - 90);
        }

        if (rb.velocity != Vector2.zero)
            rb.velocity = Vector2.zero;

        //rb.velocity = Vector3.Lerp(initialVelocity, Vector3.zero, Time.deltaTime * landingVelocityLerpSpeed);

        transform.SetParent(currentPlanet.transform);
        playerShipPrediction.predictionLine.enabled = false;
        playerShipPrediction.predictionLineMap.enabled = false;
        GetComponent<TrailRenderer>().enabled = false;
        GetComponentInChildren<TrailRenderer>().enabled = false;
    }

    void ShipTakeOff()
    {
        transform.SetParent(null);
        playerShipPrediction.predictionLine.enabled = true;
        playerShipPrediction.predictionLineMap.enabled = true;
        GetComponent<TrailRenderer>().enabled = true;
        GetComponentInChildren<TrailRenderer>().enabled = true;
    }

    private void SpeedLimiter()
    {
        speedLimit += Input.GetAxis("Mouse ScrollWheel") * limitUnits;
        speedLimit = Mathf.Clamp(speedLimit, 0f, maxSpeed);
        speedLimit = (float)((int)speedLimit);

        if ((rb.velocity.magnitude * 10) > speedLimit)
        {
            rb.AddForce(-rb.velocity.normalized * currentSpeed * Time.fixedDeltaTime);
        }
    }

    private void ShipControl()
    {
        if (!canMove && !dead)
        {
            landTimer -= Time.fixedDeltaTime;

            if (landTimer <= 0)
            {
                canMove = true;
                landTimer = landTimerSaved;
            }
        }

        if (canMove && onBoard)
        {
            if (!onGround)
                ShipLookingToMouse();

            if (playerShipStats.currentFuel > 0)
            {
                if (Input.GetMouseButton(0))
                {
                    thrusterParticles.Play();

                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        takingOff = true;

                        if (insideAtmosphere)
                        {
                            playerShipStats.FuelConsumption(true);
                            currentSpeed = boostSpeedPlanet;
                            rb.AddForce(transform.up * currentSpeed * Time.fixedDeltaTime);
                        }

                        else
                        {
                            playerShipStats.FuelConsumption(true);
                            currentSpeed = boostSpeed;
                            rb.AddForce(transform.up * currentSpeed * Time.fixedDeltaTime);
                        }
                    }

                    else
                    {
                        playerShipStats.FuelConsumption(false);
                        currentSpeed = normalSpeed;
                        rb.AddForce(transform.up * currentSpeed * Time.fixedDeltaTime);
                    }

                    if (onGround)
                        LandingEffects(true);
                }

                else
                    thrusterParticles.Stop();
            }
        }
    }

    private void ShipLookingToMouse()
    {
        if (canRotateShip)
        {
            Vector3 difference = (mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position);
            difference.Normalize();
            float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0f, 0f, rotationZ - 90), rotationDelay);
        }
    }

    private void LandingEffects(bool shipTakingOff)
    {
        if (shipTakingOff)
        {
            RaycastHit2D takingOffHit = Physics2D.Raycast(endOfShip.position, -endOfShip.up, Mathf.Infinity, collisionMask);

            if (takingOffHit)
            {
                if (Vector2.Distance(endOfShip.position, takingOffHit.point) >= 0.000000005)
                {
                    mainCamera.GetComponent<Scr_CameraFollow>().smoothRotation = false;

                    float dustPower;
                    ParticleSystem dustParticles1;
                    ParticleSystem dustParticles2;

                    dustPower = Vector2.Distance(endOfShip.position, takingOffHit.point);

                    if (GameObject.Find("DustParticles(Clone)") == null)
                        Instantiate(dustParticles, takingOffHit.point, gameObject.transform.rotation);

                    GameObject currentDustParticles = GameObject.Find("DustParticles(Clone)");
                    dustParticles1 = currentDustParticles.transform.GetChild(0).GetComponent<ParticleSystem>();
                    dustParticles2 = currentDustParticles.transform.GetChild(1).GetComponent<ParticleSystem>();

                    currentDustParticles.transform.SetParent(currentPlanet.transform);

                    dustParticles1.Play();
                    dustParticles2.Play();

                    var emission1 = dustParticles1.emission;
                    var emission2 = dustParticles2.emission;

                    emission1.rateOverTime = (1 - dustPower) * dustMultiplier;
                    emission2.rateOverTime = (1 - dustPower) * dustMultiplier;

                    Destroy(currentDustParticles, timeToDestroyParticles);

                    planetPanel.SetTrigger("TookOff");
                }
            }
        }

        else
        {
            RaycastHit2D landingHit = Physics2D.Raycast(endOfShip.position, -endOfShip.up, Mathf.Infinity, collisionMask);

            if (landingHit)
            {
                if (Vector2.Distance(endOfShip.position, landingHit.point) >= 0.000000005)
                {
                    float dustPower;
                    Vector2 direction;
                    ParticleSystem dustParticles1;
                    ParticleSystem dustParticles2;

                    dustPower = Vector2.Distance(endOfShip.position, landingHit.point);

                    if (GameObject.Find("DustParticles(Clone)") == null)
                        Instantiate(dustParticles, landingHit.point, gameObject.transform.rotation);

                    GameObject currentDustParticles = GameObject.Find("DustParticles(Clone)");
                    dustParticles1 = currentDustParticles.transform.GetChild(0).GetComponent<ParticleSystem>();
                    dustParticles2 = currentDustParticles.transform.GetChild(1).GetComponent<ParticleSystem>();

                    currentDustParticles.transform.SetParent(currentPlanet.transform);
                    currentDustParticles.transform.position = landingHit.point;

                    direction = new Vector2(landingHit.point.x - currentPlanet.transform.position.x, landingHit.point.y - currentPlanet.transform.position.y);
                    currentDustParticles.transform.up = direction;

                    dustParticles1.Play();
                    dustParticles2.Play();
                    thrusterParticles.Play();

                    var emission1 = dustParticles1.emission;
                    var emission2 = dustParticles2.emission;

                    emission1.rateOverTime = (1 - dustPower) * dustMultiplier;
                    emission2.rateOverTime = (1 - dustPower) * dustMultiplier;
                }

                if (onGround && !Input.GetMouseButton(0))
                {
                    GameObject currentDustParticles = GameObject.Find("DustParticles(Clone)");

                    thrusterParticles.Stop();
                    Destroy(currentDustParticles);
                }
            }
        }
    }
}