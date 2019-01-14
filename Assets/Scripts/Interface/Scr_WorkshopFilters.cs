using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_WorkshopFilters : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Scr_PlayerShipLaboratory playerShipLaboratory;
    [SerializeField] private Scr_CraftData craftData;
    [SerializeField] private Scr_UpgradeList upgradeList;

    private int currentCategory = 1;

    private void Start()
    {
        FilterTools();
    }

    public void FilterTools()
    {
        currentCategory = 1;

        for (int i = 0; i < playerShipLaboratory.buttonArray.Length; i++)
        {
            if (craftData.CraftList[i].craftType == CraftType.tools)
            {
                if (upgradeList.UpgradeList[i].activated)
                    playerShipLaboratory.buttonArray[i].SetActive(true);
            }

            else
                playerShipLaboratory.buttonArray[i].SetActive(false);
        }
    }

    public void FilterStory()
    {
        currentCategory = 2;

        for (int i = 0; i < playerShipLaboratory.buttonArray.Length; i++)
        {
            if (craftData.CraftList[i].craftType == CraftType.story)
            {
                if (upgradeList.UpgradeList[i].activated)
                    playerShipLaboratory.buttonArray[i].SetActive(true);
            }

            else
                playerShipLaboratory.buttonArray[i].SetActive(false);
        }
    }

    public void FilterSpaceSuit()
    {
        currentCategory = 3;

        for (int i = 0; i < playerShipLaboratory.buttonArray.Length; i++)
        {
            if (craftData.CraftList[i].craftType == CraftType.spaceSuit)
            {
                if (upgradeList.UpgradeList[i].activated)
                    playerShipLaboratory.buttonArray[i].SetActive(true);
            }

            else
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
    }
}
