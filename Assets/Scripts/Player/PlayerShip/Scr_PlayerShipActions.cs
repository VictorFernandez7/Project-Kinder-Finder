using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Scr_PlayerShipPrediction))]
[RequireComponent(typeof(Scr_PlayerShipStats))]
[RequireComponent(typeof(Scr_PlayerShipMovement))]

public class Scr_PlayerShipActions : MonoBehaviour
{
    [Header("Deploy Values")]
    [SerializeField] private float deployDelay;

    [Header("References")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject mapVisuals;
    [SerializeField] private GameObject toolPanelVisuals;

    [Header("Audio")]
    [SerializeField] private AudioSource getOutTheShipSound;

    [HideInInspector] public bool startExitDelay;

    private float deployDelaySaved;
    private bool canExitShip;
    private bool upgradePanel;
    private bool toolPanel;
    private Vector3 lastFramePlanetPosition;
    private Animator mainCanvasAnim;
    private Animator missionAnim;
    private Animator upgradesAnim;
    private GameObject astronaut;
    private Scr_PlayerShipMovement playerShipMovement;

    private void Start()
    {
        astronaut = GameObject.Find("Astronaut");
        playerShipMovement = GetComponent<Scr_PlayerShipMovement>();
        mainCanvasAnim = GameObject.Find("MainCanvas").GetComponent<Animator>();
        missionAnim = GameObject.Find("MissionsPanels").GetComponent<Animator>();
        upgradesAnim = GameObject.Find("UpgradePanel").GetComponent<Animator>();

        deployDelaySaved = deployDelay;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !astronaut.activeInHierarchy && canExitShip)
            DeployAstronaut();

        if (Input.GetKeyDown(KeyCode.M))
            mapVisuals.SetActive(true);

        if (Input.GetKeyDown(KeyCode.J) && playerShipMovement.astronautOnBoard)
            missionAnim.SetTrigger("Activate");

        if (Input.GetKeyDown(KeyCode.U) && playerShipMovement.astronautOnBoard)
        {
            playerShipMovement.canControlShip = upgradePanel;
            upgradesAnim.SetTrigger("Show");
            upgradePanel = !upgradePanel;
        }

        if(Input.GetKeyDown(KeyCode.T) && playerShipMovement.astronautOnBoard)
        {
            playerShipMovement.canControlShip = toolPanel;
            toolPanelVisuals.SetActive(!toolPanel);
            toolPanel = !toolPanel;
        }

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

    private void FixedUpdate()
    {
        if (playerShipMovement.currentPlanet != null)
            lastFramePlanetPosition = playerShipMovement.currentPlanet.transform.position;
    }

    private void DeployAstronaut()
    {
        astronaut.transform.position = spawnPoint.position;
        astronaut.SetActive(true);
        astronaut.transform.rotation = Quaternion.LookRotation(astronaut.transform.forward, (astronaut.transform.position - playerShipMovement.currentPlanet.transform.position));
        astronaut.GetComponent<Scr_AstronautMovement>().keep = false;
        playerShipMovement.astronautOnBoard = false;
        playerShipMovement.canControlShip = false;
        astronaut.GetComponent<Scr_AstronautMovement>().planetPosition = lastFramePlanetPosition;
        astronaut.GetComponent<Scr_AstronautMovement>().onGround = true;
        playerShipMovement.mainCamera.GetComponent<Scr_MainCamera>().followAstronaut = true;
        mainCanvasAnim.SetBool("OnBoard", false);
        getOutTheShipSound.Play();
    }

    public void TakeTool(int warehouseNumber, int slotNumber)
    {
        astronaut.GetComponent<Scr_AstronautStats>().toolSlots[slotNumber] = GetComponent<Scr_PlayerShipStats>().toolWarehouse[warehouseNumber];
    }

    public void SaveTool(int slotNumber, int emptyWarehouse)
    {
        GetComponent<Scr_PlayerShipStats>().toolWarehouse[emptyWarehouse] = astronaut.GetComponent<Scr_AstronautStats>().toolSlots[slotNumber];
    }
}