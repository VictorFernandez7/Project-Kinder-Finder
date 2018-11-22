﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_PlayerShipActions : MonoBehaviour
{
    [Header("Mining System")]
    [SerializeField] private float laserRange;
    [SerializeField] private LayerMask miningMask;

    [Header("Deploy Values")]
    [SerializeField] private float deployDelay;

    [Header("References")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject mapVisuals;
    [SerializeField] private GameObject toolPanelVisuals;
    [SerializeField] private Transform miningLaserStart;
    [SerializeField] private LineRenderer miningLaser;

    [Header("Audio")]
    [SerializeField] private AudioSource getOutTheShipSound;

    [HideInInspector] public bool startExitDelay;
    [HideInInspector] public bool closeToAsteroid;
    [HideInInspector] public GameObject currentAsteroid;
    [HideInInspector] public Vector3 laserHitPosition;

    private float deployDelaySaved;
    private bool canExitShip;
    private bool upgradePanel;
    private bool toolPanel;
    private Vector3 lastFramePlanetPosition;
    private Animator mainCanvasAnim;
    private Animator missionAnim;
    private Animator upgradesAnim;
    private Animator miningAnim;
    private Animator speedAnim;
    private GameObject astronaut;
    private Rigidbody2D playerShipRb;
    private Scr_MainCamera mainCamera;
    private Scr_PlayerShipMovement playerShipMovement;
    private Scr_PlayerShipPrediction playerShipPrediction;
    private Scr_PlayerShipEffects playerShipEffects;

    private void Start()
    {
        astronaut = GameObject.Find("Astronaut");
        mainCanvasAnim = GameObject.Find("MainCanvas").GetComponent<Animator>();
        missionAnim = GameObject.Find("MissionsPanels").GetComponent<Animator>();
        upgradesAnim = GameObject.Find("UpgradePanel").GetComponent<Animator>();
        miningAnim = GameObject.Find("MiningPanel").GetComponent<Animator>();
        speedAnim = GameObject.Find("SpeedPanel").GetComponent<Animator>();
        mainCamera = GameObject.Find("MainCamera").GetComponent<Scr_MainCamera>();

        playerShipMovement = GetComponent<Scr_PlayerShipMovement>();
        playerShipPrediction = GetComponent<Scr_PlayerShipPrediction>();
        playerShipEffects = GetComponent<Scr_PlayerShipEffects>();
        playerShipRb = GetComponent<Rigidbody2D>();

        deployDelaySaved = deployDelay;
    }

    private void Update()
    {
        if (playerShipMovement.astronautOnBoard)
        {
            if (Input.GetKeyDown(KeyCode.E) && !astronaut.activeInHierarchy && canExitShip)
                DeployAstronaut();

            if (Input.GetKeyDown(KeyCode.M))
                mapVisuals.SetActive(true);

            if (Input.GetKeyDown(KeyCode.J))
                missionAnim.SetTrigger("Activate");

            if (Input.GetKeyDown(KeyCode.U))
            {
                playerShipMovement.canControlShip = upgradePanel;
                upgradesAnim.SetTrigger("Show");
                upgradePanel = !upgradePanel;
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                playerShipMovement.canControlShip = toolPanel;
                toolPanelVisuals.SetActive(!toolPanel);
                toolPanel = !toolPanel;
            }

            if (closeToAsteroid)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    currentAsteroid.GetComponent<Scr_AsteroidBehaviour>().attached = !currentAsteroid.GetComponent<Scr_AsteroidBehaviour>().attached;

                    miningAnim.SetTrigger("Activate");

                    if (playerShipRb.isKinematic)
                    {
                        playerShipRb.isKinematic = false;
                        playerShipMovement.canControlShip = true;
                        GetComponent<TrailRenderer>().enabled = true;
                        playerShipPrediction.predictionTime = 6;
                        mainCamera.mining = false;
                        speedAnim.SetBool("Active", true);
                    }

                    else
                    {
                        playerShipRb.velocity = Vector2.zero;
                        playerShipRb.isKinematic = true;
                        playerShipMovement.canControlShip = false;
                        GetComponent<TrailRenderer>().enabled = false;
                        playerShipPrediction.predictionTime = 0;
                        mainCamera.mining = true;
                        speedAnim.SetBool("Active", false);
                    }
                }

                if (mainCamera.mining)
                {
                    if (Input.GetMouseButton(0))
                    {
                        miningLaser.enabled = true;
                        miningLaser.SetPosition(0, miningLaserStart.position);

                        RaycastHit2D laserHit = Physics2D.Raycast(miningLaserStart.position, transform.up, laserRange, miningMask);

                        if (laserHit)
                        {
                            //Debug.DrawRay(transform.position, transform.up * laserHit.distance, Color.yellow);

                            miningLaser.SetPosition(1, laserHit.point);
                            laserHitPosition = laserHit.point;

                            playerShipEffects.MiningEffects(true);
                        }

                        else
                        {
                            //Debug.DrawRay(transform.position, transform.up * laserRange, Color.yellow);

                            miningLaser.SetPosition(1, miningLaserStart.position + transform.up * laserRange);

                            playerShipEffects.MiningEffects(false);
                        }
                    }

                    else
                    {
                        miningLaser.enabled = false;

                        playerShipEffects.MiningEffects(false);
                    }
                }
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
        speedAnim.SetBool("Active", false);
        getOutTheShipSound.Play();
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
}
