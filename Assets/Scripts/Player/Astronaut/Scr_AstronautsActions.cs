using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Scr_AstronautMovement))]
[RequireComponent(typeof(Scr_AstronautStats))]

public class Scr_AstronautsActions : MonoBehaviour
{
    [SerializeField] private GameObject fuelExtractor;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] public Transform pickPoint;

    [HideInInspector] public bool canGrab  = true;

    private GameObject playerShip;
    private Scr_AstronautMovement astronautMovement;

    private void Start()
    {
        playerShip = GameObject.Find("PlayerShip");

        astronautMovement = GetComponent<Scr_AstronautMovement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            //Vector3 pos = Astro.transform.position + (((transform.position - Astro.transform.position).normalized * ((Astro.GetComponent<Renderer>().bounds.size.y/2) + (box.GetComponent<Renderer>().bounds.size.y / 2))));
            Instantiate(fuelExtractor, spawnPoint.transform.position, transform.rotation).transform.SetParent(astronautMovement.currentPlanet.transform);
        }

        if (Input.GetKeyDown(KeyCode.E) && astronautMovement.canEnterShip)
        {
            playerShip.GetComponent<Scr_PlayerShipMovement>().onBoard = true;
            //camara.GetComponent<CamMove>().astronaut = false;
            gameObject.SetActive(false);
        }
    }
}