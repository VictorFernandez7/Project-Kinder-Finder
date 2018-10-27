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
    [SerializeField] private LayerMask collisionMask;

    [Header("TakeOff Properties")]
    [SerializeField] private float takeOffTimer;
    [SerializeField] private float dustMultiplier;

    [Header("References FX")]
    [SerializeField] ParticleSystem thrusterParticles;
    [SerializeField] GameObject dustParticles;

    [Header("References")]
    [SerializeField] private GameObject mapVisuals;
    [SerializeField] private Transform endOfShip;

    [HideInInspector] public bool onBoard;
    [HideInInspector] public bool onGround;
    [HideInInspector] public bool insideAtmosphere;
    [HideInInspector] public GameObject currentPlanet;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Camera mainCamera;
    [HideInInspector] public Vector3 initialVelocity;

    private bool canMove = true;
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

    private void Start()
    {
        mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        limiterText = GameObject.Find("LimiterText").GetComponent<TextMeshProUGUI>();
        speedText = GameObject.Find("SpeedText").GetComponent<TextMeshProUGUI>();

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
                //LandingEffects(true);
            }

            else
            {
                RaycastHit2D hit = Physics2D.Raycast(endOfShip.position, -endOfShip.up, landDistance, collisionMask);

                if (hit)
                {
                    ShipLanding();
                    LandingEffects(true);
                }
            }
        }

        else
        {
            takingOff = false;
            LandingEffects(false);
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
        if (collision.gameObject.tag == "Planet")
        {
            currentPlanet = collision.gameObject;
            canMove = false;
            onGround = true;
            playerShipActions.canExitShip = true;
            //LandingEffects(false);

            if ((rb.velocity.magnitude * 10) >= deathSpeed)
                playerShipStats.Death();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Planet")
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

        //rb.velocity = Vector3.Lerp(initialVelocity, Vector3.zero, Time.deltaTime);

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
        if (!canMove)
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
                        if (insideAtmosphere)
                        {
                            takingOff = true;
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

    private void LandingEffects(bool active)
    {
        float dustPower;

        RaycastHit2D hit = Physics2D.Raycast(endOfShip.position + new Vector3(0, 2, 0), -endOfShip.up, collisionMask);

        if (hit)
        {
            dustPower = Vector2.Distance(endOfShip.position, hit.transform.position);
            /*
            var emission1 = dustParticles.transform.GetChild(0).GetComponent<ParticleSystem>().emission;
            var emission2 = dustParticles.transform.GetChild(1).GetComponent<ParticleSystem>().emission;

            emission1.rateOverTime = dustPower * dustMultiplier;
            emission2.rateOverTime = dustPower * dustMultiplier;
            */
            dustParticles.SetActive(true);
            dustParticles.transform.position = hit.transform.position;
        }
    }
}