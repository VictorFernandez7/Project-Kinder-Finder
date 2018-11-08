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
    [Header("Ship State")]
    [SerializeField] public PlayerShipState playerShipState;

    [Header("Taking Off Parameters")]
    [SerializeField] private float takeOffDistance;
    [SerializeField] private float targetVelocity;
    [SerializeField] private float takingOffTime;
    [Range(500, 1250)] [SerializeField] private float dustMultiplier;
    [Range(350, 2000)] [SerializeField] private float thrusterPower;

    [Header("Landed Parameters")]
    [SerializeField] private float canControlTimer;
    [SerializeField] private float shipOrientationSpeed;

    [Header("Landing Parameters")]
    [SerializeField] public float landDistance;
    [SerializeField] private float landingTime;
    [SerializeField] private LayerMask planetLayer;

    [Header("In Space Parameters")]    
    [SerializeField] private float rotationDelay;

    [Header("Speed System")]
    [SerializeField] public float maxSpeed;
    [SerializeField] private float normalSpeed;
    [SerializeField] private float boostSpeed;
    [SerializeField] private float limitUnits;

    [Header("References")]
    [SerializeField] public ParticleSystem thrusterParticles;
    [SerializeField] private GameObject dustParticles;
    [SerializeField] private Transform endOfShip;

    [HideInInspector] public bool astronautOnBoard;
    [HideInInspector] public bool onGround;
    [HideInInspector] public bool takingOff;
    [HideInInspector] public bool landing;
    [HideInInspector] public bool dead;
    [HideInInspector] public bool canRotateShip;
    [HideInInspector] public bool canControlShip = true;
    [HideInInspector] public GameObject currentPlanet;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Camera mainCamera;

    private bool countDownToMove;
    private float maxSpeedSaved;
    private float currentSpeed;
    private float canControlTimerSaved;
    private Vector3 targetTakingOff;
    private Vector3 targetLanding;
    private TrailRenderer trailRenderer;
    private TextMeshProUGUI limiterText;
    private TextMeshProUGUI speedText;
    private Scr_PlayerShipStats playerShipStats;
    private Scr_PlayerShipActions playerShipActions;
    private Scr_AstronautMovement astronautMovement;    

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
        astronautMovement = GameObject.Find("Astronaut").GetComponent<Scr_AstronautMovement>();

        rb = GetComponent<Rigidbody2D>();
        playerShipStats = GetComponent<Scr_PlayerShipStats>();
        playerShipActions = GetComponent<Scr_PlayerShipActions>();
        trailRenderer = GetComponent<TrailRenderer>();

        canControlTimerSaved = canControlTimer;
        maxSpeedSaved = maxSpeed;
    }

    private void Update()
    {
        speedText.text = ((int)(rb.velocity.magnitude * 10)).ToString();

        ShipControl();
        SpeedLimiter();
        Timers();

        if (currentPlanet != null)
        {
            trailRenderer.enabled = false;

            RaycastHit2D distanceHit = Physics2D.Raycast(endOfShip.position, -endOfShip.up, landDistance, planetLayer);

            if (distanceHit && playerShipState == PlayerShipState.inSpace)
            {
                mainCamera.GetComponent<Scr_CameraFollow>().smoothRotation = true;

                Landing();
            }
        }

        else
        {
            playerShipState = PlayerShipState.inSpace;

            trailRenderer.enabled = true;

        }

        if (onGround && Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(0) && playerShipState == PlayerShipState.landed)
        {
            mainCamera.GetComponent<Scr_CameraFollow>().smoothRotation = false;

            TakingOff();
        }

        if (onGround)
        {
            playerShipState = PlayerShipState.landed;

            Landed();
            OnGroundEffects();
            LandingEffects(false);
        }

        if (takingOff)
        {
            playerShipState = PlayerShipState.takingOff;
            canRotateShip = false;
            canControlShip = false;

            rb.velocity = Vector2.Lerp(rb.velocity, targetVelocity * transform.up, Time.deltaTime * takingOffTime);

            TakingOffEffects(true);

            if (currentPlanet == null)
            {
                transform.SetParent(null);
                TakingOffEffects(false);

                canRotateShip = true;
                canControlShip = true;

                takingOff = false;
            }
        }

        if (landing)
        {
            playerShipState = PlayerShipState.landing;
            canRotateShip = true;

            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.deltaTime * landingTime);

            LandingEffects(true);

            if (onGround)
            {
                LandingEffects(false);

                landing = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Planet" && !dead)
        {
            currentPlanet = collision.gameObject;
            astronautMovement.currentPlanet = collision.gameObject;

            countDownToMove = true;
            onGround = true;

            playerShipActions.canExitShip = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Planet" && !dead)
        {
            onGround = false;

            playerShipActions.canExitShip = false;
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

    private void Landed()
    {
        Vector3 direction = -new Vector3(currentPlanet.transform.position.x - transform.position.x, currentPlanet.transform.position.y - transform.position.y, currentPlanet.transform.position.z - transform.position.z);

        transform.up = Vector3.Lerp(transform.up, direction, Time.deltaTime * shipOrientationSpeed);
        transform.SetParent(currentPlanet.transform);

        if (rb.velocity != Vector2.zero)
            rb.velocity = Vector2.zero;
    }

    void TakingOff()
    {
        targetTakingOff = transform.position + new Vector3(0, takeOffDistance, 0);

        takingOff = true;
    }

    private void Landing()
    {
        transform.SetParent(currentPlanet.transform);

        RaycastHit2D landingHit = Physics2D.Raycast(endOfShip.position, -endOfShip.up, landDistance, planetLayer);

        if (landingHit && playerShipState == PlayerShipState.inSpace)
            targetLanding = landingHit.point;

        landing = true;
    }

    private void SpeedLimiter()
    {
        maxSpeedSaved += Input.GetAxis("Mouse ScrollWheel") * limitUnits;
        maxSpeedSaved = Mathf.Clamp(maxSpeedSaved, 0f, maxSpeed);
        maxSpeedSaved = (float)((int)maxSpeedSaved);

        if ((rb.velocity.magnitude * 10) > maxSpeedSaved)
            rb.AddForce(-rb.velocity.normalized * currentSpeed * Time.fixedDeltaTime);
    }

    private void ShipControl()
    {
        if (canControlShip && astronautOnBoard && !onGround)
        {
            if (canRotateShip)
            {
                Vector3 difference = (mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position);
                difference.Normalize();
                float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0f, 0f, rotationZ - 90), rotationDelay);
            }

            if (playerShipStats.currentFuel > 0)
            {
                if (Input.GetMouseButton(0))
                {
                    thrusterParticles.Play();

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

                else
                    thrusterParticles.Stop();
            }
        }
    }

    private void OnGroundEffects()
    {
        if (playerShipStats.currentFuel > 0)
        {
            if (Input.GetMouseButton(0))
            {
                thrusterParticles.Play();

                playerShipStats.FuelConsumption(false);
            }

            else
                thrusterParticles.Stop();
        }
    }

    private void TakingOffEffects(bool play)
    {
        if (play)
        {
            RaycastHit2D takingOffHit = Physics2D.Raycast(endOfShip.position, -endOfShip.up, Mathf.Infinity, planetLayer);

            if (takingOffHit)
            {
                float dustPower = Vector2.Distance(endOfShip.position, takingOffHit.point);

                thrusterParticles.Play();

                if (GameObject.Find("DustParticles(Clone)") == null)
                    Instantiate(dustParticles, takingOffHit.point, gameObject.transform.rotation);

                GameObject currentDustParticles = GameObject.Find("DustParticles(Clone)");

                ParticleSystem dustParticles1 = currentDustParticles.transform.GetChild(0).GetComponent<ParticleSystem>();
                ParticleSystem dustParticles2 = currentDustParticles.transform.GetChild(1).GetComponent<ParticleSystem>();

                if (currentPlanet != null)
                    currentDustParticles.transform.SetParent(currentPlanet.transform);

                dustParticles1.Play();
                dustParticles2.Play();

                var emission1 = dustParticles1.emission;
                var emission2 = dustParticles2.emission;
                var emission3 = thrusterParticles.emission;

                emission1.rateOverTime = (2.5f - dustPower) * dustMultiplier;
                emission2.rateOverTime = (2.5f - dustPower) * dustMultiplier;
                emission3.rateOverTime = thrusterPower;
            }
        }

        else
        {
            GameObject currentDustParticles = GameObject.Find("DustParticles(Clone)");

            thrusterParticles.Stop();

            Destroy(currentDustParticles);
        }
    }

    private void LandingEffects(bool play)
    {
        if (play)
        {
            RaycastHit2D landingHit = Physics2D.Raycast(endOfShip.position, -endOfShip.up, Mathf.Infinity, planetLayer);

            if (landingHit)
            {
                float dustPower = Vector2.Distance(endOfShip.position, landingHit.point);

                if (GameObject.Find("DustParticles(Clone)") == null)
                    Instantiate(dustParticles, landingHit.point, gameObject.transform.rotation);

                GameObject currentDustParticles = GameObject.Find("DustParticles(Clone)");

                ParticleSystem dustParticles1 = currentDustParticles.transform.GetChild(0).GetComponent<ParticleSystem>();
                ParticleSystem dustParticles2 = currentDustParticles.transform.GetChild(1).GetComponent<ParticleSystem>();

                currentDustParticles.transform.SetParent(currentPlanet.transform);
                currentDustParticles.transform.position = landingHit.point;

                Vector2 direction = new Vector2(landingHit.point.x - currentPlanet.transform.position.x, landingHit.point.y - currentPlanet.transform.position.y);
                currentDustParticles.transform.up = direction;

                dustParticles1.Play();
                dustParticles2.Play();
                thrusterParticles.Play();

                var emission1 = dustParticles1.emission;
                var emission2 = dustParticles2.emission;

                emission1.rateOverTime = (2.5f - dustPower) * dustMultiplier;
                emission2.rateOverTime = (2.5f - dustPower) * dustMultiplier;
            }
        }

        else
        {
            GameObject currentDustParticles = GameObject.Find("DustParticles(Clone)");

            currentDustParticles = GameObject.Find("DustParticles(Clone)");

            thrusterParticles.Stop();

            Destroy(currentDustParticles);
        }
    }
}