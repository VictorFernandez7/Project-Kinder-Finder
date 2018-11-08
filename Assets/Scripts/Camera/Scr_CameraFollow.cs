using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_CameraFollow : MonoBehaviour
{
    [Header("Camera Zoom Properties")]
    [SerializeField] private float zoomInSpace;
    [SerializeField] private float zoomInPlanet;
    [SerializeField] private float zoomSpeed;

    [Header("Camera Rotation Properties")]
    [SerializeField] private float shipRotationSpeed;
    [SerializeField] private float astronautRotationSpeed;

    [HideInInspector] public bool followAstronaut = true;
    [HideInInspector] public bool smoothRotation = true;

    private float angleDifference;
    private float zoomDistance;
    private Camera mainCamera;
    private GameObject currentPlanet;
    private GameObject playerShip;
    private GameObject astronaut;
    private Scr_PlayerShipMovement playerShipMovement;

    private void Start()
    {
        playerShip = GameObject.Find("PlayerShip");
        playerShipMovement = playerShip.GetComponent<Scr_PlayerShipMovement>();
        astronaut = GameObject.Find("Astronaut");
        mainCamera = GetComponent<Camera>();

        mainCamera.orthographicSize = zoomInPlanet;
        smoothRotation = true;
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

    private void CameraRotation()
    {
        if (smoothRotation)
        {
            Vector3 astronautUpVector = astronaut.transform.up;
            Vector3 playerShipVectorUp = playerShip.transform.up;

            transform.up = Vector3.Lerp(transform.up, followAstronaut ? astronautUpVector : playerShipVectorUp, Time.deltaTime * (followAstronaut ? astronautRotationSpeed : shipRotationSpeed));
        }
    }

    private void CameraZoom()
    {
        if (playerShipMovement.takingOff || playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.inSpace)
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, zoomInSpace, Time.deltaTime * zoomSpeed);


        if (playerShipMovement.landing || playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.landed)
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, zoomInPlanet, Time.deltaTime * zoomSpeed);
    }
}