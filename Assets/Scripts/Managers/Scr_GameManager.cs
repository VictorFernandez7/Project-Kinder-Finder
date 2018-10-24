using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject initialPlanet;

    private GameObject astronaut;
    private GameObject playerShip;

    private void Start()
    {
        astronaut = GameObject.Find("Astronaut");
        playerShip = GameObject.Find("PlayerShip");

        astronaut.GetComponent<Scr_AstronautMovement>().currentPlanet = initialPlanet;
        astronaut.GetComponent<Scr_AstronautMovement>().planetPosition = initialPlanet.transform.position;
        playerShip.GetComponent<Scr_PlayerShipMovement>().currentPlanet = initialPlanet;
    }
}