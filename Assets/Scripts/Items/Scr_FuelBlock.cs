using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_FuelBlock : MonoBehaviour
{
    [SerializeField] private readonly int fuelAmount;

    private bool onRange;
    private GameObject nave;
    private GameObject astronaut;

    void Start()
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