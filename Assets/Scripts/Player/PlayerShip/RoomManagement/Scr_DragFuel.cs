﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Scr_DragFuel : MonoBehaviour, IPointerDownHandler,IPointerUpHandler, IPointerEnterHandler
{
    [Header("Display Tooltip?")]
    [SerializeField] private bool displayTooltip;

    [Header("Item Values")]
    [SerializeField] private int itemIndex;

    [Header("References")]
    [SerializeField] private Scr_PlayerShipStats playerShipStats;
    [SerializeField] private Scr_PlayerShipWarehouse playerShipWarehouse;
    [SerializeField] private GameObject fuelSliderGlow;

    private bool dragging;
    private bool onRange;

    private void Update()
    {
        if (dragging)
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        dragging = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        dragging = false;
        transform.localPosition = Vector3.zero;

        if (onRange && playerShipStats.resourceWarehouse[itemIndex].name == "Fuel")
            Refuel();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (displayTooltip)
            GetComponentInChildren<Scr_Tooltip>().tipText = playerShipStats.resourceWarehouse[itemIndex].name;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FuelTank"))
        {
            onRange = true;

            if (playerShipStats.resourceWarehouse[itemIndex].name == "Fuel")
                fuelSliderGlow.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("FuelTank"))
        {
            onRange = false;
            fuelSliderGlow.SetActive(false);
        }
    }

    private void Refuel()
    {
        playerShipStats.currentFuel += 50;
        playerShipStats.resourceWarehouse[itemIndex] = null;
    }
}
