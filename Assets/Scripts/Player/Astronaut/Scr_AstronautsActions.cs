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

    [Header("Audio")]
    [SerializeField] private AudioSource getIntoTheShipSound;

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
        if (Input.GetKeyDown(KeyCode.E) && astronautMovement.canEnterShip)
        {
            if (emptyHands)
            {
                //tocada audio
                getIntoTheShipSound.Play();

                playerShip.GetComponent<Scr_PlayerShipMovement>().astronautOnBoard = true;
                playerShip.GetComponent<Scr_PlayerShipActions>().startExitDelay = true;
                playerShip.GetComponent<Scr_PlayerShipMovement>().canControlShip = true;
                mainCamera.GetComponent<Scr_MainCamera>().followAstronaut = false;
                mainCanvasAnim.SetBool("OnBoard", true);
                astronautMovement.keep = true;

                gameObject.SetActive(false);
            }

            else
            {
                playerShip.GetComponent<Scr_PlayerShipStats>().ReFuel(currentFuelBLock.GetComponent<Scr_FuelBlock>().fuelAmount);
                Destroy(currentFuelBLock);
                emptyHands = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.T))
            Instantiate(fuelCollector, spawnPoint.transform.position, transform.rotation);

        if (Input.GetKeyDown(KeyCode.E) && astronautMovement.closeToCollector && astronautMovement.currentFuelCollector != null)
        {
            if (astronautMovement.currentFuelCollector.GetComponent<Scr_FuelCollector>().canCollect)
            {
                astronautMovement.currentFuelCollector.GetComponent<Scr_FuelCollector>().CollectFuel();
                CollectFuel();
            }
        }
    }

    void CollectFuel()
    {
        if (GameObject.Find("FuelBlock(Clone)") == null)
            currentFuelBLock = Instantiate(fuelBlock, pickPoint.transform.position, pickPoint.transform.rotation);

        emptyHands = false;
    }
}