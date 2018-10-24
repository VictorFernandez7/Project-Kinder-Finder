using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]

public class Scr_FuelBlock : MonoBehaviour
{
    [Header("Object Properties")]
    [SerializeField] private int fuelAmount;

    private bool onRange;
    private GameObject nave;
    private GameObject astronaut;

    private void Start()
    {
        astronaut = transform.parent.gameObject;
    }

    private void Update()
    {
        if (onRange)
        {
            nave.GetComponent<Scr_PlayerShipStats>().ReFuel(fuelAmount);
            astronaut.GetComponent<Scr_AstronautsActions>().canGrab = true;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerShip")
        {
            nave = collision.gameObject;
            onRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerShip")
            onRange = false;
    }
}