using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_PlayerShipActions : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform spawnPoint;

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
        if (Input.GetKeyDown(KeyCode.E) && !astronaut.activeInHierarchy)
            DeployAstronaut();
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
        playerShipMovement.onBoard = false;
        astronaut.GetComponent<Scr_AstronautMovement>().planetPosition = lastFramePlanetPosition;
        astronaut.GetComponent<Scr_AstronautMovement>().onGround = true;
        playerShipMovement.mainCamera.GetComponent<Scr_CameraFollow>().followAstronaut = true;
    }
}