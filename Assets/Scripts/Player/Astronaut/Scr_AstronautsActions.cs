﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_AstronautsActions : MonoBehaviour
{
    [Header("Resources")]
    [SerializeField] private int maxResourcesCapacity;

    [Header("References")]
    [SerializeField] private Animator interactionIndicatorAnim;
    [SerializeField] public Transform pickPoint;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform iaResourcePoint;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject playerShip;
    [SerializeField] private GameObject lantern;
    [SerializeField] private Scr_SunLight sunLight;
    [SerializeField] private Scr_GameManager gameManager;
    [SerializeField] private Scr_AstronautResourcesCheck astronautResourcesCheck;
    [SerializeField] private Scr_PlayerShipProxCheck playerShipProxCheck;
    [SerializeField] private Scr_NarrativeManager narrativeManager;
    [SerializeField] public Scr_PlayerShipWarehouse playerShipWarehouse;

    [HideInInspector] public int numberToolActive;
    [HideInInspector] public bool emptyHands;
    [HideInInspector] public bool toolOnHands;
    [HideInInspector] public GameObject toolOnFloor;
    [HideInInspector] public bool[] unlockedTools = new bool[5];

    private float fuelAmount;
    private float holdInputTime = 0.9f;
    private bool canInputAgain = true;
    private bool pickFirst;
    private bool introduceFirst;

    private GameObject currentResource;
    private Scr_CableVisuals cableVisuals;
    private Scr_AstronautMovement astronautMovement;
    private Scr_AstronautStats astronautStats;
    private Scr_PlayerShipActions playerShipActions;

    private void Start()
    {
        astronautMovement = GetComponent<Scr_AstronautMovement>();
        astronautStats = GetComponent<Scr_AstronautStats>();
        cableVisuals = playerShip.GetComponentInChildren<Scr_CableVisuals>();
        playerShipActions = playerShip.GetComponent<Scr_PlayerShipActions>();

        toolOnFloor = null;
        emptyHands = true;
    }

    private void Update()
    {
        if (Input.GetButton("Interact"))
        {
            if (astronautMovement.canEnterShip && emptyHands)
            {
                holdInputTime -= Time.deltaTime;
                interactionIndicatorAnim.gameObject.SetActive(true);

                if (holdInputTime <= 0 && canInputAgain)
                {
                    canInputAgain = false;
                    interactionIndicatorAnim.gameObject.SetActive(false);

                    if (playerShip.GetComponent<Scr_PlayerShipMovement>().playerShipState == Scr_PlayerShipMovement.PlayerShipState.landed)
                        EnterShipFromPlanet();

                    if (playerShip.GetComponent<Scr_PlayerShipMovement>().playerShipState == Scr_PlayerShipMovement.PlayerShipState.inSpace)
                        EnterShipFromSpace();
                }
            }

            else if (astronautMovement.canEnterShip)
                IntroduceResource();
        }

        else
        {
            canInputAgain = true;
            holdInputTime = 0.9f;
            interactionIndicatorAnim.gameObject.SetActive(false);
        }

        if (Input.GetButtonDown("Interact"))
        {
            if (toolOnHands)
                astronautStats.toolSlots[numberToolActive].GetComponent<Scr_ToolBase>().UseTool();

            else if (emptyHands && !toolOnHands && astronautResourcesCheck.resourceList.Count > 0)
            {

                currentResource = astronautResourcesCheck.resourceList[0];
                astronautResourcesCheck.resourceList.RemoveAt(0);

                currentResource.transform.position = pickPoint.position;
                currentResource.GetComponent<Scr_Resource>().onHands = true;
                currentResource.GetComponent<BoxCollider2D>().enabled = false;
                currentResource.transform.SetParent(pickPoint);
                emptyHands = false;

                if (!pickFirst)
                {
                    narrativeManager.StartDialogue(4);
                    pickFirst = true;
                }
            }
        }

        TurnOnLantern();
    }
    
    private void EnterShipFromPlanet()
    {
            playerShip.GetComponent<Scr_PlayerShipMovement>().astronautOnBoard = true;
            playerShip.GetComponent<Scr_PlayerShipActions>().startExitDelay = true;
            playerShip.GetComponent<Scr_PlayerShipMovement>().canControlShip = true;
            mainCamera.GetComponent<Scr_MainCamera>().followAstronaut = false;
            astronautMovement.keep = true;
            playerShipWarehouse.ReadNames();
            gameObject.SetActive(false);
    }

    private void EnterShipFromSpace()
    {
        playerShipProxCheck.ClearInterface(true);
        GetComponent<Scr_AstronautEffects>().breathingBool = false;
        playerShip.GetComponent<Scr_PlayerShipMovement>().astronautOnBoard = true;
        playerShip.GetComponent<Scr_PlayerShipMovement>().canControlShip = true;
        playerShip.GetComponent<Scr_PlayerShipMovement>().canRotateShip = true;
        playerShip.GetComponent<Scr_PlayerShipActions>().doingSpaceWalk = false;
        cableVisuals.printCable = false;
        cableVisuals.ResetCable();
        gameManager.Gravity(true);
        gameObject.SetActive(false);
    }

    private void IntroduceResource()
    {
        if (currentResource.CompareTag("Resources"))
        {
            for (int j = 0; j < playerShip.GetComponent<Scr_PlayerShipStats>().resourceWarehouse.Length; j++)
            {
                if (playerShip.GetComponent<Scr_PlayerShipStats>().resourceWarehouse[j] == null)
                {
                    playerShip.GetComponent<Scr_PlayerShipStats>().resourceWarehouse[j] = currentResource.GetComponent<Scr_Resource>().resourceReference;
                    break;
                }
            }
        }

        if (!introduceFirst)
        {
            narrativeManager.StartDialogue(5);
            introduceFirst = true;
        }

        Destroy(currentResource);
        currentResource = null;
        emptyHands = true;
    }

   //HACER SELECCION DE TOOL
   public void TakeTool (int index)
    {
        if (unlockedTools[index] && emptyHands)
        {
            if (toolOnHands)
            {
                astronautStats.toolSlots[numberToolActive].SetActive(false);
            }

            else
            {
                toolOnHands = true;
            }

            astronautStats.toolSlots[index].SetActive(true);
            numberToolActive = index;
        }
    }

    public void NoToolsOnHands()
    {
        if (emptyHands)
        {
            astronautStats.toolSlots[numberToolActive].SetActive(false);
            toolOnHands = false;
            emptyHands = true;
        }
    }

    private void TurnOnLantern()
    {
        lantern.SetActive(!sunLight.hitByLight);
    }
}