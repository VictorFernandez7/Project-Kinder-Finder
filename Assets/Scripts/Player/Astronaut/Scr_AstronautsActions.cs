﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Scr_AstronautMovement))]
[RequireComponent(typeof(Scr_AstronautStats))]

public class Scr_AstronautsActions : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject fuelCollector;
    [SerializeField] private GameObject fuelBlock;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] public Transform pickPoint;

    [HideInInspector] public bool emptyHands = true;
    [HideInInspector] private GameObject mainCamera;
    
    private float fuelAmount;
    private GameObject playerShip;
    private Scr_AstronautMovement astronautMovement;
    private GameObject currentFuelBLock;
    private Animator mainCanvasAnim;

    private void Start()
    {
        playerShip = GameObject.Find("PlayerShip");
        mainCamera = GameObject.Find("MainCamera");
        mainCanvasAnim = GameObject.Find("MainCanvas").GetComponent<Animator>();

        astronautMovement = GetComponent<Scr_AstronautMovement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            //Vector3 pos = Astro.transform.position + (((transform.position - Astro.transform.position).normalized * ((Astro.GetComponent<Renderer>().bounds.size.y/2) + (box.GetComponent<Renderer>().bounds.size.y / 2))));
            Instantiate(fuelCollector, spawnPoint.transform.position, transform.rotation);
        }

        if (Input.GetKeyDown(KeyCode.E) && astronautMovement.canEnterShip && emptyHands)
        {
            playerShip.GetComponent<Scr_PlayerShipMovement>().astronautOnBoard = true;
            playerShip.GetComponent<Scr_PlayerShipActions>().startExitDelay = true;
            mainCamera.GetComponent<Scr_MainCamera>().followAstronaut = false;
            gameObject.SetActive(false);

            mainCanvasAnim.SetBool("OnBoard", true);
        }

        if (Input.GetKeyDown(KeyCode.E) && astronautMovement.closeToCollector && astronautMovement.currentFuelCollector != null)
        {
            if (astronautMovement.currentFuelCollector.GetComponent<Scr_FuelCollector>().canCollect)
            {
                astronautMovement.currentFuelCollector.GetComponent<Scr_FuelCollector>().CollectFuel();
                CollectFuel();
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && astronautMovement.canEnterShip && !emptyHands)
        {
            playerShip.GetComponent<Scr_PlayerShipStats>().ReFuel(currentFuelBLock.GetComponent<Scr_FuelBlock>().fuelAmount);
            Destroy(currentFuelBLock);
            emptyHands = true;
        }
    }

    void CollectFuel()
    {
        if (GameObject.Find("FuelBlock(Clone)") == null)
            currentFuelBLock = Instantiate(fuelBlock, pickPoint.transform.position, pickPoint.transform.rotation);

        emptyHands = false;
    }
}