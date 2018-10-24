using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Atmosphere : MonoBehaviour
{
    [Header("Atmosphere properties")]
    [SerializeField] private float atmosphereDrag;

    [Header("References")]
    [SerializeField] private Rigidbody2D playerShip;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerShip")
        {
            playerShip.drag = atmosphereDrag;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerShip")
        {
            playerShip.drag = 0;
        }
    }
}