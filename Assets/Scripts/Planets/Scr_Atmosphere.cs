using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Atmosphere : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Scr_PlayerShipMovement playerShipMovement;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerShip")
        {
            playerShipMovement.insideAtmosphere = true;
            playerShipMovement.currentPlanet = transform.parent.gameObject;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerShipMovement.initialVelocity = playerShipMovement.gameObject.GetComponent<Rigidbody2D>().velocity;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerShip")
        {
            playerShipMovement.insideAtmosphere = false;
        }
    }
}