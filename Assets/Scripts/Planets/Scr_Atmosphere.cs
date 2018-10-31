using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Atmosphere : MonoBehaviour
{
    private Scr_PlayerShipMovement playerShipMovement;

    private void Start()
    {
        playerShipMovement = GameObject.Find("PlayerShip").GetComponent<Scr_PlayerShipMovement>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerShip")
        {
            playerShipMovement.currentPlanet = transform.parent.transform.parent.gameObject;
        }
    }
}