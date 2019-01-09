using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_PlayerShipCraft : MonoBehaviour {

    [SerializeField] public Dictionary<string, int> Resources = new Dictionary<string, int>();
    [SerializeField] private Scr_CraftData craftData;
    [SerializeField] private GameObject[] buttonArray;

    private Scr_PlayerShipStats playerShipStats;

    private void Start()
    {
        playerShipStats = GetComponent<Scr_PlayerShipStats>();

        Resources.Add("Fuel", 0);
        Resources.Add("Iron", 0);
    }

    private void Update()
    {
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
}