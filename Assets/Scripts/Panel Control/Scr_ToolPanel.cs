using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scr_ToolPanel : MonoBehaviour {

    public bool[] slots;
    public bool[] warehouses;

    [Header("Script References")]
    [SerializeField] private Scr_PlayerShipStats playerShipStats;
    [SerializeField] private Scr_PlayerShipActions playerShipActions;
    [SerializeField] private Scr_AstronautStats astronautStats;

    [HideInInspector] public bool slot;
    [HideInInspector] public bool warehouse;
    [HideInInspector] public int warehouseNumber;
    [HideInInspector] public int slotNumber;

    // Use this for initialization
    void Start () {
        boolControl();
	}
	
	// Update is called once per frame
	void Update () {

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

    public void Slot1()
    {
        if(!slot && !warehouse)
        {
            slots[0] = true;
        }
        else if (slots[0])
        {
            slots[0] = false;
        }
        else if (slot)
        {
            slots[slotNumber] = false;
            slots[0] = true;
        }
        else if (warehouse)
        {
            GameObject temporalObject = astronautStats.toolSlots[0]; 
            playerShipActions.TakeTool(warehouseNumber, 0);
            playerShipStats.toolWarehouse[warehouseNumber] = temporalObject;
            astronautStats.ReadNames();
            playerShipStats.ReadNames();
            warehouses[warehouseNumber] = false;
        }

        boolControl();
    }

    public void Slot2()
    {
        if (!slot && !warehouse)
        {
            slots[1] = true;
        }
        else if (slots[1])
        {
            slots[1] = false;
        }
        else if (slot)
        {
            slots[slotNumber] = false;
            slots[1] = true;
        }
        else if (warehouse)
        {
            GameObject temporalObject = astronautStats.toolSlots[1];
            playerShipActions.TakeTool(warehouseNumber, 1);
            playerShipStats.toolWarehouse[warehouseNumber] = temporalObject;
            astronautStats.ReadNames();
            playerShipStats.ReadNames();
            warehouses[warehouseNumber] = false;
        }

        boolControl();
    }

    public void Slot3()
    {
        if (!slot && !warehouse)
        {
            slots[2] = true;
        }
        else if (slots[2])
        {
            slots[2] = false;
        }
        else if (slot)
        {
            slots[slotNumber] = false;
            slots[2] = true;
        }
        else if (warehouse)
        {
            GameObject temporalObject = astronautStats.toolSlots[2];
            playerShipActions.TakeTool(warehouseNumber, 2);
            playerShipStats.toolWarehouse[warehouseNumber] = temporalObject;
            astronautStats.ReadNames();
            playerShipStats.ReadNames();
            warehouses[warehouseNumber] = false;
        }

        boolControl();
    }

    public void Warehouse1()
    {
        if (!slot && !warehouse)
        {
            warehouses[0] = true;
        }
        else if (warehouses[0])
        {
            warehouses[0] = false;
        }
        else if (warehouse)
        {
            warehouses[warehouseNumber] = false;
            warehouses[0] = true;
        }
        else if (slot)
        {
            GameObject temporalObject = playerShipStats.toolWarehouse[0];
            playerShipActions.SaveTool(slotNumber, 0);
            astronautStats.toolSlots[slotNumber] = temporalObject;
            astronautStats.ReadNames();
            playerShipStats.ReadNames();
            slots[slotNumber] = false;
        }

        boolControl();
    }

    public void Warehouse2()
    {
        if (!slot && !warehouse)
        {
            warehouses[1] = true;
        }
        else if (warehouses[1])
        {
            warehouses[1] = false;
        }
        else if (warehouse)
        {
            warehouses[warehouseNumber] = false;
            warehouses[1] = true;
        }
        else if (slot)
        {
            GameObject temporalObject = playerShipStats.toolWarehouse[1];
            playerShipActions.SaveTool(slotNumber, 1);
            astronautStats.toolSlots[slotNumber] = temporalObject;
            astronautStats.ReadNames();
            playerShipStats.ReadNames();
            slots[slotNumber] = false;
        }

        boolControl();
    }

    public void Warehouse3()
    {
        if (!slot && !warehouse)
        {
            warehouses[2] = true;
        }
        else if (warehouses[2])
        {
            warehouses[2] = false;
        }
        else if (warehouse)
        {
            warehouses[warehouseNumber] = false;
            warehouses[2] = true;
        }
        else if (slot)
        {
            GameObject temporalObject = playerShipStats.toolWarehouse[2];
            playerShipActions.SaveTool(slotNumber, 2);
            astronautStats.toolSlots[slotNumber] = temporalObject;
            astronautStats.ReadNames();
            playerShipStats.ReadNames();
            slots[slotNumber] = false;
        }

        boolControl();
    }

    public void Warehouse4()
    {
        if (!slot && !warehouse)
        {
            warehouses[3] = true;
        }
        else if (warehouses[3])
        {
            warehouses[3] = false;
        }
        else if (warehouse)
        {
            warehouses[warehouseNumber] = false;
            warehouses[3] = true;
        }
        else if (slot)
        {
            GameObject temporalObject = playerShipStats.toolWarehouse[3];
            playerShipActions.SaveTool(slotNumber, 3);
            astronautStats.toolSlots[slotNumber] = temporalObject;
            astronautStats.ReadNames();
            playerShipStats.ReadNames();
            slots[slotNumber] = false;
        }

        boolControl();
    }

    public void Warehouse5()
    {
        if (!slot && !warehouse)
        {
            warehouses[4] = true;
        }
        else if (warehouses[4])
        {
            warehouses[4] = false;
        }
        else if (warehouse)
        {
            warehouses[warehouseNumber] = false;
            warehouses[4] = true;
        }
        else if (slot)
        {
            GameObject temporalObject = playerShipStats.toolWarehouse[4];
            playerShipActions.SaveTool(slotNumber, 4);
            astronautStats.toolSlots[slotNumber] = temporalObject;
            astronautStats.ReadNames();
            playerShipStats.ReadNames();
            slots[slotNumber] = false;
        }

        boolControl();
    }
}
