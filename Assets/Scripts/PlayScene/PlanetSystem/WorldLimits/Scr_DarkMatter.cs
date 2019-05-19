using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_DarkMatter : MonoBehaviour
{
    [Header("References")]
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
                playerShipStats.inDanger = true;

                Invoke("Temporal", 1f);
            }

            //else
                //playerShipStats.Death();
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

    private void Temporal()
    {
        visuals.SetActive(true);
    }
}