using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Scr_PlayerShipActions : MonoBehaviour
{
    [Header("Mining System")]
    [SerializeField] public float currentPower;
    [SerializeField] private float maxPower;
    [SerializeField] private LayerMask miningMask;
    [SerializeField] private Color powerColor0;
    [SerializeField] private Color powerColor25;
    [SerializeField] private Color powerColor50;
    [SerializeField] private Color powerColor75;

    [Header("Deploy Values")]
    [SerializeField] private float deployDelay;

    [Header("References")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform astronautPickUp;
    [SerializeField] private GameObject mapVisuals;
    [SerializeField] private Transform miningLaserStart;
    [SerializeField] private LineRenderer miningLaser;

    [Header("Audio")]
    [SerializeField] private AudioSource getOutTheShipSound;
    [SerializeField] private DistanceJoint2D spaceWalkCable;

    [HideInInspector] public bool startExitDelay;
    [HideInInspector] public bool closeToAsteroid;
    [HideInInspector] public bool doingSpaceWalk;
    [HideInInspector] public GameObject currentAsteroid;
    [HideInInspector] public Vector3 laserHitPosition;

    private float laserRange;
    private float deployDelaySaved;
    private bool canExitShip;
    private bool toolPanel;
    private bool doneOnce;
    private Image miningFill;
    private Slider miningSlider;
    private Vector3 lastFramePlanetPosition;
    private GameObject astronaut;
    private Rigidbody2D playerShipRb;
    private Rigidbody2D astronautRb;
    private Scr_MainCamera mainCamera;
    private TextMeshProUGUI miningPowerText;
    private Scr_PlayerShipMovement playerShipMovement;
    private Scr_PlayerShipPrediction playerShipPrediction;
    private Scr_PlayerShipEffects playerShipEffects;

    private void Start()
    {
        astronaut = GameObject.Find("Astronaut");
        miningSlider = GameObject.Find("MiningSlider").GetComponent<Slider>();
        miningFill = GameObject.Find("MiningFill").GetComponent<Image>();
        miningPowerText = GameObject.Find("Power").GetComponent<TextMeshProUGUI>();
        mainCamera = GameObject.Find("MainCamera").GetComponent<Scr_MainCamera>();

        playerShipMovement = GetComponent<Scr_PlayerShipMovement>();
        playerShipPrediction = GetComponent<Scr_PlayerShipPrediction>();
        playerShipEffects = GetComponent<Scr_PlayerShipEffects>();
        playerShipRb = GetComponent<Rigidbody2D>();
        astronautRb = astronaut.GetComponent<Rigidbody2D>();
        
        deployDelaySaved = deployDelay;
        miningSlider.maxValue = maxPower;
    }

    private void Update()
    {
        MiningSliderColor();
        CheckInputs();
        ExitShipControl();
        SpaceWalk();

        if (doingSpaceWalk && currentAsteroid != null)
            transform.up = currentAsteroid.transform.position - transform.position;
    }

    private void FixedUpdate()
    {
        if (playerShipMovement.currentPlanet != null)
            lastFramePlanetPosition = playerShipMovement.currentPlanet.transform.position;
    }

    private void ExitShipControl()
    {
        if (playerShipMovement.astronautOnBoard)
        {
            if (startExitDelay)
            {
                canExitShip = false;

                deployDelaySaved -= Time.deltaTime;

                if (deployDelaySaved <= 0)
                {
                    deployDelaySaved = deployDelay;

                    canExitShip = true;
                    startExitDelay = false;
                }
            }
        }

        if (playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.inSpace)
            canExitShip = false;
    }

    private void CheckInputs()
    {
        if (playerShipMovement.astronautOnBoard)
        {
            if (Input.GetButtonDown("Interact") && !astronaut.activeInHierarchy && canExitShip)
                DeployAstronaut();

            if (Input.GetButtonDown("Map"))
                mapVisuals.SetActive(true);

            if (closeToAsteroid)
            {
                if (Input.GetButtonDown("Interact"))
                {
                    if (playerShipRb.isKinematic)
                        MiningState(false);

                    else
                        MiningState(true);
                }

                if (mainCamera.mining)
                {
                    laserRange = Vector3.Distance(transform.position, currentAsteroid.transform.position);

                    if (Input.GetMouseButton(0) && currentPower > 0)
                    {
                        miningLaser.enabled = true;
                        miningLaser.SetPosition(0, miningLaserStart.position);

                        RaycastHit2D laserHit = Physics2D.Raycast(miningLaserStart.position, transform.up, laserRange, miningMask);

                        if (laserHit)
                        {
                            miningLaser.SetPosition(1, laserHit.point);
                            laserHitPosition = laserHit.point;

                            playerShipEffects.MiningEffects(true);

                            currentAsteroid.GetComponent<Scr_AsteroidStats>().mining = true;

                            currentAsteroid.GetComponent<Scr_AsteroidStats>().currentPower += (currentPower * Time.deltaTime);
                        }

                        else
                        {
                            miningLaser.SetPosition(1, miningLaserStart.position + transform.up * laserRange);

                            currentAsteroid.GetComponent<Scr_AsteroidStats>().mining = false;

                            playerShipEffects.MiningEffects(false);
                        }
                    }

                    else
                    {
                        miningLaser.enabled = false;

                        currentAsteroid.GetComponent<Scr_AsteroidStats>().mining = false;

                        playerShipEffects.MiningEffects(false);
                    }

                    currentPower = Mathf.Clamp(currentPower, 0, maxPower);
                    currentPower += Input.GetAxis("Mouse ScrollWheel") * 50;
                    miningSlider.value = currentPower;
                    miningPowerText.text = "" + (int)currentPower;
                }
            }
        }
    }

    private void DeployAstronaut()
    {
        astronaut.transform.position = spawnPoint.position;
        astronaut.SetActive(true);
        astronaut.transform.rotation = Quaternion.LookRotation(astronaut.transform.forward, (astronaut.transform.position - playerShipMovement.currentPlanet.transform.position));
        astronaut.GetComponent<Scr_AstronautMovement>().keep = false;
        astronaut.GetComponent<Scr_AstronautMovement>().planetRotation = playerShipMovement.currentPlanet.transform.rotation;
        playerShipMovement.astronautOnBoard = false;
        playerShipMovement.canControlShip = false;
        astronaut.GetComponent<Scr_AstronautMovement>().planetPosition = lastFramePlanetPosition;
        astronaut.GetComponent<Scr_AstronautMovement>().onGround = true;
        playerShipMovement.mainCamera.GetComponent<Scr_MainCamera>().followAstronaut = true;
        getOutTheShipSound.Play();

        for (int i = 0; i < astronaut.GetComponent<Scr_AstronautStats>().toolSlots.Length; i++)
        {
            if (astronaut.GetComponent<Scr_AstronautStats>().toolSlots[i] != null)
            {
                astronaut.GetComponent<Scr_AstronautStats>().physicToolSlots[i] = Instantiate(astronaut.GetComponent<Scr_AstronautStats>().toolSlots[i], astronautPickUp);
                astronaut.GetComponent<Scr_AstronautStats>().physicToolSlots[i].SetActive(false);
            }
        }
    }

    public void TakeTool(int warehouseNumber, int slotNumber)
    {
        astronaut.GetComponent<Scr_AstronautStats>().toolSlots[slotNumber] = GetComponent<Scr_PlayerShipStats>().toolWarehouse[warehouseNumber];
        GetComponent<Scr_PlayerShipStats>().toolWarehouse[warehouseNumber] = null;
    }

    public void SaveTool(int slotNumber, int emptyWarehouse)
    {
        GetComponent<Scr_PlayerShipStats>().toolWarehouse[emptyWarehouse] = astronaut.GetComponent<Scr_AstronautStats>().toolSlots[slotNumber];
        astronaut.GetComponent<Scr_AstronautStats>().toolSlots[slotNumber] = null;
    }

    private void MiningSliderColor()
    {
        float colorChangeSpeed = 2;

        if (miningSlider.value >= (0.75f * miningSlider.maxValue))
        {
            miningFill.color = Color.Lerp(miningFill.color, powerColor75, Time.deltaTime * colorChangeSpeed);
            miningPowerText.color = Color.Lerp(miningFill.color, powerColor75, Time.deltaTime * colorChangeSpeed);
            miningLaser.material.color = Color.Lerp(miningLaser.material.GetColor("_Color"), powerColor75, Time.deltaTime * colorChangeSpeed);
        }

        if (miningSlider.value <= (0.75f * miningSlider.maxValue))
        {
            miningFill.color = Color.Lerp(miningFill.color, powerColor50, Time.deltaTime * colorChangeSpeed);
            miningPowerText.color = Color.Lerp(miningFill.color, powerColor50, Time.deltaTime * colorChangeSpeed);
            miningLaser.material.color = Color.Lerp(miningLaser.material.GetColor("_Color"), powerColor50, Time.deltaTime * colorChangeSpeed);

        }

        if (miningSlider.value <= (0.50f * miningSlider.maxValue))
        {
            miningFill.color = Color.Lerp(miningFill.color, powerColor25, Time.deltaTime * colorChangeSpeed);
            miningPowerText.color = Color.Lerp(miningFill.color, powerColor25, Time.deltaTime * colorChangeSpeed);
            miningLaser.material.color = Color.Lerp(miningLaser.material.GetColor("_Color"), powerColor25, Time.deltaTime * colorChangeSpeed);

        }

        if (miningSlider.value <= (0.25f * miningSlider.maxValue))
        {
            miningFill.color = Color.Lerp(miningFill.color, powerColor0, Time.deltaTime * colorChangeSpeed);
            miningPowerText.color = Color.Lerp(miningFill.color, powerColor0, Time.deltaTime * colorChangeSpeed);
            miningLaser.material.color = Color.Lerp(miningLaser.material.GetColor("_Color"), powerColor0, Time.deltaTime * colorChangeSpeed);

        }
    }

    public void MiningState(bool on)
    {
        currentAsteroid.GetComponent<Scr_AsteroidBehaviour>().attached = !currentAsteroid.GetComponent<Scr_AsteroidBehaviour>().attached;

        if (on)
        {
            playerShipRb.velocity = Vector2.zero;
            playerShipRb.isKinematic = true;
            playerShipMovement.canControlShip = false;
            GetComponent<TrailRenderer>().enabled = false;
            playerShipPrediction.predictionTime = 0;
            mainCamera.mining = true;

            playerShipEffects.AttachedEffects(true);
        }

        else
        {
            playerShipRb.isKinematic = false;
            playerShipMovement.canControlShip = true;
            GetComponent<TrailRenderer>().enabled = true;
            playerShipPrediction.predictionTime = 6;
            mainCamera.mining = false;
            miningLaser.enabled = false;

            playerShipEffects.AttachedEffects(false);
        }
    }

    private void SpaceWalk()
    {
        if (playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.inSpace && !doingSpaceWalk)
        {
            if (Input.GetButtonDown("Interact"))
            {
                doingSpaceWalk = true;

                astronaut.transform.position = spawnPoint.position;
                playerShipMovement.astronautOnBoard = false;
                playerShipMovement.canControlShip = false;
                playerShipMovement.canRotateShip = false;

                if (!mainCamera.mining)
                    playerShipRb.velocity = Vector2.zero;

                astronaut.SetActive(true);

                spaceWalkCable.connectedBody = astronautRb;
            }
        }

        if (doingSpaceWalk)
        {
            if (Vector3.Distance(transform.position, astronaut.transform.position) < 1)
            {
                astronautRb.freezeRotation = false;
                playerShipRb.isKinematic = false;
            }

            else
            {
                astronautRb.freezeRotation = true;
                playerShipRb.isKinematic = true;
            }
        }
    }
}