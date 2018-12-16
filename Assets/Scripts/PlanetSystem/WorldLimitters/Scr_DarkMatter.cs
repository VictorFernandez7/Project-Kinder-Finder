using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_DarkMatter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BoxCollider2D warningTrigger;
    [SerializeField] private BoxCollider2D deathTrigger;
    [SerializeField] private GameObject visuals;

    private Scr_PlayerShipStats playerShipStats;

    private void Start()
    {
        playerShipStats = GameObject.Find("PlayerShip").GetComponent<Scr_PlayerShipStats>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerShip"))
        {
            if (!visuals.activeInHierarchy)
            {
                visuals.SetActive(true);
                playerShipStats.inDanger = true;
            }

            else
                playerShipStats.Death();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerShip"))
        {
            visuals.SetActive(false);
            playerShipStats.inDanger = false;
        }
    }
}