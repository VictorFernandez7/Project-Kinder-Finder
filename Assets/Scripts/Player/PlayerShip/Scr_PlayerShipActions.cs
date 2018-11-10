﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Scr_PlayerShipPrediction))]
[RequireComponent(typeof(Scr_PlayerShipStats))]
[RequireComponent(typeof(Scr_PlayerShipMovement))]

public class Scr_PlayerShipActions : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private float deployDelay;

    [Header("References")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject mapVisuals;

    [HideInInspector] public bool startExitDelay;

    private float deployDelaySaved;
    private bool canExitShip;
    private bool questPanel;
    private Vector3 lastFramePlanetPosition;
    private Animator mainCanvasAnim;
    private GameObject astronaut;
    private Scr_PlayerShipMovement playerShipMovement;

    private void Start()
    {
        astronaut = GameObject.Find("Astronaut");
        playerShipMovement = GetComponent<Scr_PlayerShipMovement>();
        mainCanvasAnim = GameObject.Find("MainCanvas").GetComponent<Animator>();

        deployDelaySaved = deployDelay;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !astronaut.activeInHierarchy && canExitShip)
            DeployAstronaut();

        if (Input.GetKeyDown(KeyCode.M))
            mapVisuals.SetActive(true);

        if (Input.GetKeyDown(KeyCode.J) && playerShipMovement.astronautOnBoard)
        {
            mainCanvasAnim.SetBool("QuestPanel", !questPanel);
            questPanel = !questPanel;
        }
            

        if (startExitDelay)
        {
            canExitShip = false;

            deployDelay -= Time.deltaTime;

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

    void DeployAstronaut()
    {
        astronaut.transform.position = spawnPoint.position;
        astronaut.transform.rotation = transform.rotation;
        astronaut.SetActive(true);
        playerShipMovement.astronautOnBoard = false;
        astronaut.GetComponent<Scr_AstronautMovement>().planetPosition = lastFramePlanetPosition;
        astronaut.GetComponent<Scr_AstronautMovement>().onGround = true;
        playerShipMovement.mainCamera.GetComponent<Scr_CameraFollow>().followAstronaut = true;
        mainCanvasAnim.SetBool("OnBoard", false);
    }
}