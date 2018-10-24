using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private float normalSpeed;
    [SerializeField] private float deathSpeed;

    [Header("Landing Properties")]
    [SerializeField] private float landTimer;

    [Header("References")]
    [SerializeField] private ParticleSystem thrusterParticles;

    [HideInInspector] public bool onBoard;
    [HideInInspector] public GameObject currentPlanet;
    [HideInInspector] public Camera mainCamera;
    [HideInInspector] public bool onGround;

    private bool canMove = true;
    private float speedLimit;
    private float currentSpeed;
    private float landTimerSaved;
    private Rigidbody2D rb;
    private Scr_PlayerShipStats playerShipStats;
    private Scr_PlayerShipPrediction playerShipPrediction;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerShipPrediction = GetComponent<Scr_PlayerShipPrediction>();

        landTimerSaved = landTimer;
    }

    void FixedUpdate()
    {
        ShipControl();
        VelocityLimit();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Planet")
        {
            currentPlanet = collision.gameObject;
            canMove = false;
            onGround = true;

            Vector3 landDirection = (collision.gameObject.transform.position - transform.position);
            landDirection.Normalize();
            float rotationZ = Mathf.Atan2(-landDirection.y, -landDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotationZ - 90);

            if ((rb.velocity.magnitude * 10) >= deathSpeed)
                playerShipStats.Death();

            transform.SetParent(currentPlanet.transform);
            playerShipPrediction.predictionLine.enabled = false;
            playerShipPrediction.predictionLineMap.enabled = false;
            GetComponent<TrailRenderer>().enabled = false;
            GetComponentInChildren<TrailRenderer>().enabled = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Planet")
        {
            onGround = false;
            transform.SetParent(null);
            playerShipPrediction.predictionLine.enabled = true;
            playerShipPrediction.predictionLineMap.enabled = true;
            GetComponent<TrailRenderer>().enabled = true;
            GetComponentInChildren<TrailRenderer>().enabled = true;
        }
    }

    void VelocityLimit()
    {
        speedLimit += Input.GetAxis("Mouse ScrollWheel") * limitUnits;
        speedLimit = Mathf.Clamp(speedLimit, 0f, maxSpeed);
        speedLimit = (float)((int)speedLimit);

        if ((rb.velocity.magnitude * 10) > speedLimit)
        {
            rb.AddForce(-rb.velocity.normalized * currentSpeed * Time.fixedDeltaTime);
        }
    }

    void ShipControl()
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

            if (playerShipStats.fuel > 0)
            {
                if (Input.GetMouseButton(0))
                {
                    if (Input.GetKey(KeyCode.LeftShift))
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
            }
        }
    }

    void ShipLookingToMouse()
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