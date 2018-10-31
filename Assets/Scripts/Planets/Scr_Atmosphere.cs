using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Atmosphere : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Scr_PlayerShipMovement playerShipMovement;

    private float playerShipMaxSpeedSaved;

    private void Start()
    {
        playerShipMaxSpeedSaved = playerShipMovement.maxSpeed;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        playerShipMovement.maxSpeed = playerShipMovement.maxSpeedAtmosphere;

        if (collision.gameObject.tag == "PlayerShip")
        {
            playerShipMovement.insideAtmosphere = true;
            playerShipMovement.currentPlanet = transform.parent.transform.parent.gameObject;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerShipMovement.initialVelocity = playerShipMovement.gameObject.GetComponent<Rigidbody2D>().velocity;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerShipMovement.maxSpeed = playerShipMaxSpeedSaved;

        if (collision.gameObject.tag == "PlayerShip")
        {
            playerShipMovement.insideAtmosphere = false;
            playerShipMovement.takingOffParticles = false;
        }
    }
}