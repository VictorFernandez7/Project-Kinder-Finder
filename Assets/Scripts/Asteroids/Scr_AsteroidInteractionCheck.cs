using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_AsteroidInteractionCheck : MonoBehaviour
{
    private Scr_AsteroidBehaviour asteroidBehaviour;

    private void Start()
    {
        asteroidBehaviour = GetComponentInParent<Scr_AsteroidBehaviour>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerShip"))
            asteroidBehaviour.CloseToShip();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerShip"))
            asteroidBehaviour.NotColoseToShip();
    }
}