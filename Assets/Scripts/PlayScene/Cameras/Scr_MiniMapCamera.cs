﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_MiniMapCamera : MonoBehaviour
{
    [Header("Minimap Parameters")]
    [SerializeField] private float zoomInSpace;
    [SerializeField] private float zoomInPlanetMult;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float followSpeed;

    [Header("References")]
    [SerializeField] private GameObject playerShip;
    [SerializeField] private GameObject mainCamera;

    private float zoomInPlanet;
    private Camera minimapCamera;
    private GameObject currentTarget;
    private Scr_PlayerShipMovement playerShipMovement;

    private void Start()
    {
        minimapCamera = GetComponent<Camera>();
        playerShipMovement = playerShip.GetComponent<Scr_PlayerShipMovement>();

        currentTarget = playerShipMovement.currentPlanet;
    }

    private void Update()
    {
        ZoomSystem();
        TargetSet();
        FollowTarget();
        RecalibrateCamera();
    }

    private void TargetSet()
    {
        if (playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.landed || playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.landing)
            currentTarget = playerShipMovement.currentPlanet;

        if (playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.takingOff || playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.inSpace)
            currentTarget = playerShip;
    }

    private void FollowTarget()
    {
        Vector3 followedTarget = new Vector3(currentTarget.transform.position.x, currentTarget.transform.position.y, -100);

        transform.position = Vector3.Lerp(transform.position, followedTarget, Time.deltaTime * (currentTarget == playerShip ? followSpeed : (followSpeed * 25f)));
    }

    private void ZoomSystem()
    {
        if (playerShipMovement.currentPlanet != null)
            zoomInPlanet = playerShipMovement.currentPlanet.transform.localScale.x * zoomInPlanetMult;

        if (playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.landed || playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.landing)
            minimapCamera.orthographicSize = Mathf.Lerp(minimapCamera.orthographicSize, zoomInPlanet, Time.deltaTime * zoomSpeed);

        if (playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.takingOff || playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.inSpace)
            minimapCamera.orthographicSize = Mathf.Lerp(minimapCamera.orthographicSize, zoomInSpace, Time.deltaTime * zoomSpeed);
    }

    private void RecalibrateCamera()
    {
        if (playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.takingOff)
            transform.rotation = mainCamera.transform.rotation;
    }
}