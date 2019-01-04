using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_PlayerShipLaboratory : MonoBehaviour
{ 
    [Header("References")]
    [SerializeField] private Scr_UpgradeList upgradeData;

    private Scr_PlayerShipCraft playerShipCraft;
    private Scr_PlayerShipStats playerShipStats;

    private void Start()
    {
        playerShipCraft = GetComponent<Scr_PlayerShipCraft>();
        playerShipStats = GetComponent<Scr_PlayerShipStats>();
    }

    public void TechnologyClic(int index)
    {
        if (!upgradeData.UpgradeList[index].activated)
        {
            playerShipCraft.InventoryInfo();

            //Codigo activando el boton

            List<string> keyr = new List<string>(playerShipCraft.Resources.Keys);

            foreach (string k in keyr)
            {
                if (upgradeData.UpgradeList[index].Resources[k] > playerShipCraft.Resources[k])
                {
                    //Codigo desactivando el boton
                    break;
                }
            }
        }

        else
            Debug.Log(0); //Codigo desactivando el boton
    }

    public void UpgradeButton(int index)
    {
        List<string> keyr = new List<string>(playerShipCraft.Resources.Keys);

        foreach (string k in keyr)
        {
            if (upgradeData.UpgradeList[index].Resources[k] > 0)
            {
                for(int i = upgradeData.UpgradeList[index].Resources[k]; i > 0; i--)
                {
                    for (int j = playerShipStats.resourceWarehouse.Length; j > 0; j--)
                    {
                        if(playerShipStats.resourceWarehouse[j].name == k)
                        {
                            playerShipStats.resourceWarehouse[j] = null;
                            break;
                        }
                    }
                }
            }
        }

        upgradeData.UpgradeList[index].activated = true;
        GenerateRecipe(index); 
    }

    private void GenerateRecipe(int index)
    {

    }
}
