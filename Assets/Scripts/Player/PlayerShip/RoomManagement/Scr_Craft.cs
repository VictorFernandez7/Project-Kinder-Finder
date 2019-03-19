using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Scr_Craft : MonoBehaviour {

    [Header("References")]
    [SerializeField] private Scr_PlayerShipStats playerShipStats;
    [SerializeField] private Scr_PlayerShipActions playerShipActions;
    [SerializeField] private Scr_AstronautsActions astronautsActions;
    [SerializeField] private Scr_AstronautMovement astronautMovement;
    [SerializeField] private Scr_PlayerShipWarehouse playerShipWarehouse;
    [SerializeField] private Scr_CraftData craftData;
    [SerializeField] private Scr_CraftInterface craftInterface;
    [SerializeField] private Scr_Travel travel;
    [SerializeField] private GameObject rightPanel;

    private int resourceListIndex;

    public void CraftItem()
    {
        List<string> keyr = new List<string>(playerShipWarehouse.Resources.Keys);

        foreach (string k in keyr)
        {
            for (int p = 0; p < craftData.CraftList[craftInterface.index].resourceNameList.Count; p++)
            {
                if (craftData.CraftList[craftInterface.index].resourceNameList[p] == k)
                {
                    resourceListIndex = p;
                    break;
                }
            }

            if (craftData.CraftList[craftInterface.index].resourceAmountList[resourceListIndex] > 0)
            {
                for (int i = craftData.CraftList[craftInterface.index].resourceAmountList[resourceListIndex]; i > 0; i--)
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

        if (craftData.CraftList[craftInterface.index].craftType != CraftType.jumpCell)
        {
            craftData.CraftList[craftInterface.index].crafteable = false;
            rightPanel.SetActive(false);
        }

        else
            craftInterface.UpdateInfo(Scr_CraftInterface.TypeOfCraft.Ship, 1);

        GenerateCraft();

    }

    private void GenerateCraft()
    {
        if(craftData.CraftList[craftInterface.index].craftType == CraftType.tools)
            astronautsActions.unlockedTools[craftData.CraftList[craftInterface.index].ToolNum] = true;

        if (craftData.CraftList[craftInterface.index].craftType == CraftType.jetpack)
            astronautMovement.unlockedJetpack = true;

        if (craftData.CraftList[craftInterface.index].craftType == CraftType.jumpCell)
            playerShipWarehouse.jumpCellAmount += 1;           

        if (craftData.CraftList[craftInterface.index].craftType == CraftType.miningLaser)
            playerShipActions.unlockedMiningLaser = true;

        if (craftData.CraftList[craftInterface.index].craftType == CraftType.spaceship)
            travel.unlockedMultiJump = true;

        if (craftData.CraftList[craftInterface.index].craftType == CraftType.spacewalk)
            playerShipActions.unlockedSpaceWalk = true;

        if (craftData.CraftList[craftInterface.index].craftType == CraftType.suits)
            playerShipActions.unlockedSuits[craftData.CraftList[craftInterface.index].SuitNum] = true;
    }
}