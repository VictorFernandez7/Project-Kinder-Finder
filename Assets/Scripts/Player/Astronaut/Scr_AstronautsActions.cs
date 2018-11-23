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

    [Header("Audio")]
    [SerializeField] private AudioSource getIntoTheShipSound;

    [HideInInspector] public bool emptyHands = true;
    [HideInInspector] private GameObject mainCamera;

    private bool toolOnHands;
    private float fuelAmount;
    private int numberToolActive;
    private GameObject playerShip;
    private Scr_AstronautMovement astronautMovement;
    private Scr_AstronautStats astronautStats;
    private GameObject currentFuelBLock;
    private Animator mainCanvasAnim;
    private Animator speedAnim;

    private void Start()
    {
        playerShip = GameObject.Find("PlayerShip");
        mainCamera = GameObject.Find("MainCamera");
        mainCanvasAnim = GameObject.Find("MainCanvas").GetComponent<Animator>();
        speedAnim = GameObject.Find("SpeedPanel").GetComponent<Animator>();

        astronautMovement = GetComponent<Scr_AstronautMovement>();
        astronautStats = GetComponent<Scr_AstronautStats>();
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
                speedAnim.SetTrigger("Activate");
                speedAnim.SetBool("Active", true);
                astronautMovement.keep = true;
                DestroyAllTools();
                gameObject.SetActive(false);
            }

            else
            {
                playerShip.GetComponent<Scr_PlayerShipStats>().ReFuel(currentFuelBLock.GetComponent<Scr_FuelBlock>().fuelAmount);
                Destroy(currentFuelBLock);
                emptyHands = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && astronautMovement.closeToCollector && astronautMovement.currentFuelCollector != null)
        {
            if (astronautMovement.currentFuelCollector.GetComponent<Scr_FuelCollector>().canCollect)
            {
                astronautMovement.currentFuelCollector.GetComponent<Scr_FuelCollector>().CollectFuel();
                CollectFuel();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            HandTool(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            HandTool(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            HandTool(2);
        }

        if(Input.GetKeyDown(KeyCode.R) && toolOnHands)
        {
            astronautStats.physicToolSlots[numberToolActive].GetComponent<Scr_Tool>().UseTool();
        }

    }

    private void BoolControl()
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
}