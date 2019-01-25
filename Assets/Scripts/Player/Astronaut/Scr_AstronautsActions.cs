using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_AstronautsActions : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] public Transform pickPoint;
    [SerializeField] public Scr_PlayerShipWarehouse playerShipWarehouse;
    [SerializeField] private Animator interactionIndicatorAnim;
    [SerializeField] private Scr_PlanetManager planetManager;
    [SerializeField] private Scr_AstronautResourcesCheck astronautResourcesCheck;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject playerShip;

    [HideInInspector] public bool emptyHands;
    [HideInInspector] public bool toolOnHands;
    [HideInInspector] public int numberToolActive;
    [HideInInspector] public GameObject toolOnFloor;

    private float fuelAmount;
    private float holdInputTime = 0.9f;
    private bool canInputAgain = true;
    
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

            else if(astronautMovement.canEnterShip)
                IntroduceResource();
        }

        else
        {
            canInputAgain = true;
            holdInputTime = 0.9f;
            interactionIndicatorAnim.gameObject.SetActive(false);
        }

        if (Input.GetButtonDown("Tool1"))
            HandTool(0);

        if (Input.GetButtonDown("Tool2"))
            HandTool(1);

        if (Input.GetButtonDown("Tool3"))
            HandTool(2);

        if (Input.GetButtonDown("Interact"))
        {
            if (toolOnHands)
                astronautStats.physicToolSlots[numberToolActive].GetComponent<Scr_ToolBase>().UseTool();

            else if(emptyHands && !toolOnHands)
            {
                for(int i = 0; i < astronautResourcesCheck.resourceList.Count; i++)
                {
                    if (i != 0)
                    {
                        if (Vector3.Project(astronautResourcesCheck.resourceList[i].transform.position, transform.up).magnitude > Vector3.Project(astronautResourcesCheck.resourceList[i - 1].transform.position, transform.up).magnitude)
                            currentResource = astronautResourcesCheck.resourceList[i];
                    }

                    else
                        currentResource = astronautResourcesCheck.resourceList[i];
                }

                currentResource.transform.position = pickPoint.position;
                currentResource.transform.SetParent(pickPoint);
                emptyHands = false;
            }

            else if (toolOnFloor != null && toolOnFloor.GetComponent<Scr_ToolBase>().resourceAmount <= 0)
                toolOnFloor.GetComponent<Scr_ToolBase>().RecoverTool();
        }
    }
    
    private void EnterShipFromPlanet()
    {
            playerShip.GetComponent<Scr_PlayerShipMovement>().astronautOnBoard = true;
            playerShip.GetComponent<Scr_PlayerShipActions>().startExitDelay = true;
            playerShip.GetComponent<Scr_PlayerShipMovement>().canControlShip = true;
            mainCamera.GetComponent<Scr_MainCamera>().followAstronaut = false;
            astronautMovement.keep = true;
            DestroyAllTools();
            playerShipWarehouse.ReadNames();
            gameObject.SetActive(false);
    }

    private void EnterShipFromSpace()
    {
        GetComponent<Scr_AstronautEffects>().breathingBool = false;
        playerShip.GetComponent<Scr_PlayerShipMovement>().astronautOnBoard = true;
        playerShip.GetComponent<Scr_PlayerShipMovement>().canControlShip = true;
        playerShip.GetComponent<Scr_PlayerShipMovement>().canRotateShip = true;
        playerShip.GetComponent<Scr_PlayerShipActions>().doingSpaceWalk = false;
        cableVisuals.printCable = false;
        cableVisuals.ResetCable();
        planetManager.Gravity(true);
        gameObject.SetActive(false);
    }

    private void IntroduceResource()
    {
        if (currentResource.CompareTag("Resources"))
        {
            for (int i = 0; i < playerShip.GetComponent<Scr_PlayerShipStats>().resourceWarehouse.Length; i++)
            {
                if (playerShip.GetComponent<Scr_PlayerShipStats>().resourceWarehouse[i] == null)
                {
                    playerShip.GetComponent<Scr_PlayerShipStats>().resourceWarehouse[i] = currentResource.GetComponent<Scr_Resource>().resourceReference;
                    break;
                } 
            }
        }

        Destroy(currentResource);
        emptyHands = true;
    }

    public void BoolControl()
    {
        for (int i = 0; i < astronautStats.physicToolSlots.Length; i++)
        {
            if (astronautStats.physicToolSlots[i] != null)
            {
                if (astronautStats.physicToolSlots[i].activeInHierarchy)
                {
                    toolOnHands = true;
                    numberToolActive = i;
                    break;
                }
            }

            toolOnHands = false;
        }
    }

    private void HandTool(int indice)
    {
        if (astronautStats.physicToolSlots[indice] != null)
        {
            if (!toolOnHands && emptyHands)
                astronautStats.physicToolSlots[indice].SetActive(true);

            else if (astronautStats.physicToolSlots[indice].activeInHierarchy && emptyHands)
                astronautStats.physicToolSlots[indice].SetActive(false);

            else if (toolOnHands && emptyHands)
            {
                astronautStats.physicToolSlots[numberToolActive].SetActive(false);
                astronautStats.physicToolSlots[indice].SetActive(true);
            }
        }

        BoolControl();
    }

    private void DestroyAllTools()
    {
        for (int i = 0; i < astronautStats.physicToolSlots.Length; i++)
        {
            if (astronautStats.physicToolSlots[i] != null)
                Destroy(astronautStats.physicToolSlots[i]);
        }
    }
}