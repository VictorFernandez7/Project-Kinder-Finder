using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Atmosphere : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Scr_PlayerShipMovement playerShipMovement;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerShip")
            playerShipMovement.currentPlanet = GetComponentInParent<Scr_Planet>().gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerShip")
        {
            playerShipMovement.currentPlanet = null;
            playerShipMovement.GetComponent<Scr_PlayerShipEffects>().mainThruster.Stop();
            playerShipMovement.GetComponent<Scr_PlayerShipEffects>().takingOffSmoke.Stop();
        }
    }
}