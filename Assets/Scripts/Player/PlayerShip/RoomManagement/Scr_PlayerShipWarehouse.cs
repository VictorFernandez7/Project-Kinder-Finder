using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Scr_PlayerShipWarehouse : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public Dictionary<string, int> Resources = new Dictionary<string, int>();
    [SerializeField] public bool[] resourcesWarehouseSlots;
    [SerializeField] private Scr_PlayerShipStats playerShipStats;
    [SerializeField] private Scr_PlayerShipActions playerShipActions;
    [SerializeField] private Scr_AstronautStats astronautStats;
    [SerializeField] private Image[] iconResourcesWarehouse;

    [HideInInspector] public bool slot;
    [HideInInspector] public bool warehouse;
    [HideInInspector] public bool resourceWarehouse;
    [HideInInspector] public bool unlockedJumpCell;
    [HideInInspector] public int warehouseNumber;
    [HideInInspector] public int resourceWarehouseNumber;
    [HideInInspector] public int slotNumber;
    [HideInInspector] public int jumpCellAmount;

    void Start ()
    {
        ReadNames();

        Resources.Add("Oxygen", 0);
        Resources.Add("Fuel", 0);
        Resources.Add("Carbon", 0);
        Resources.Add("Silicon", 0);
        Resources.Add("Helium", 0);
        Resources.Add("Airgel", 0);
        Resources.Add("Iron", 0);
        Resources.Add("Aluminum", 0);
        Resources.Add("Magnetite", 0);
        Resources.Add("Mercury", 0);
        Resources.Add("Ceramics", 0);
        Resources.Add("Thermatite", 0);
        Resources.Add("Technology Rests", 0);
        Resources.Add("Energetic Core", 0);
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

    public void Refuel(int index)
    {
        if (!resourcesWarehouseSlots[index])
        {
            for(int i = 0; i < resourcesWarehouseSlots.Length; i++)
            {
                resourcesWarehouseSlots[i] = false;
            }

            resourcesWarehouseSlots[index] = true;
        }

        else if (resourcesWarehouseSlots[index])
        {
            if (playerShipStats.resourceWarehouse[index].name == "Fuel")
            {
                playerShipStats.currentFuel += 50;
                playerShipStats.resourceWarehouse[index] = null;
            }
        }

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
        for (int i = 0; i < playerShipStats.resourceWarehouse.Length; i++)
        {
            if (playerShipStats.resourceWarehouse[i] == null)
                iconResourcesWarehouse[i].enabled = false;

            else
            {
                iconResourcesWarehouse[i].enabled = true;
                iconResourcesWarehouse[i].sprite = playerShipStats.resourceWarehouse[i].GetComponent<Scr_Resource>().icon;
            }
                
        }
    }
}