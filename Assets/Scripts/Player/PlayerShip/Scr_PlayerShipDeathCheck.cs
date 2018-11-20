using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_PlayerShipDeathCheck : MonoBehaviour
{
    private bool checkCollisions;
    private Scr_PlayerShipStats playerShipStats;
    private Scr_PlayerShipMovement playerShipMovement;
    private CapsuleCollider2D capsuleCollider;

    private void Start()
    {
        playerShipStats = GetComponentInParent<Scr_PlayerShipStats>();
        playerShipMovement = GetComponentInParent<Scr_PlayerShipMovement>();
    }

    private void Update()
    {
        checkCollisions = !playerShipMovement.landingProperly;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Planet") && checkCollisions)
            playerShipStats.Death();
    }
}