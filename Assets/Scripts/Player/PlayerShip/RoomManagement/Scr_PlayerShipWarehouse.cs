﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Scr_PlayerShipWarehouse : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public Dictionary<string, int> Resources = new Dictionary<string, int>();
    [SerializeField] public bool[] astronautSlots;
    [SerializeField] public bool[] warehouseSlots;
    [SerializeField] public bool[] resourcesWarehouseSlots;
    [SerializeField] private Scr_PlayerShipStats playerShipStats;
    [SerializeField] private Scr_PlayerShipActions playerShipActions;
    [SerializeField] private Scr_AstronautStats astronautStats;
    [SerializeField] private TextMeshProUGUI[] textToolSlots;
    [SerializeField] private TextMeshProUGUI[] textToolWarehouse;
    [SerializeField] private TextMeshProUGUI[] textResourcesWarehouse;

    [HideInInspector] public bool slot;
    [HideInInspector] public bool warehouse;
    [HideInInspector] public bool resourceWarehouse;
    [HideInInspector] public int warehouseNumber;
    [HideInInspector] public int resourceWarehouseNumber;
    [HideInInspector] public int slotNumber;

    void Start ()
    {
        boolControl();
        ReadNames();

        Resources.Add("Fuel", 0);
        Resources.Add("Iron", 0);
        Resources.Add("Copper", 0);
    }

	void Update ()
    {
        ReadNames();

        if (Input.GetKeyDown(KeyCode.V)) //Cuando se realiza el calculo
        {
            InventoryInfo();
        }
    }

    public void InventoryInfo()
    {
        List<string> keyr = new List<string>(Resources.Keys);

        foreach (string k in keyr)
            Resources[k] = 0;

        CalculateResources();

        foreach (string k in keyr)
            Debug.Log(k + " " + Resources[k]);
    }

    private void CalculateResources()
    {
        for (int i = 0; i < playerShipStats.resourceWarehouse.Length; i++)
        {
            List<string> keys = new List<string>(Resources.Keys);

            if (!playerShipStats.resourceWarehouse[i])
                break;

            else
            {
                if (Resources.Count != 0)
                {
                    foreach (string key in keys)
                    {
                        if (key == playerShipStats.resourceWarehouse[i].name)
                        {
                            Resources[key] += 1;
                            break;
                        }
                    }
                }
            }
        }
    }

    //TOOL WAREHOUSE

    private void boolControl()
    {
        for (int i = 0; i < astronautSlots.Length; i++)
        {
            if (astronautSlots[i] == true)
            {
                slot = true;
                slotNumber = i;
                break;
            }

            slot = false;
        }

        for (int i = 0; i < warehouseSlots.Length; i++)
        {
            if (warehouseSlots[i] == true)
            {
                warehouse = true;
                warehouseNumber = i;
                break;
            }

            warehouse = false;
        }
    }

    public void Slot(int indice)
    {
        if (!slot && !warehouse)
            astronautSlots[indice] = true;

        else if (astronautSlots[indice])
            astronautSlots[indice] = false;

        else if (slot)
        {
            GameObject temporalObject = astronautStats.toolSlots[indice];
            astronautStats.toolSlots[indice] = astronautStats.toolSlots[slotNumber];
            astronautStats.toolSlots[slotNumber] = temporalObject;
            ReadNames();
            astronautSlots[slotNumber] = false;
        }

        else if (warehouse)
        {
            GameObject temporalObject = astronautStats.toolSlots[indice]; 
            playerShipActions.TakeTool(warehouseNumber, indice);
            playerShipStats.toolWarehouse[warehouseNumber] = temporalObject;
            ReadNames();
            warehouseSlots[warehouseNumber] = false;
        }

        boolControl();
    }

    public void Warehouse(int indice)
    {
        if (!slot && !warehouse)
            warehouseSlots[indice] = true;

        else if (warehouseSlots[indice])
            warehouseSlots[indice] = false;

        else if (warehouse)
        {
            GameObject temporalObject = playerShipStats.toolWarehouse[indice];
            playerShipStats.toolWarehouse[indice] = playerShipStats.toolWarehouse[warehouseNumber];
            playerShipStats.toolWarehouse[warehouseNumber] = temporalObject;
            warehouseSlots[warehouseNumber] = false;
        }

        else if (slot)
        {
            GameObject temporalObject = playerShipStats.toolWarehouse[indice];
            playerShipActions.SaveTool(slotNumber, indice);
            astronautStats.toolSlots[slotNumber] = temporalObject;
            ReadNames();
            astronautSlots[slotNumber] = false;
        }

        boolControl();
    }

    //RESOURCE WAREHOUSE

    public void ControlResources()
    {
        for (int i = 0; i < resourcesWarehouseSlots.Length; i++)
        {
            if (resourcesWarehouseSlots[i] == true)
            {
                resourceWarehouse = true;
                resourceWarehouseNumber = i;
                break;
            }

            resourceWarehouse = false;
        }
    }

    public void ResourceWarehousew(int indice)
    {
        if (!resourceWarehouse)
            resourcesWarehouseSlots[indice] = true;

        else
        {
            GameObject temporalObject = playerShipStats.resourceWarehouse[indice];
            playerShipStats.resourceWarehouse[indice] = playerShipStats.resourceWarehouse[resourceWarehouseNumber];
            playerShipStats.resourceWarehouse[resourceWarehouseNumber] = temporalObject;
            resourcesWarehouseSlots[resourceWarehouseNumber] = false;
        }

        ControlResources();
    }

    //LECTURA DE TEXTOS

    public void ReadNames()
    {
        for (int i = 0; i < astronautStats.toolSlots.Length; i++)
        {
            if (astronautStats.toolSlots[i] == null)
                textToolSlots[i].text = "Empty";

            else
                textToolSlots[i].text = astronautStats.toolSlots[i].GetComponent<Scr_ToolBase>().toolName;
        }

        for (int i = 0; i < playerShipStats.toolWarehouse.Length; i++)
        {
            if (playerShipStats.toolWarehouse[i] == null)
                textToolWarehouse[i].text = "Empty";

            else
                textToolWarehouse[i].text = playerShipStats.toolWarehouse[i].GetComponent<Scr_ToolBase>().toolName;
        }

        for(int i = 0; i < playerShipStats.resourceWarehouse.Length; i++)
        {
            if (playerShipStats.resourceWarehouse[i] == null)
                textResourcesWarehouse[i].text = "Empty";

            else
                textResourcesWarehouse[i].text = playerShipStats.resourceWarehouse[i].name;
        }
    }

    //BUTTON RESET

    public void ButtonReset()
    {
        for (int i = 0; i < astronautSlots.Length; i++)
        {
            astronautSlots[i] = false;
        }

        slot = false;

        for (int i = 0; i < warehouseSlots.Length; i++)
        {
            warehouseSlots[i] = false;
        }

        warehouse = false;

        for (int i = 0; i < resourcesWarehouseSlots.Length; i++)
        {
            resourcesWarehouseSlots[i] = false;
        }

        resourceWarehouse = false;
    }
}