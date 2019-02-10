using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_WorkshopFilters : MonoBehaviour
{/*
    [Header("References")]
    [SerializeField] private Scr_PlayerShipLaboratory playerShipLaboratory;
    [SerializeField] private Scr_CraftData craftData;
    

    private int currentCategory = 1;

    public void FilterTools()
    {
        currentCategory = 1;

        for(int i = 0; i < upgradeList.UpgradeList.Count; i++)
        {
            if (upgradeList.UpgradeList[i].activated)
            {
                for (int j = 0; j < upgradeList.UpgradeList[i].recipeList.Count; j++)
                {
                    playerShipLaboratory.buttonArray[upgradeList.UpgradeList[i].recipeList[j]].SetActive(true);
                }
            }
        }

        for (int i = 0; i < playerShipLaboratory.buttonArray.Length; i++)
        {
            if (craftData.CraftList[i].craftType != CraftType.tools)
                playerShipLaboratory.buttonArray[i].SetActive(false); 
        }
    }

    public void FilterStory()
    {
        currentCategory = 2;

        for (int i = 0; i < upgradeList.UpgradeList.Count; i++)
        {
            if (upgradeList.UpgradeList[i].activated)
            {
                for (int j = 0; j < upgradeList.UpgradeList[i].recipeList.Count; j++)
                {
                    playerShipLaboratory.buttonArray[upgradeList.UpgradeList[i].recipeList[j]].SetActive(true);
                }
            }
        }

        for (int i = 0; i < playerShipLaboratory.buttonArray.Length; i++)
        {
            if (craftData.CraftList[i].craftType != CraftType.story)
                playerShipLaboratory.buttonArray[i].SetActive(false);
        }
    }

    public void FilterSpaceSuit()
    {
        currentCategory = 3;

        for (int i = 0; i < upgradeList.UpgradeList.Count; i++)
        {
            if (upgradeList.UpgradeList[i].activated)
            {
                for (int j = 0; j < upgradeList.UpgradeList[i].recipeList.Count; j++)
                {
                    playerShipLaboratory.buttonArray[upgradeList.UpgradeList[i].recipeList[j]].SetActive(true);
                }
            }
        }

        for (int i = 0; i < playerShipLaboratory.buttonArray.Length; i++)
        {
            if (craftData.CraftList[i].craftType != CraftType.spaceSuit)
                playerShipLaboratory.buttonArray[i].SetActive(false);
        }
    }

    public void WorkShopButton()
    {
        switch (currentCategory)
        {
            case 1:
                FilterTools();
                break;
            case 2:
                FilterStory();
                break;
            case 3:
                FilterSpaceSuit();
                break;
        }
    }*/
}
