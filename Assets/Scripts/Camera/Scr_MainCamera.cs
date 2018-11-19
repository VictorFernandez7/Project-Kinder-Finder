﻿using System.Collections;
using System.Collections.Generic;
using EZCameraShake;
using UnityEngine;

public class Scr_MainCamera : MonoBehaviour
{
    [Header("Zoom Properties")]
    [SerializeField] private float zoomInSpace;
    [SerializeField] private float zoomInPlanet;
    [SerializeField] private float zoomSpeed;

    [Header("Rotation Properties")]
    [SerializeField] private float shipRotationSpeed;
    [SerializeField] private float astronautRotationSpeed;

    [Header("Shake Properties")]
    [SerializeField] private float magnitude = 2f;
    [SerializeField] private float roughness = 10f;
    [SerializeField] private float fadeOutTime = 5f;

    [HideInInspector] public bool followAstronaut = true;
    [HideInInspector] public bool smoothRotation = true;

    private float angleDifference;
    private float zoomDistance;
    private Camera mainCamera;
    private GameObject currentPlanet;
    private GameObject playerShip;
    private GameObject astronaut;
    private Scr_PlayerShipMovement playerShipMovement;
    private CameraShakeInstance shakeInstance;

    private void Start()
    {
        playerShip = GameObject.Find("PlayerShip");
        playerShipMovement = playerShip.GetComponent<Scr_PlayerShipMovement>();
        astronaut = GameObject.Find("Astronaut");
        mainCamera = GetComponent<Camera>();
        desiredup = transform.up;

        mainCamera.orthographicSize = zoomInPlanet;
        smoothRotation = true;

        shakeInstance = CameraShaker.Instance.StartShake(magnitude, roughness, fadeOutTime);
        shakeInstance.StartFadeOut(0);
        shakeInstance.DeleteOnInactive = true;
    }

    private void Update()
    {
        currentPlanet = playerShipMovement.currentPlanet;
        zoomDistance = playerShipMovement.landDistance;

        FollowTarget();
        CameraZoom();
        CameraRotation();
    }

    private void FollowTarget()
    {
        if (followAstronaut)
            transform.position = new Vector3(astronaut.transform.position.x, astronaut.transform.position.y, -10);

        else
            transform.position = new Vector3(playerShip.transform.position.x, playerShip.transform.position.y, -10);
    }

    Vector3 desiredup;

    private void CameraRotation()
    {
        if (smoothRotation)
        {
            Vector3 astronautUpVector = astronaut.transform.up;
            Vector3 playerShipVectorUp = playerShip.transform.up;

            desiredup = Vector3.Lerp(desiredup, followAstronaut ? astronautUpVector : playerShipVectorUp, Time.deltaTime * (followAstronaut ? astronautRotationSpeed : shipRotationSpeed));


            transform.rotation = Quaternion.LookRotation(transform.forward, desiredup);
        }
    }

    private void CameraZoom()
    {
        if (playerShipMovement.takingOff || playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.inSpace)
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, zoomInSpace, Time.deltaTime * zoomSpeed);


        if (playerShipMovement.landing || playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.landed)
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, zoomInPlanet, Time.deltaTime * zoomSpeed);
    }

    public void CameraShake()
    {
        CameraShaker.Instance.ShakeOnce(magnitude, roughness, 0, fadeOutTime);
    }

    public void CameraStartShake(bool fadeIn)
    {
        if (fadeIn)
            shakeInstance.StartFadeIn(1f);
        else
            shakeInstance.StartFadeOut(3f);
    }
}