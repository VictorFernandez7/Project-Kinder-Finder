using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Scr_DragFuel : MonoBehaviour, IPointerDownHandler,IPointerUpHandler
{
    [Header("Display Tooltip?")]
    [SerializeField] private bool displayTooltip;

    [Header("Item Values")]
    [SerializeField] public int itemIndex;

    [Header("References")]
    [SerializeField] private Scr_PlayerShipStats playerShipStats;
    [SerializeField] private Scr_PlayerShipWarehouse playerShipWarehouse;
    [SerializeField] private GameObject fuelSliderGlow;

    private bool dragging;
    private bool onRange;
    private bool overSlot;
    private Scr_DragFuel slot;

    private void Start()
    {
        if(GetComponentInChildren<Scr_Tooltip>())
            GetComponentInChildren<Scr_Tooltip>().tipText = "";
    }

    private void Update()
    {
        if (dragging)
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        if (playerShipStats.resourceWarehouse[itemIndex] != null)
        {
            displayTooltip = true;
            GetComponentInChildren<Scr_Tooltip>().isItem = true;
        }

        else
        {
            displayTooltip = false;
            GetComponentInChildren<Scr_Tooltip>().isItem = false;
        }

        if (displayTooltip)
            GetComponentInChildren<Scr_Tooltip>().tipText = playerShipStats.resourceWarehouse[itemIndex].name;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        dragging = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        dragging = false;

        if (overSlot)
        {
            playerShipStats.resourceWarehouse[slot.itemIndex] = playerShipStats.resourceWarehouse[itemIndex];
            playerShipStats.resourceWarehouse[itemIndex] = null;
            slot.GetComponentInChildren<Scr_Tooltip>().isJustActive = true;
        }

        transform.localPosition = Vector3.zero;

        if (onRange && playerShipStats.resourceWarehouse[itemIndex].name == "Fuel")
            Refuel();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FuelTank"))
        {
            onRange = true;

            if (playerShipStats.resourceWarehouse[itemIndex].name == "Fuel")
                fuelSliderGlow.SetActive(true);
        }

        else if (collision.CompareTag("WarehouseSlot"))
        {
            slot = collision.gameObject.GetComponent<Scr_DragFuel>();
            overSlot = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("FuelTank"))
        {
            onRange = false;
            fuelSliderGlow.SetActive(false);
        }

        else if (collision.CompareTag("WarehouseSlot"))
        {
            slot = null;
            overSlot = false;
        }
    }

    private void Refuel()
    {
        playerShipStats.currentFuel += 50;
        playerShipStats.resourceWarehouse[itemIndex] = null;
    }
}
