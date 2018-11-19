using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_PlayerShipDeathCheck : MonoBehaviour
{
    Scr_PlayerShipStats playerShipStats;

    private void Start()
    {
        playerShipStats = GetComponentInParent<Scr_PlayerShipStats>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Planet"))
            playerShipStats.Death();
    }
}