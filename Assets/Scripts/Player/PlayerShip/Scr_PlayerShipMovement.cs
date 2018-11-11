using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
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
    [Range(350, 2000)] [SerializeField] private float takingOffThrusterPower;

    [Header("Landed Parameters")]
    [SerializeField] private float canControlTimer;
    [SerializeField] private float shipOrientationSpeed;

    [Header("Landing Parameters")]
    [SerializeField] public float landDistance;
    [SerializeField] private float landingTime;
    [Range(350, 2000)] [SerializeField] private float landingThrusterPower;
    [SerializeField] private LayerMask planetLayer;

    [Header("In Space Parameters")]
    [Range(350, 750)] [SerializeField] private float inSpaceThrusterPower;
    [SerializeField] private float rotationDelay;

    [Header("Speed System")]
    [SerializeField] public float maxSpeed;
    [SerializeField] private float normalSpeed;
    [SerializeField] private float boostSpeed;
    [SerializeField] private float limitUnits;

    [Header("Warming System")]
    [SerializeField] private float warmingAmount;
    [SerializeField] private float warmingSpeed;
    [SerializeField] private float landedThrusterMult;
    [SerializeField] private float landedDustMult;
    [SerializeField] private Color color0;
    [SerializeField] private Color color25;
    [SerializeField] private Color color50;
    [SerializeField] private Color color75;

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
    private Image warmingFill;
    private Slider speedSlider;
    private Slider limitSlider;
    private Slider warmingSlider;
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
        speedSlider = GameObject.Find("SpeedSlider").GetComponent<Slider>();
        limitSlider = GameObject.Find("LimitSlider").GetComponent<Slider>();
        warmingSlider = GameObject.Find("WarmingSlider").GetComponent<Slider>();
        warmingFill = GameObject.Find("WarmingFill").GetComponent<Image>();

        rb = GetComponent<Rigidbody2D>();
        playerShipStats = GetComponent<Scr_PlayerShipStats>();
        playerShipActions = GetComponent<Scr_PlayerShipActions>();
        trailRenderer = GetComponent<TrailRenderer>();

        canControlTimerSaved = canControlTimer;
        maxSpeedSaved = maxSpeed;
        limitSlider.maxValue = maxSpeedSaved;
        speedSlider.maxValue = maxSpeedSaved;
        warmingSlider.maxValue = warmingAmount;

        warmingSlider.gameObject.SetActive(false);
    }

    private void Update()
    {
        PlayerShipStateCheck();
        OnGroundEffects();
        Timers();

        if (playerShipState == PlayerShipState.inSpace)
        {
            ShipControl();
            SpeedLimiter();
            InSpaceEffects();
        }

        if (playerShipState == PlayerShipState.landing)
            ShipControl();

        if (playerShipState == PlayerShipState.landed)
            WarmingSliderColor();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Planet" && !dead)
        {
            currentPlanet = collision.gameObject;
            astronautMovement.currentPlanet = collision.gameObject;

            countDownToMove = true;
            onGround = true;

            playerShipActions.startExitDelay = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Planet" && !dead)
        {
            onGround = false;

            playerShipActions.startExitDelay = false;
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

    private void PlayerShipStateCheck()
    {
        if (currentPlanet != null)
        {
            trailRenderer.enabled = false;

            RaycastHit2D distanceHit = Physics2D.Raycast(endOfShip.position, -endOfShip.up, landDistance, planetLayer);

            if (distanceHit && playerShipState == PlayerShipState.inSpace)
            {
                mainCamera.GetComponent<Scr_MainCamera>().smoothRotation = true;
                mainCamera.GetComponent<Scr_MainCamera>().CameraStartShake(true);

                Landing();
            }
        }

        else
        {
            playerShipState = PlayerShipState.inSpace;

            trailRenderer.enabled = true;
        }
        
        if (Input.GetKey(KeyCode.LeftShift) && playerShipState == PlayerShipState.landed)
        {
            warmingSlider.gameObject.SetActive(true);
            warmingSlider.value += Time.deltaTime * warmingSpeed;

            if (warmingSlider.value >= (0.95f * warmingSlider.maxValue) && Input.GetMouseButtonDown(0))
            {
                warmingSlider.gameObject.SetActive(false);

                mainCamera.GetComponent<Scr_MainCamera>().smoothRotation = false;
                mainCamera.GetComponent<Scr_MainCamera>().CameraShake();

                TakingOff();
            }
        }

        else
        {
            warmingSlider.value -= Time.deltaTime;

            if (warmingSlider.value <= 0)
                warmingSlider.gameObject.SetActive(false);
        }

        if (onGround)
        {
            playerShipState = PlayerShipState.landed;

            mainCamera.GetComponent<Scr_MainCamera>().CameraStartShake(false);

            Landed();
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
        int speed = (int)(rb.velocity.magnitude * 10);

        speedSlider.value = speed;
        limitSlider.value = maxSpeedSaved;

        speedText.text = speed.ToString();
        speedText.transform.localPosition = new Vector3(0, speed * 3, 0);
        limiterText.text = maxSpeedSaved.ToString();

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

                else if (playerShipState == PlayerShipState.inSpace)
                    thrusterParticles.Stop();
            }
        }
    }

    private void WarmingSliderColor()
    {
        float colorChangeSpeed = 2;

        if (warmingSlider.value >= (0.75f * warmingSlider.maxValue))
            warmingFill.color = Color.Lerp(warmingFill.color, color75, Time.deltaTime * colorChangeSpeed);
        if (warmingSlider.value <= (0.75f * warmingSlider.maxValue))
            warmingFill.color = Color.Lerp(warmingFill.color, color50, Time.deltaTime * colorChangeSpeed);
        if (warmingSlider.value <= (0.50f * warmingSlider.maxValue))
            warmingFill.color = Color.Lerp(warmingFill.color, color25, Time.deltaTime * colorChangeSpeed);
        if (warmingSlider.value <= (0.25f * warmingSlider.maxValue))
            warmingFill.color = Color.Lerp(warmingFill.color, color0, Time.deltaTime * colorChangeSpeed);
    }

    private void OnGroundEffects()
    {
        ParticleSystem landedDustParticles1 = GameObject.Find("LandedDustParticles1").GetComponent<ParticleSystem>();
        ParticleSystem landedDustParticles2 = GameObject.Find("LandedDustParticles2").GetComponent<ParticleSystem>();

        var thrusterEmission = thrusterParticles.emission;
        var LandedDustParticles1Emission = landedDustParticles1.emission;
        var LandedDustParticles2Emission = landedDustParticles2.emission;

        if (playerShipState == PlayerShipState.landed)
        {
            thrusterParticles.Play();
            landedDustParticles1.Play();
            landedDustParticles2.Play();

            thrusterEmission.rateOverTime = warmingSlider.value * landedThrusterMult;
            LandedDustParticles1Emission.rateOverTime = warmingSlider.value * landedDustMult;
            LandedDustParticles2Emission.rateOverTime = warmingSlider.value * landedDustMult;
        }

        if (playerShipState == PlayerShipState.takingOff)
        {
            thrusterEmission.rateOverTime = takingOffThrusterPower;

            landedDustParticles1.Stop();
            landedDustParticles2.Stop();
        }
    }

    private void InSpaceEffects()
    {
        var thrusterEmission = thrusterParticles.emission;

        thrusterEmission.rateOverTime = inSpaceThrusterPower;
    }


    private void TakingOffEffects(bool play)
    {
        if (play)
        {
            RaycastHit2D takingOffHit = Physics2D.Raycast(endOfShip.position, -endOfShip.up, Mathf.Infinity, planetLayer);

            if (takingOffHit)
            {
                float dustPower = Vector2.Distance(endOfShip.position, takingOffHit.point);

                if (GameObject.Find("DustParticles(Clone)") == null)
                    Instantiate(dustParticles, takingOffHit.point, gameObject.transform.rotation);

                GameObject currentDustParticles = GameObject.Find("DustParticles(Clone)");

                ParticleSystem dustParticles1 = currentDustParticles.transform.GetChild(0).GetComponent<ParticleSystem>();
                ParticleSystem dustParticles2 = currentDustParticles.transform.GetChild(1).GetComponent<ParticleSystem>();

                if (currentPlanet != null)
                    currentDustParticles.transform.SetParent(currentPlanet.transform);

                dustParticles1.Play();
                dustParticles2.Play();
                thrusterParticles.Play();

                var emission1 = dustParticles1.emission;
                var emission2 = dustParticles2.emission;
                var emission3 = thrusterParticles.emission;

                emission1.rateOverTime = (2.5f - dustPower) * dustMultiplier;
                emission2.rateOverTime = (2.5f - dustPower) * dustMultiplier;
                emission3.rateOverTime = takingOffThrusterPower;
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
                var emission3 = thrusterParticles.emission;

                emission1.rateOverTime = (2.5f - dustPower) * dustMultiplier;
                emission2.rateOverTime = (2.5f - dustPower) * dustMultiplier;
                emission3.rateOverTime = landingThrusterPower;
            }
        }

        else
        {
            GameObject currentDustParticles = GameObject.Find("DustParticles(Clone)");

            currentDustParticles = GameObject.Find("DustParticles(Clone)");

            if (!Input.GetKey(KeyCode.LeftShift))
                thrusterParticles.Stop();

            Destroy(currentDustParticles);
        }
    }
}