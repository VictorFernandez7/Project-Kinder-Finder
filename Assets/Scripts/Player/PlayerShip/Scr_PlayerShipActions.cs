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
    [SerializeField] private GameObject astronaut;
    [SerializeField] private Slider miningSlider;
    [SerializeField] private Image miningFill;
    [SerializeField] private TextMeshProUGUI miningPowerText;
    [SerializeField] private Scr_MainCamera mainCamera;
    [SerializeField] private Scr_GameManager gameManager;
    [SerializeField] private GameObject lineRenderer;
    [SerializeField] private GameObject IA;
    [SerializeField] private Transform IAStpot;

    [HideInInspector] public bool startExitDelay;
    [HideInInspector] public bool closeToAsteroid;
    [HideInInspector] public bool doingSpaceWalk;
    [HideInInspector] public bool unlockedMiningLaser;
    [HideInInspector] public bool unlockedMultiJump;
    [HideInInspector] public bool unlockedSpaceWalk;
    [HideInInspector] public GameObject currentAsteroid;
    [HideInInspector] public Vector3 laserHitPosition;
    [HideInInspector] public bool[] unlockedSuits = new bool[3];

    private float laserRange;
    private float deployDelaySaved;
    private float holdInputTime = 0.9f;
    private bool canExitShip;
    private bool toolPanel;
    private bool doneOnce;
    private bool canInputAgain = true;
    private int system = 0;
    private int galaxy = 0;
    private Vector3 lastFramePlanetPosition;
    private Rigidbody2D playerShipRb;
    private Rigidbody2D astronautRb;
    private Scr_CableVisuals cableVisuals;
    private Scr_PlayerShipMovement playerShipMovement;
    private Scr_PlayerShipPrediction playerShipPrediction;
    private Scr_PlayerShipEffects playerShipEffects;
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
        playerShipEffects = GetComponent<Scr_PlayerShipEffects>();
        playerShipProxCheck = GetComponentInChildren<Scr_PlayerShipProxCheck>();
        playerShipRb = GetComponent<Rigidbody2D>();
        astronautRb = astronaut.GetComponent<Rigidbody2D>();

        unlockedMiningLaser = false;
        unlockedMultiJump = false;
        unlockedSpaceWalk = false;

        deployDelaySaved = deployDelay;
        miningSlider.maxValue = maxPower;
    }

    private void Update()
    {
        MiningSliderColor();
        CheckInputs();
        ExitShipControl();
        ColliderUpdate();

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
            if (Input.GetButton("Interact"))
            {
                holdInputTime -= Time.deltaTime;
                interactionIndicatorAnim.gameObject.SetActive(true);

                if (holdInputTime <= 0 && canInputAgain)
                {
                    canInputAgain = false;
                    interactionIndicatorAnim.gameObject.SetActive(false);

                    if (playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.landed && !astronaut.activeInHierarchy && canExitShip)
                    {
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

    private void DeployAstronaut(Suit suit)
    {
        astronaut.GetComponent<Scr_AstronautEffects>().breathingBool = true;
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
        Instantiate(IA, IAStpot.position, IAStpot.rotation);
        playerShipMovement.currentPlanet.GetComponentInParent<Scr_PlanetDiscovery>().explored = true;
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
            playerShipProxCheck.ClearInterface(false);
            playerShipRb.velocity = Vector2.zero;
            playerShipRb.isKinematic = true;
            playerShipMovement.canControlShip = false;
            playerShipPrediction.predictionTime = 0;
            mainCamera.mining = true;
            lineRenderer.SetActive(false);
            playerShipProxCheck.ClearInterface(false);

            playerShipEffects.AttachedEffects(true);
        }

        else
        {
            playerShipProxCheck.ClearInterface(true);
            playerShipRb.isKinematic = false;
            playerShipMovement.canControlShip = true;
            playerShipPrediction.predictionTime = 6;
            mainCamera.mining = false;
            miningLaser.enabled = false;
            lineRenderer.SetActive(true);
            playerShipProxCheck.ClearInterface(true);

            playerShipEffects.AttachedEffects(false);
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

    public void JumpTravel(bool multiJump, Scr_Levels.Galaxies targetGalaxy, Scr_Levels.LevelToLoad targetSystem)
    {
        if (multiJump)
        {
            switch (targetGalaxy)
            {
                case Scr_Levels.Galaxies.Galaxy1:
                    if(Scr_Levels.currentGalaxy == Scr_Levels.Galaxies.Galaxy1)
                    {
                        if((Scr_Levels.currentGalaxy == Scr_Levels.Galaxies.Galaxy2 && playerShipWarehouse.jumpCellAmount >= Scr_LevelManager.travelCost0to1) || (Scr_Levels.currentGalaxy == Scr_Levels.Galaxies.Galaxy3 && playerShipWarehouse.jumpCellAmount >= Scr_LevelManager.travelCost0to2))
                        {
                            if (Scr_Levels.currentGalaxy == Scr_Levels.Galaxies.Galaxy2)
                                playerShipWarehouse.jumpCellAmount -= Scr_LevelManager.travelCost0to1;

                            else
                                playerShipWarehouse.jumpCellAmount -= Scr_LevelManager.travelCost0to2;


                            switch (targetSystem)
                            {
                                case Scr_Levels.LevelToLoad.PlanetSystem1:
                                    Scr_LevelManager.LoadPlanetSystem(Scr_Levels.LevelToLoad.PlanetSystem1);
                                    break;

                                case Scr_Levels.LevelToLoad.PlanetSystem2:
                                    Scr_LevelManager.LoadPlanetSystem(Scr_Levels.LevelToLoad.PlanetSystem2);
                                    break;
                            }
                        }
                    }
                    break;

                case Scr_Levels.Galaxies.Galaxy2:
                    if (Scr_Levels.currentGalaxy == Scr_Levels.Galaxies.Galaxy2)
                    {
                        if ((Scr_Levels.currentGalaxy == Scr_Levels.Galaxies.Galaxy1 && playerShipWarehouse.jumpCellAmount >= Scr_LevelManager.travelCost0to1) || (Scr_Levels.currentGalaxy == Scr_Levels.Galaxies.Galaxy3 && playerShipWarehouse.jumpCellAmount >= Scr_LevelManager.travelCost1to2))
                        {
                            if (Scr_Levels.currentGalaxy == Scr_Levels.Galaxies.Galaxy1)
                                playerShipWarehouse.jumpCellAmount -= Scr_LevelManager.travelCost0to1;

                            else
                                playerShipWarehouse.jumpCellAmount -= Scr_LevelManager.travelCost1to2;

                            switch (targetSystem)
                            {
                                case Scr_Levels.LevelToLoad.PlanetSystem3:
                                    Scr_LevelManager.LoadPlanetSystem(Scr_Levels.LevelToLoad.PlanetSystem3);
                                    break;

                                case Scr_Levels.LevelToLoad.PlanetSystem4:
                                    Scr_LevelManager.LoadPlanetSystem(Scr_Levels.LevelToLoad.PlanetSystem4);
                                    break;

                                case Scr_Levels.LevelToLoad.PlanetSystem5:
                                    Scr_LevelManager.LoadPlanetSystem(Scr_Levels.LevelToLoad.PlanetSystem5);
                                    break;
                            }
                        }
                    }
                    break;

                case Scr_Levels.Galaxies.Galaxy3:
                    if (Scr_Levels.currentGalaxy == Scr_Levels.Galaxies.Galaxy3)
                    {
                        if ((Scr_Levels.currentGalaxy == Scr_Levels.Galaxies.Galaxy1 && playerShipWarehouse.jumpCellAmount >= Scr_LevelManager.travelCost0to2) || (Scr_Levels.currentGalaxy == Scr_Levels.Galaxies.Galaxy2 && playerShipWarehouse.jumpCellAmount >= Scr_LevelManager.travelCost1to2))
                        {
                            if (Scr_Levels.currentGalaxy == Scr_Levels.Galaxies.Galaxy1)
                                playerShipWarehouse.jumpCellAmount -= Scr_LevelManager.travelCost0to2;

                            else
                                playerShipWarehouse.jumpCellAmount -= Scr_LevelManager.travelCost1to2;

                            switch (targetSystem)
                            {
                                case Scr_Levels.LevelToLoad.PlanetSystem6:
                                    Scr_LevelManager.LoadPlanetSystem(Scr_Levels.LevelToLoad.PlanetSystem6);
                                    break;

                                case Scr_Levels.LevelToLoad.PlanetSystem7:
                                    Scr_LevelManager.LoadPlanetSystem(Scr_Levels.LevelToLoad.PlanetSystem7);
                                    break;
                            }
                        }
                    }
                    break;
            }

        }

        else
        {
            switch (targetSystem)
            {
                case Scr_Levels.LevelToLoad.PlanetSystem1:
                    if(Scr_Levels.currentLevel == Scr_Levels.LevelToLoad.PlanetSystem2 && playerShipWarehouse.jumpCellAmount >= 1)
                    {
                        playerShipWarehouse.jumpCellAmount -= 1;
                        Scr_LevelManager.LoadPlanetSystem(Scr_Levels.LevelToLoad.PlanetSystem1);
                    }
                    break;

                case Scr_Levels.LevelToLoad.PlanetSystem2:
                    if (Scr_Levels.currentLevel == Scr_Levels.LevelToLoad.PlanetSystem1 && playerShipWarehouse.jumpCellAmount >= 1)
                    {
                        playerShipWarehouse.jumpCellAmount -= 1;
                        Scr_LevelManager.LoadPlanetSystem(Scr_Levels.LevelToLoad.PlanetSystem2);
                    }
                    break;

                case Scr_Levels.LevelToLoad.PlanetSystem3:
                    if ((Scr_Levels.currentLevel == Scr_Levels.LevelToLoad.PlanetSystem4 || Scr_Levels.currentLevel == Scr_Levels.LevelToLoad.PlanetSystem5) && playerShipWarehouse.jumpCellAmount >= 1)
                    {
                        playerShipWarehouse.jumpCellAmount -= 1;
                        Scr_LevelManager.LoadPlanetSystem(Scr_Levels.LevelToLoad.PlanetSystem3);
                    }
                    break;

                case Scr_Levels.LevelToLoad.PlanetSystem4:
                    if ((Scr_Levels.currentLevel == Scr_Levels.LevelToLoad.PlanetSystem3 || Scr_Levels.currentLevel == Scr_Levels.LevelToLoad.PlanetSystem5) && playerShipWarehouse.jumpCellAmount >= 1)
                    {
                        playerShipWarehouse.jumpCellAmount -= 1;
                        Scr_LevelManager.LoadPlanetSystem(Scr_Levels.LevelToLoad.PlanetSystem4);
                    }
                    break;

                case Scr_Levels.LevelToLoad.PlanetSystem5:
                    if ((Scr_Levels.currentLevel == Scr_Levels.LevelToLoad.PlanetSystem3 || Scr_Levels.currentLevel == Scr_Levels.LevelToLoad.PlanetSystem4) && playerShipWarehouse.jumpCellAmount >= 1)
                    {
                        playerShipWarehouse.jumpCellAmount -= 1;
                        Scr_LevelManager.LoadPlanetSystem(Scr_Levels.LevelToLoad.PlanetSystem5);
                    }
                    break;

                case Scr_Levels.LevelToLoad.PlanetSystem6:
                    if ((Scr_Levels.currentLevel == Scr_Levels.LevelToLoad.PlanetSystem7) && playerShipWarehouse.jumpCellAmount >= 1)
                    {
                        playerShipWarehouse.jumpCellAmount -= 1;
                        Scr_LevelManager.LoadPlanetSystem(Scr_Levels.LevelToLoad.PlanetSystem6);
                    }
                    break;

                case Scr_Levels.LevelToLoad.PlanetSystem7:
                    if ((Scr_Levels.currentLevel == Scr_Levels.LevelToLoad.PlanetSystem6) && playerShipWarehouse.jumpCellAmount >= 1)
                    {
                        playerShipWarehouse.jumpCellAmount -= 1;
                        Scr_LevelManager.LoadPlanetSystem(Scr_Levels.LevelToLoad.PlanetSystem7);
                    }
                    break;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, maxDistanceOfShip);
    }
}