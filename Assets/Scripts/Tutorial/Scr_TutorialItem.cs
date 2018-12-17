using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_TutorialItem : MonoBehaviour
{
    [Header("Set Parent Of")]
    [SerializeField] private Transform targetParent;

    [Header("REferemcves")]
    [SerializeField] private GameObject warehouseText;

    private GameObject playerShip;
    private Scr_PlayerShipMovement playerShipMovement;

    private void Start()
    {
        playerShip = GameObject.Find("PlayerShip");

        playerShipMovement = playerShip.GetComponent<Scr_PlayerShipMovement>();

        if (targetParent != null)
            transform.SetParent(targetParent);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Astronaut"))
        {
            if (Input.GetKeyDown(KeyCode.P) && warehouseText != null && playerShipMovement.astronautOnBoard)
                Destroy(warehouseText);
        }
    }
}