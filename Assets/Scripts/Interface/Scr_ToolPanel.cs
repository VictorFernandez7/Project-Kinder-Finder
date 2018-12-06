using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Scr_ToolPanel : MonoBehaviour {

    public bool[] slots;
    public bool[] warehouses;

    [Header("Script References")]
    [SerializeField] private Scr_PlayerShipStats playerShipStats;
    [SerializeField] private Scr_PlayerShipActions playerShipActions;
    [SerializeField] private Scr_AstronautStats astronautStats;

    [Header("Text References")]
    [SerializeField] private TextMeshProUGUI[] textToolSlots;
    [SerializeField] private TextMeshProUGUI[] textToolWarehouse;

    [HideInInspector] public bool slot;
    [HideInInspector] public bool warehouse;
    [HideInInspector] public int warehouseNumber;
    [HideInInspector] public int slotNumber;

    // Use this for initialization
    void Start () {
        boolControl();
        ReadNames();
	}
	
	// Update is called once per frame
	void Update () {
        ReadNames();
	}

    private void boolControl()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == true)
            {
                slot = true;
                slotNumber = i;
                break;
            }
            slot = false;
        }

        for (int i = 0; i < warehouses.Length; i++)
        {
            if (warehouses[i] == true)
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
        if(!slot && !warehouse)
        {
            slots[indice] = true;
        }
        else if (slots[indice])
        {
            slots[indice] = false;
        }
        else if (slot)
        {
            slots[slotNumber] = false;
            slots[indice] = true;
        }
        else if (warehouse)
        {
            GameObject temporalObject = astronautStats.toolSlots[indice]; 
            playerShipActions.TakeTool(warehouseNumber, indice);
            playerShipStats.toolWarehouse[warehouseNumber] = temporalObject;
            ReadNames();
            warehouses[warehouseNumber] = false;
        }

        boolControl();
    }

    public void Warehouse(int indice)
    {
        if (!slot && !warehouse)
        {
            warehouses[indice] = true;
        }
        else if (warehouses[indice])
        {
            warehouses[indice] = false;
        }
        else if (warehouse)
        {
            warehouses[warehouseNumber] = false;
            warehouses[indice] = true;
        }
        else if (slot)
        {
            GameObject temporalObject = playerShipStats.toolWarehouse[indice];
            playerShipActions.SaveTool(slotNumber, indice);
            astronautStats.toolSlots[slotNumber] = temporalObject;
            ReadNames();
            slots[slotNumber] = false;
        }

        boolControl();
    }

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
    }
}
