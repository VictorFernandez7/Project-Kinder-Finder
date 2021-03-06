﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_AstronautsActions : MonoBehaviour
{
    [Header("Resources")]
    [SerializeField] private int maxResourcesCapacity;
    [SerializeField] private float timeToMine;

    [Header("References")]
    [SerializeField] private Animator interactionIndicatorAnim;
    [SerializeField] public Transform pickPoint;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] public Transform iaSpotTransformUp;
    [SerializeField] public Transform iaSpotTransformDown;
    [SerializeField] public Transform iaSpotTransformRight;
    [SerializeField] public Transform iaSpotTransformLeft;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject playerShip;
    [SerializeField] public GameObject solidTool;
    [SerializeField] public GameObject liquidTool;
    [SerializeField] public GameObject gasTool;
    [SerializeField] public GameObject repairingTool;
    [SerializeField] private Scr_GameManager gameManager;
    [SerializeField] private Scr_AstronautResourcesCheck astronautResourcesCheck;
    [SerializeField] private Scr_PlayerShipProxCheck playerShipProxCheck;
    [SerializeField] private Scr_NarrativeManager narrativeManager;
    [SerializeField] public Scr_PlayerShipWarehouse playerShipWarehouse;
    [SerializeField] public Scr_IAMovement iAMovement;

    [HideInInspector] public int numberToolActive;
    [HideInInspector] public bool emptyHands;
    [HideInInspector] public bool toolOnHands;
    [HideInInspector] public bool[] unlockedTools = new bool[5];
    [HideInInspector] public SpotType spotType;
    [HideInInspector] public GameObject miningSpot;
    [HideInInspector] public GameObject toolOnFloor;

    public int resourceIndex = 0;
    private bool canInputAgain = true;
    private bool pickFirst;
    private bool introduceFirst;
    private bool unlockInteract = true;
    private float fuelAmount;
    private float holdInputTime = 0.9f;
    private float savedTimeToMine;

    public GameObject[] currentResource = new GameObject[5];
    private Scr_CableVisuals cableVisuals;
    private Scr_AstronautStats astronautStats;
    private Scr_AstronautMovement astronautMovement;
    private Scr_PlayerShipActions playerShipActions;

    public enum SpotType
    {
        solidSpot,
        liquidSpot,
        gasSpot,
        breakeable
    }

    private void Start()
    {
        astronautMovement = GetComponent<Scr_AstronautMovement>();
        astronautStats = GetComponent<Scr_AstronautStats>();
        cableVisuals = playerShip.GetComponentInChildren<Scr_CableVisuals>();
        playerShipActions = playerShip.GetComponent<Scr_PlayerShipActions>();

        toolOnFloor = null;
        emptyHands = true;
        savedTimeToMine = timeToMine;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))
            unlockInteract = true;

        if (Input.GetButton("Interact") && !narrativeManager.onDialogue)
        {
            if (miningSpot != null && ((spotType == SpotType.solidSpot && solidTool.activeInHierarchy) || (spotType == SpotType.liquidSpot && liquidTool.activeInHierarchy) || (spotType == SpotType.gasSpot && gasTool.activeInHierarchy) || (spotType == SpotType.breakeable && solidTool.activeInHierarchy) || (repairingTool.activeInHierarchy && playerShip.GetComponent<Scr_PlayerShipStats>().currentShield != playerShip.GetComponent<Scr_PlayerShipStats>().maxShield)))
            {
                if (savedTimeToMine > 0)
                    savedTimeToMine -= Time.deltaTime;

                else
                {
                    iAMovement.isMining = true;
                    iAMovement.target = miningSpot.transform;

                    if (spotType == SpotType.liquidSpot)
                        liquidTool.GetComponent<Scr_LiquidTool>().zone = miningSpot.transform.parent.gameObject;

                    if (spotType == SpotType.gasSpot)
                        gasTool.GetComponent<Scr_GasTool>().zone = miningSpot.transform.parent.gameObject;
                }
            }

            else
            {
                iAMovement.isMining = false;
                liquidTool.GetComponent<Scr_LiquidTool>().zone = null;
                gasTool.GetComponent<Scr_GasTool>().zone = null;
            }

            if (astronautMovement.canEnterShip && emptyHands && resourceIndex == 0 && unlockInteract)
            {
                holdInputTime -= Time.deltaTime;
                interactionIndicatorAnim.gameObject.SetActive(true);

                if (holdInputTime <= 0 && canInputAgain)
                {
                    canInputAgain = false;
                    interactionIndicatorAnim.gameObject.SetActive(false);
                    unlockInteract = false;

                    if (playerShip.GetComponent<Scr_PlayerShipMovement>().playerShipState == Scr_PlayerShipMovement.PlayerShipState.landed)
                        EnterShipFromPlanet();

                    if (playerShip.GetComponent<Scr_PlayerShipMovement>().playerShipState == Scr_PlayerShipMovement.PlayerShipState.inSpace)
                        EnterShipFromSpace();
                }
            }

            else if (astronautMovement.canEnterShip)
            {
                IntroduceResource();
                unlockInteract = false;
            }
        }

        else
        {
            canInputAgain = true;
            holdInputTime = 0.9f;
            interactionIndicatorAnim.gameObject.SetActive(false);
            iAMovement.isMining = false;
            savedTimeToMine = timeToMine;
        }

        if (Input.GetButtonDown("Interact") && !narrativeManager.onDialogue)
        {
            emptyHands = false;

            for (int i = 0; i < 5; i++)
            {
                if(currentResource[i] == null)
                {
                    emptyHands = true;
                    break;
                }
            }


            if (emptyHands && astronautResourcesCheck.resourceList.Count > 0)
            {
                int index = 0;

                for(int i = 0; i < 5; i++)
                {
                    if (currentResource[i] == null)
                    {
                        index = i;
                        break;
                    }
                }

                switch (index)
                {
                    case 0:
                        currentResource[0] = astronautResourcesCheck.resourceList[0];

                        //currentResource[0].transform.position = iaSpotTransformLeft.position;
                        currentResource[0].GetComponent<Scr_Resource>().targetPosition = iaSpotTransformLeft.gameObject;
                        currentResource[0].GetComponent<Scr_Resource>().lerping = true;

                        currentResource[0].GetComponent<Scr_Resource>().onHands = true;
                        currentResource[0].GetComponent<Scr_Resource>().lootParticles.Stop();
                        currentResource[0].GetComponent<BoxCollider2D>().enabled = false;
                        currentResource[0].transform.SetParent(iaSpotTransformLeft);
                        break;

                    case 1:
                        currentResource[1] = astronautResourcesCheck.resourceList[0];

                        //currentResource[1].transform.position = iaSpotTransformUp.position;
                        currentResource[1].GetComponent<Scr_Resource>().targetPosition = iaSpotTransformUp.gameObject;
                        currentResource[1].GetComponent<Scr_Resource>().lerping = true;

                        currentResource[1].GetComponent<Scr_Resource>().onHands = true;
                        currentResource[1].GetComponent<Scr_Resource>().lootParticles.Stop();
                        currentResource[1].GetComponent<BoxCollider2D>().enabled = false;
                        currentResource[1].transform.SetParent(iaSpotTransformUp);
                        break;

                    case 2:
                        currentResource[2] = astronautResourcesCheck.resourceList[0];

                        //currentResource[2].transform.position = iaSpotTransformRight.position;
                        currentResource[2].GetComponent<Scr_Resource>().targetPosition = iaSpotTransformRight.gameObject;
                        currentResource[2].GetComponent<Scr_Resource>().lerping = true;

                        currentResource[2].GetComponent<Scr_Resource>().onHands = true;
                        currentResource[2].GetComponent<Scr_Resource>().lootParticles.Stop();
                        currentResource[2].GetComponent<BoxCollider2D>().enabled = false;
                        currentResource[2].transform.SetParent(iaSpotTransformRight);
                        break;

                    case 3:
                        currentResource[3] = astronautResourcesCheck.resourceList[0];

                        //currentResource[3].transform.position = iaSpotTransformDown.position;
                        currentResource[3].GetComponent<Scr_Resource>().targetPosition = iaSpotTransformDown.gameObject;
                        currentResource[3].GetComponent<Scr_Resource>().lerping = true;

                        currentResource[3].GetComponent<Scr_Resource>().onHands = true;
                        currentResource[3].GetComponent<Scr_Resource>().lootParticles.Stop();
                        currentResource[3].GetComponent<BoxCollider2D>().enabled = false;
                        currentResource[3].transform.SetParent(iaSpotTransformDown);
                        break;

                    case 4:
                        currentResource[4] = astronautResourcesCheck.resourceList[0];
                        //currentResource[4].transform.position = pickPoint.position;
                        currentResource[4].GetComponent<Scr_Resource>().onHands = true;
                        currentResource[4].GetComponent<Scr_Resource>().lootParticles.Stop();
                        currentResource[4].GetComponent<BoxCollider2D>().enabled = false;
                        currentResource[4].transform.SetParent(pickPoint);
                        currentResource[4].transform.localPosition = Vector3.zero;
                        currentResource[4].transform.localRotation = Quaternion.Euler(Vector3.zero);
                        break;
                }

                resourceIndex += 1;
                
                if (!pickFirst)
                {
                    narrativeManager.StartDialogue(4);
                    pickFirst = true;
                }
            }

            else if(astronautResourcesCheck.resourceList.Count > 0)
                narrativeManager.StartDialogue(10);
        }
    }
    
    public void EnterShipFromPlanet()
    {
        playerShip.GetComponent<Scr_PlayerShipMovement>().astronautOnBoard = true;
        playerShip.GetComponent<Scr_PlayerShipActions>().startExitDelay = true;
        playerShip.GetComponent<Scr_PlayerShipMovement>().canControlShip = true;
        mainCamera.GetComponent<Scr_MainCamera>().followAstronaut = false;
        astronautStats.currentOxygen = astronautStats.maxOxygen;
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
        for(int i = 0; i < currentResource.Length; i++)
        {
            if (currentResource[i] != null)
            {
                if (currentResource[i].CompareTag("Resources"))
                {
                    for (int j = 0; j < playerShip.GetComponent<Scr_PlayerShipStats>().resourceWarehouse.Length; j++)
                    {
                        if (playerShip.GetComponent<Scr_PlayerShipStats>().resourceWarehouse[j] == null)
                        {
                            playerShip.GetComponent<Scr_PlayerShipStats>().resourceWarehouse[j] = currentResource[i].GetComponent<Scr_Resource>().resourceReference;
                            break;
                        }
                    }

                    currentResource[i].GetComponent<Scr_Resource>().entering = true;
                    //Destroy(currentResource[i].gameObject);
                    currentResource[i] = null;
                }
            }
        }

        if (!introduceFirst)
        {
            narrativeManager.StartDialogue(5);
            introduceFirst = true;
        }

        resourceIndex = 0;
        emptyHands = true;
    }

   //HACER SELECCION DE TOOL
   public void TakeTool (int index)
    {
        if (unlockedTools[index])
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
}