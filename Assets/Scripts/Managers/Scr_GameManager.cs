using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject initialPlanet;

    private GameObject astronaut;

    private void Start()
    {
        astronaut = GameObject.Find("Astronaut");

        astronaut.GetComponent<Scr_AstronautMovement>().currentPlanet = initialPlanet;
    }
}