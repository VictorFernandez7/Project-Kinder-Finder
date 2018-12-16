using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Sun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CircleCollider2D warningTrigger;
    [SerializeField] private CircleCollider2D deathTrigger;

    private bool danger;
    private Scr_PlayerShipStats playerShipStats;

    private void Start()
    {
        playerShipStats = GameObject.Find("PlayerShip").GetComponent<Scr_PlayerShipStats>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerShip") && !danger)
        {
            danger = true;
            playerShipStats.inDanger = true;
        }

        if (collision.CompareTag("PlayerShip") && danger)
            playerShipStats.Death();

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerShip"))
        {
            playerShipStats.inDanger = false;
            danger = false;
        }
    }
}