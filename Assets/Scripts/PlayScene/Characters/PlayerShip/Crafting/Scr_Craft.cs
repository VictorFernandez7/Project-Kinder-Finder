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
    [SerializeField] private GameObject rightPanel;

    [Header("Sound")]
    [SerializeField] private SoundDefinition upgrade;

    private int resourceListIndex;

    public void CraftItem()
    {
        Scr_MusicManager.Instance.PlaySound(upgrade.Sound, 0);

        List<string> keyr = new List<string>(playerShipWarehouse.Resources.Keys);

        foreach (string k in keyr)
        {
            bool pickUp = false;
            bool up = false;
            bool down = false;
            bool right = false;
            bool left = false;

            if (astronautsActions.pickPoint.childCount > 0)
            {
                if (astronautsActions.pickPoint.GetChild(0).name.Contains(k))
                    pickUp = true;
            }

            if (astronautsActions.iaSpotTransformDown.childCount > 0)
            {
                if (astronautsActions.iaSpotTransformDown.transform.GetChild(0).name.Contains(k))
                    down = true;
            }

            if (astronautsActions.iaSpotTransformLeft.childCount > 0)
            {
                if (astronautsActions.iaSpotTransformLeft.GetChild(0).name.Contains(k))
                    left = true;
            }

            if (astronautsActions.iaSpotTransformRight.childCount > 0)
            {
                if (astronautsActions.iaSpotTransformRight.GetChild(0).name.Contains(k))
                    right = true;
            }

            if (astronautsActions.iaSpotTransformUp.childCount > 0)
            {
                if (astronautsActions.iaSpotTransformUp.GetChild(0).name.Contains(k))
                    up = true;
            }

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
                    if(pickUp)
                    {
                        Destroy(astronautsActions.pickPoint.GetChild(0).gameObject);
                        pickUp = false;
                        astronautsActions.resourceIndex -= 1;
                    }

                    else if (down)
                    {
                        Destroy(astronautsActions.iaSpotTransformDown.transform.GetChild(0).gameObject);
                        down = false;
                        astronautsActions.resourceIndex -= 1;
                    }

                    else if (right)
                    {
                        Destroy(astronautsActions.iaSpotTransformRight.GetChild(0).gameObject);
                        right = false;
                        astronautsActions.resourceIndex -= 1;
                    }

                    else if (up)
                    {
                        Destroy(astronautsActions.iaSpotTransformUp.GetChild(0).gameObject);
                        up = false;
                        astronautsActions.resourceIndex -= 1;
                    }

                    else if (left)
                    {
                        Destroy(astronautsActions.iaSpotTransformLeft.GetChild(0).gameObject);
                        left = false;
                        astronautsActions.resourceIndex -= 1;
                    }

                    else
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
        {
            astronautsActions.unlockedTools[craftData.CraftList[craftInterface.index].ToolNum] = true;
            Scr_LevelManager.unlockedTools[craftData.CraftList[craftInterface.index].ToolNum] = true;
        }

        if (craftData.CraftList[craftInterface.index].craftType == CraftType.jetpack)
            astronautMovement.unlockedJetpack = true;

        if (craftData.CraftList[craftInterface.index].craftType == CraftType.jumpCell)
            playerShipWarehouse.jumpCellAmount += 1;           

        if (craftData.CraftList[craftInterface.index].craftType == CraftType.miningLaser)
            playerShipActions.unlockedMiningLaser = true;

        if (craftData.CraftList[craftInterface.index].craftType == CraftType.spaceship)
            Scr_Travel.unlockedMultiJump = true;

        if (craftData.CraftList[craftInterface.index].craftType == CraftType.spacewalk)
            playerShipActions.unlockedSpaceWalk = true;

        if (craftData.CraftList[craftInterface.index].craftType == CraftType.suits)
            playerShipActions.unlockedSuits[craftData.CraftList[craftInterface.index].SuitNum] = true;
    }
}