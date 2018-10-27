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
    [SerializeField] private TextMeshProUGUI textLimiter;
    [SerializeField] private TextMeshProUGUI textSpeed;

    [Header("Landing Properties")]
    [SerializeField] private float landTimer;
    [SerializeField] private float landDistance;
    [SerializeField] private LayerMask collisionMask;

    [Header("References")]
    [SerializeField] private ParticleSystem thrusterParticles;
    [SerializeField] private GameObject mapVisuals;
    [SerializeField] private Transform endOfShip;
    [SerializeField] public Camera mainCamera;

    [HideInInspector] public bool onBoard;
    [HideInInspector] public GameObject currentPlanet;
    [HideInInspector] public bool onGround;
    [HideInInspector] public bool insideAtmosphere;
    [HideInInspector] public Rigidbody2D rb;

    private bool canMove = true;
    private bool landing;
    private bool takingOff;
    private float speedLimit;
    private float currentSpeed;
    private float landTimerSaved;
    
    private Scr_PlayerShipStats playerShipStats;
    private Scr_PlayerShipPrediction playerShipPrediction;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerShipPrediction = GetComponent<Scr_PlayerShipPrediction>();
        playerShipStats = GetComponent<Scr_PlayerShipStats>();

        mapVisuals.SetActive(true);

        landTimerSaved = landTimer;
        speedLimit = maxSpeed;
    }

    private void Update()
    {
        textLimiter.text = speedLimit.ToString();
        textSpeed.text = ((int)(rb.velocity.magnitude * 10)).ToString();

        if (insideAtmosphere)
        {
            if (takingOff)
            {
                ShipTakeOff();
            }

            else
            {
                RaycastHit2D hit = Physics2D.Raycast(endOfShip.position, -endOfShip.up, landDistance, collisionMask);
                Debug.DrawRay(endOfShip.position, -endOfShip.transform.up * landDistance, Color.red);

                if (hit)
                    ShipLanding();
            }
        }

        if (!insideAtmosphere)
            takingOff = false;
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

            if ((rb.velocity.magnitude * 10) >= deathSpeed)
                playerShipStats.Death();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Planet")
        {
            onGround = false;

            ShipTakeOff();
        }
    }

    private void ShipLanding()
    {
        Vector3 landDirection = (currentPlanet.gameObject.transform.position - transform.position);
        landDirection.Normalize();
        float rotationZ = Mathf.Atan2(-landDirection.y, -landDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ - 90);

        if (rb.velocity != Vector2.zero)
            rb.velocity = Vector2.zero;

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
            }
        }
    }

    private void ShipLookingToMouse()
    {
        Vector3 difference = (mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position);
        difference.Normalize();
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0f, 0f, rotationZ - 90), rotationDelay);
    }

    private void ThrusterEffects()
    {
        
    }
}