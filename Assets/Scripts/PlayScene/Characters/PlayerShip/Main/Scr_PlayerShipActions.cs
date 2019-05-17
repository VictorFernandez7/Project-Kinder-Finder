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

    [Header("Space Walk Properties")]
    [SerializeField] public float maxDistanceOfShip;

    [Header("Deploy Values")]
    [SerializeField] private float deployDelay;

    [Header("References")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Scr_PlayerShipWarehouse playerShipWarehouse;
    [SerializeField] private Transform astronautPickUp;
    [SerializeField] private GameObject mapVisuals;
    [SerializeField] private Transform miningLaserStart;
    [SerializeField] private LineRenderer miningLaser;
    [SerializeField] private DistanceJoint2D spaceWalkCable;
    [SerializeField] private Animator interactionIndicatorAnim;
    [SerializeField] private BoxCollider2D spaceCollider;
    [SerializeField] public GameObject astronaut;
    [SerializeField] private Slider miningSlider;
    [SerializeField] private Image miningFill;
    [SerializeField] private TextMeshProUGUI miningPowerText;
    [SerializeField] public Scr_MainCamera mainCamera;
    [SerializeField] private Scr_GameManager gameManager;
    [SerializeField] private GameObject lineRenderer;
    [SerializeField] public GameObject IA;
    [SerializeField] private GameObject normalSuit;
    [SerializeField] private GameObject lowTemperatureSuit;
    [SerializeField] private GameObject highTemperatureSuit;
    [SerializeField] private GameObject toxicSuit;
    [SerializeField] private Transform IAStpot;
    [SerializeField] private Scr_DevTools devTools;

    [HideInInspector] public bool startExitDelay;
    [HideInInspector] public bool closeToAsteroid;
    [HideInInspector] public bool doingSpaceWalk;
    [HideInInspector] public bool unlockedMiningLaser;
    [HideInInspector] public bool unlockedSpaceWalk;
    [HideInInspector] public GameObject currentAsteroid;
    [HideInInspector] public Vector3 laserHitPosition;
    [HideInInspector] public bool[] unlockedSuits = new bool[3];
    [HideInInspector] public bool canInputAgain;

    private float laserRange;
    private float deployDelaySaved;
    private float holdInputTime = 0.9f;
    public bool canExitShip;
    public bool unlockInteract;
    private bool toolPanel;
    private bool doneOnce;
    private int system = 0;
    private int galaxy = 0;
    private Vector3 lastFramePlanetPosition;
    private Rigidbody2D playerShipRb;
    private Rigidbody2D astronautRb;
    private Scr_CableVisuals cableVisuals;
    private Scr_PlayerShipMovement playerShipMovement;
    private Scr_PlayerShipPrediction playerShipPrediction;
    private Scr_PlayerShipProxCheck playerShipProxCheck;

    public enum Suit
    {
        SpaceSuit,
        HotResistance,
        ColdResistance,
        ToxicResistance
    }

    private void Start()
    {
        playerShipMovement = GetComponent<Scr_PlayerShipMovement>();
        playerShipPrediction = GetComponent<Scr_PlayerShipPrediction>();
        cableVisuals = GetComponentInChildren<Scr_CableVisuals>();
        playerShipProxCheck = GetComponentInChildren<Scr_PlayerShipProxCheck>();
        playerShipRb = GetComponent<Rigidbody2D>();
        astronautRb = astronaut.GetComponent<Rigidbody2D>();

        unlockedMiningLaser = false;
        unlockedSpaceWalk = false;
        canInputAgain = true;

        deployDelaySaved = deployDelay;
        miningSlider.maxValue = maxPower;

        Scr_PlayerData.checkpointPlanet = playerShipMovement.currentPlanet.transform;
        Scr_PlayerData.checkpointPlayershipPosition = transform.localPosition;
        Scr_PlayerData.checkpointPlayershipRotation = transform.localRotation;
        Scr_PlayerData.checkpointFuel = GetComponent<Scr_PlayerShipStats>().currentFuel;
        Scr_PlayerData.checkpointShield = GetComponent<Scr_PlayerShipStats>().currentShield;
    }

    private void Update()
    {
        MiningSliderColor();
        CheckInputs();
        ExitShipControl();
        ColliderUpdate();

        if (doingSpaceWalk && currentAsteroid != null)
            transform.up = currentAsteroid.transform.position - transform.position;

        if (Input.GetKeyUp(KeyCode.E) && playerShipMovement.astronautOnBoard)
            unlockInteract = true;
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

    private void ColliderUpdate()
    {
        if (doingSpaceWalk)
            spaceCollider.enabled = true;

        else
            spaceCollider.enabled = false;
    }

    private void CheckInputs()
    {
        if (playerShipMovement.astronautOnBoard)
        {
            if (Input.GetButton("Interact") && unlockInteract && playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.landed && !astronaut.activeInHierarchy && canExitShip)
            { 
                holdInputTime -= Time.deltaTime;
                interactionIndicatorAnim.gameObject.SetActive(true);

                if (holdInputTime <= 0 && canInputAgain)
                {
                    canInputAgain = false;
                    interactionIndicatorAnim.gameObject.SetActive(false);

                    if (playerShipMovement.currentPlanet.GetComponent<Scr_Planet>().blockType == Scr_Planet.BlockType.HighTemperature && !unlockedSuits[0])
                    {
                        //reaccion CANT GO HOT PLANET
                    }

                    else if (playerShipMovement.currentPlanet.GetComponent<Scr_Planet>().blockType == Scr_Planet.BlockType.LowTemperature && !unlockedSuits[1])
                    {
                        //reaccion CANT GO COLD PLANET
                    }

                    else if (playerShipMovement.currentPlanet.GetComponent<Scr_Planet>().blockType == Scr_Planet.BlockType.Toxic && !unlockedSuits[2])
                    {
                        //reaccion CANT GO TOXIC PLANET
                    }

                    else
                    {
                        if (playerShipMovement.currentPlanet.GetComponent<Scr_Planet>().blockType == Scr_Planet.BlockType.HighTemperature)
                            DeployAstronaut(Suit.HotResistance);

                        else if (playerShipMovement.currentPlanet.GetComponent<Scr_Planet>().blockType == Scr_Planet.BlockType.LowTemperature)
                            DeployAstronaut(Suit.ColdResistance);

                        else if (playerShipMovement.currentPlanet.GetComponent<Scr_Planet>().blockType == Scr_Planet.BlockType.Toxic)
                            DeployAstronaut(Suit.ToxicResistance);

                        else
                            DeployAstronaut(Suit.SpaceSuit);
                    }

                    if (playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.inSpace && !astronaut.activeInHierarchy && !doingSpaceWalk && unlockedSpaceWalk)
                        SpaceWalk();
                }
            }

            else
            {
                canInputAgain = true;
                holdInputTime = 0.9f;
                interactionIndicatorAnim.gameObject.SetActive(false);
            }

            if (Input.GetButtonDown("Map"))
                mapVisuals.SetActive(true);

            if (closeToAsteroid)
            {
                if (Input.GetButtonUp("Interact") && holdInputTime >= 0.5f)
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

                            currentAsteroid.GetComponent<Scr_AsteroidStats>().mining = true;
                            currentAsteroid.GetComponent<Scr_AsteroidStats>().currentPower += (currentPower * Time.deltaTime);
                        }

                        else
                        {
                            miningLaser.SetPosition(1, miningLaserStart.position + transform.up * laserRange);
                            currentAsteroid.GetComponent<Scr_AsteroidStats>().mining = false;
                        }
                    }

                    else
                    {
                        miningLaser.enabled = false;
                        currentAsteroid.GetComponent<Scr_AsteroidStats>().mining = false;
                    }

                    currentPower = Mathf.Clamp(currentPower, 0, maxPower);
                    currentPower += Input.GetAxis("Mouse ScrollWheel") * 50;
                    miningSlider.value = currentPower;
                    miningPowerText.text = "" + (int)currentPower;
                }
            }
        }
    }

    private void DeployAstronaut(Suit suit)
    {
        switch (suit)
        {
            case Suit.SpaceSuit:
                normalSuit.SetActive(true);
                highTemperatureSuit.SetActive(false);
                lowTemperatureSuit.SetActive(false);
                toxicSuit.SetActive(false);
                break;

            case Suit.HotResistance:
                normalSuit.SetActive(false);
                highTemperatureSuit.SetActive(true);
                lowTemperatureSuit.SetActive(false);
                toxicSuit.SetActive(false);
                break;

            case Suit.ColdResistance:
                normalSuit.SetActive(false);
                highTemperatureSuit.SetActive(false);
                lowTemperatureSuit.SetActive(true);
                toxicSuit.SetActive(false);
                break;

            case Suit.ToxicResistance:
                normalSuit.SetActive(false);
                highTemperatureSuit.SetActive(false);
                lowTemperatureSuit.SetActive(false);
                toxicSuit.SetActive(true);
                break;
        }

        unlockInteract = false;
        astronaut.GetComponent<Scr_AstronautEffects>().breathingBool = true;
        astronaut.transform.position = spawnPoint.position;
        astronaut.GetComponent<Scr_AstronautMovement>().currentPlanet = playerShipMovement.currentPlanet;
        astronaut.transform.rotation = Quaternion.LookRotation(astronaut.transform.forward, (astronaut.transform.position - playerShipMovement.currentPlanet.transform.position));
        astronaut.SetActive(true);
        astronaut.GetComponent<Scr_AstronautMovement>().planetRotation = playerShipMovement.currentPlanet.transform.rotation;
        playerShipMovement.astronautOnBoard = false;
        playerShipMovement.canControlShip = false;
        astronaut.GetComponent<Scr_AstronautMovement>().planetPosition = lastFramePlanetPosition;
        playerShipMovement.mainCamera.GetComponent<Scr_MainCamera>().followAstronaut = true;
        GameObject ia = Instantiate(IA, IAStpot.position, IAStpot.rotation);
        GetComponent<Scr_PlayerShipStats>().narrativeManager.IAGlow = ia.GetComponent<Scr_IAMovement>().IAGlow;

        for(int i = 0; i < astronaut.GetComponent<Scr_AstronautStats>().toolSlots.Count; i++)
        {
            astronaut.GetComponent<Scr_AstronautStats>().toolSlots[i] = ia.GetComponent<Scr_IAMovement>().tools[i];
        }

        astronaut.GetComponent<Scr_AstronautsActions>().solidTool = ia.GetComponent<Scr_IAMovement>().tools[0];
        astronaut.GetComponent<Scr_AstronautsActions>().liquidTool = ia.GetComponent<Scr_IAMovement>().tools[1];
        astronaut.GetComponent<Scr_AstronautsActions>().gasTool = ia.GetComponent<Scr_IAMovement>().tools[2];
        astronaut.GetComponent<Scr_AstronautsActions>().repairingTool = ia.GetComponent<Scr_IAMovement>().tools[3];

        devTools.solidTool = ia.GetComponent<Scr_IAMovement>().tools[0];
        devTools.liquidTool = ia.GetComponent<Scr_IAMovement>().tools[1];
        devTools.gasTool = ia.GetComponent<Scr_IAMovement>().tools[2];

        astronaut.GetComponent<Scr_AstronautsActions>().iaSpotTransformDown = ia.GetComponent<Scr_IAMovement>().down;
        astronaut.GetComponent<Scr_AstronautsActions>().iaSpotTransformLeft = ia.GetComponent<Scr_IAMovement>().left;
        astronaut.GetComponent<Scr_AstronautsActions>().iaSpotTransformRight = ia.GetComponent<Scr_IAMovement>().right;
        astronaut.GetComponent<Scr_AstronautsActions>().iaSpotTransformUp = ia.GetComponent<Scr_IAMovement>().up;
        astronaut.GetComponent<Scr_AstronautsActions>().iAMovement = ia.GetComponent<Scr_IAMovement>();

        Scr_PlayerData.checkpointPlanet = playerShipMovement.currentPlanet.transform;
        Scr_PlayerData.checkpointPlayershipPosition = transform.localPosition;
        Scr_PlayerData.checkpointPlayershipRotation = transform.localRotation;
        Scr_PlayerData.checkpointFuel = GetComponent<Scr_PlayerShipStats>().currentFuel;
        Scr_PlayerData.checkpointShield = GetComponent<Scr_PlayerShipStats>().currentShield;

        playerShipMovement.currentPlanet.GetComponentInChildren<Scr_PlanetDiscovery>().PlanetExplored();
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
            playerShipPrediction.predictionTime = 0;
            mainCamera.mining = true;
            lineRenderer.SetActive(false);
            playerShipProxCheck.ClearInterface(false);
        }

        else
        {
            playerShipRb.isKinematic = false;
            playerShipMovement.canControlShip = true;
            playerShipPrediction.predictionTime = 6;
            mainCamera.mining = false;
            miningLaser.enabled = false;
            lineRenderer.SetActive(true);
            playerShipProxCheck.ClearInterface(true);
        }
    }

    private void SpaceWalk()
    {
        playerShipProxCheck.ClearInterface(false);
        doingSpaceWalk = true;
        cableVisuals.printCable = true;

        astronaut.transform.position = spawnPoint.position;
        playerShipMovement.astronautOnBoard = false;
        playerShipMovement.canControlShip = false;
        playerShipMovement.canRotateShip = false;

        if (!mainCamera.mining)
            playerShipRb.velocity = Vector2.zero;

        astronaut.SetActive(true);

        gameManager.Gravity(false);

        spaceWalkCable.connectedBody = astronautRb;

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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, maxDistanceOfShip);
    }
}