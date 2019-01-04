using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Scr_PlayerShipLaboratory : MonoBehaviour
{ 
    [Header("References")]
    [SerializeField] private Scr_UpgradeList upgradeData;
    [SerializeField] private Button upgradeButton;

    private Scr_PlayerShipCraft playerShipCraft;
    private Scr_PlayerShipStats playerShipStats;

    private int resourceListIndex;

    private void Start()
    {
        playerShipCraft = GetComponent<Scr_PlayerShipCraft>();
        playerShipStats = GetComponent<Scr_PlayerShipStats>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {

        }
    }

    public void TechnologyClic(int index)
    {
        if (!upgradeData.UpgradeList[index].activated)
        {
            playerShipCraft.InventoryInfo();

            upgradeButton.interactable = true;

            List<string> keyr = new List<string>(playerShipCraft.Resources.Keys);

            foreach (string k in keyr)
            {
                for(int p = 0; p < upgradeData.UpgradeList[index].resourceNameList.Count; p++)
                {
                    if(upgradeData.UpgradeList[index].resourceNameList[p] == k)
                    {
                        resourceListIndex = p;
                        break;
                    }
                }

                if (upgradeData.UpgradeList[index].resourceAmountList[resourceListIndex] > playerShipCraft.Resources[k])
                {
                    upgradeButton.interactable = false;
                    break;
                }
            }
        }

        else
            upgradeButton.interactable = false;
    }

    public void UpgradeButton(int index)
    {
        List<string> keyr = new List<string>(playerShipCraft.Resources.Keys);

        foreach (string k in keyr)
        {
            for (int p = 0; p < upgradeData.UpgradeList[index].resourceNameList.Count; p++)
            {
                if (upgradeData.UpgradeList[index].resourceNameList[p] == k)
                {
                    resourceListIndex = p;
                    break;
                }
            }

            if (upgradeData.UpgradeList[index].resourceAmountList[resourceListIndex] > 0)
            {
                for(int i = upgradeData.UpgradeList[index].resourceAmountList[resourceListIndex]; i > 0; i--)
                {
                    for (int j = playerShipStats.resourceWarehouse.Length - 1; j >= 0; j--)
                    {
                        if (playerShipStats.resourceWarehouse[j])
                        {
                            if (playerShipStats.resourceWarehouse[j].name == k)
                            {
                                playerShipStats.resourceWarehouse[j] = null;
                                break;
                            }
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
