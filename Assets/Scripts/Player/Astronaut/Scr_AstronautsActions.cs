using System.Collections;
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
    [SerializeField] public Scr_ToolPanel toolPanel;

    [Header("Audio")]
    [SerializeField] private AudioSource getIntoTheShipSound;

    [HideInInspector] public bool emptyHands;
    [HideInInspector] public bool toolOnHands;
    [HideInInspector] public int numberToolActive;

    public GameObject toolOnFloor;
    private GameObject mainCamera;
    private float fuelAmount;
    private GameObject playerShip;
    private Scr_AstronautMovement astronautMovement;
    private Scr_AstronautStats astronautStats;
    private GameObject currentFuelBLock;

    private void Start()
    {
        playerShip = GameObject.Find("PlayerShip");
        mainCamera = GameObject.Find("MainCamera");

        astronautMovement = GetComponent<Scr_AstronautMovement>();
        astronautStats = GetComponent<Scr_AstronautStats>();
        toolOnFloor = null;

        emptyHands = true;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact") && astronautMovement.canEnterShip)
        {
            if (playerShip.GetComponent<Scr_PlayerShipMovement>().playerShipState == Scr_PlayerShipMovement.PlayerShipState.landed)
            {
                if (emptyHands)
                {
                    //tocada audio
                    getIntoTheShipSound.Play();

                    playerShip.GetComponent<Scr_PlayerShipMovement>().astronautOnBoard = true;
                    playerShip.GetComponent<Scr_PlayerShipActions>().startExitDelay = true;
                    playerShip.GetComponent<Scr_PlayerShipMovement>().canControlShip = true;
                    mainCamera.GetComponent<Scr_MainCamera>().followAstronaut = false;
                    astronautMovement.keep = true;
                    DestroyAllTools();
                    toolPanel.ReadNames();
                    gameObject.SetActive(false);
                }

                else
                {
                    playerShip.GetComponent<Scr_PlayerShipStats>().ReFuel(currentFuelBLock.GetComponent<Scr_FuelBlock>().fuelAmount);
                    Destroy(currentFuelBLock);
                    emptyHands = true;
                }
            }

            if (playerShip.GetComponent<Scr_PlayerShipMovement>().playerShipState == Scr_PlayerShipMovement.PlayerShipState.inSpace)
            {
                playerShip.GetComponent<Scr_PlayerShipMovement>().astronautOnBoard = true;
                playerShip.GetComponent<Scr_PlayerShipMovement>().canControlShip = true;
                playerShip.GetComponent<Scr_PlayerShipMovement>().canRotateShip = true;
                gameObject.SetActive(false);
            }
        }

        if (Input.GetButtonDown("Interact") && astronautMovement.closeToCollector && astronautMovement.currentFuelCollector != null)
        {
            if (emptyHands && !toolOnHands && astronautMovement.currentFuelCollector.GetComponent<Scr_ToolBase>().resourceAmount > 0 && !astronautMovement.currentFuelCollector.GetComponent<Scr_ToolBase>().onHands)
            {
                astronautMovement.currentFuelCollector.GetComponent<Scr_ToolBase>().resourceAmount -= 1;
                currentFuelBLock = Instantiate(astronautMovement.currentFuelCollector.GetComponent<Scr_ToolBase>().resource, pickPoint);
                emptyHands = false;
            }
        }

        if (Input.GetButtonDown("Tool1"))
        {
            HandTool(0);
        }

        if (Input.GetButtonDown("Tool2"))
        {
            HandTool(1);
        }

        if (Input.GetButtonDown("Tool3"))
        {
            HandTool(2);
        }

        if(Input.GetButtonDown("Interact"))
        {
            if (toolOnHands)
            {
                astronautStats.physicToolSlots[numberToolActive].GetComponent<Scr_ToolBase>().UseTool();
            }

            else if (toolOnFloor != null && toolOnFloor.GetComponent<Scr_ToolBase>().resourceAmount <= 0)
            {
                toolOnFloor.GetComponent<Scr_ToolBase>().RecoverTool();
            }
        }
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
            {
                astronautStats.physicToolSlots[indice].SetActive(false);
            }

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
            {
                Destroy(astronautStats.physicToolSlots[i]);
            }
        }
    }

    void CollectFuel()
    {
        if (GameObject.Find("FuelBlock(Clone)") == null)
            currentFuelBLock = Instantiate(fuelBlock, pickPoint.transform.position, pickPoint.transform.rotation);

        emptyHands = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Tool")
        {
            toolOnFloor = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Tool")
        {
            toolOnFloor = null;
        }
    }
}