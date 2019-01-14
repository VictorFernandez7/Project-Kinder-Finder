using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_WorkshopFilters : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Scr_PlayerShipLaboratory playerShipLaboratory;
    [SerializeField] private Scr_CraftData craftData;
    [SerializeField] private Scr_UpgradeList upgradeList;

    private void Start()
    {
        FilterTools();
    }

    public void FilterTools()
    {
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
}
