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
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerShip")
        {
            playerShipMovement.insideAtmosphere = false;
        }
    }
}