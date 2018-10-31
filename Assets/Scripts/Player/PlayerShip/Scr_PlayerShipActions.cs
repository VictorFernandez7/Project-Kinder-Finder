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
    [Header("References")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject mapVisuals;

    [HideInInspector] public bool canExitShip;

    private Vector3 lastFramePlanetPosition;
    private GameObject astronaut;
    private Scr_PlayerShipMovement playerShipMovement;

    private void Start()
    {
        astronaut = GameObject.Find("Astronaut");
        playerShipMovement = GetComponent<Scr_PlayerShipMovement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !astronaut.activeInHierarchy && canExitShip)
            DeployAstronaut();

        if (Input.GetKeyDown(KeyCode.M))
            mapVisuals.SetActive(true);
    }

    private void FixedUpdate()
    {
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
    }
}