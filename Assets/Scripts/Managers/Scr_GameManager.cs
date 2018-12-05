using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_GameManager : MonoBehaviour
{
    [Header("Vortex Spawn")]
    [SerializeField] private float ratio;
    [SerializeField] private float xMax;
    [SerializeField] private float yMax;

    [Header("References")]
    [SerializeField] public GameObject initialPlanet;
    [SerializeField] public GameObject vortex;

    private float initialRatio;
    private Vector3 vortexPosition;
    private GameObject astronaut;
    private GameObject playerShip;

    private void Awake()
    {
        astronaut = GameObject.Find("Astronaut");
        playerShip = GameObject.Find("PlayerShip");

        astronaut.GetComponent<Scr_AstronautMovement>().currentPlanet = initialPlanet;
        astronaut.GetComponent<Scr_AstronautMovement>().planetPosition = initialPlanet.transform.position;
        playerShip.GetComponent<Scr_PlayerShipMovement>().currentPlanet = initialPlanet;

        initialRatio = ratio;
    }

    private void Update()
    {
        VortexSpawn();
    }

    private void VortexSpawn()
    {
        initialRatio -= Time.deltaTime;

        if (initialRatio <= 0)
        {
            vortexPosition = new Vector3(Random.Range(-xMax, xMax), Random.Range(-yMax, yMax), 0);

            Instantiate(vortex, vortexPosition, transform.rotation);

            initialRatio = ratio;
        }
    }
}