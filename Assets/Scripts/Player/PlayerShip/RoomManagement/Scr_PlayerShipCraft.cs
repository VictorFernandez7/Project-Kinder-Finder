using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Scr_PlayerShipCraft : MonoBehaviour {

    [Header("References")]
    [SerializeField] private Scr_PlayerShipStats playerShipStats;
    [SerializeField] private Scr_PlayerShipActions playerShipActions;
    [SerializeField] private Scr_AstronautsActions astronautsActions;
    [SerializeField] private Scr_AstronautMovement astronautMovement;
    [SerializeField] private Scr_PlayerShipWarehouse playerShipWarehouse;
    [SerializeField] private Scr_CraftData craftData;
    [SerializeField] private GameObject craftInfoPanel;
    [SerializeField] private Color enoughColor;
    [SerializeField] private Color notEnoughColor;

    [Header("Info References")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI resource1Text;
    [SerializeField] private TextMeshProUGUI resource2Text;
    [SerializeField] private TextMeshProUGUI resource3Text;

    private int resourceListIndex;
    private bool enableCraft;
    private bool onRange;
    private int craftIndex;
    private List<int> crafteableTools = new List<int>();

    private void Start()
    {
        craftIndex = 0;
    }

    private void Update()
    {
        if (onRange && enableCraft && Input.GetKeyDown(KeyCode.E))
            CraftItem();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Astronaut"))
        {
            PanelActive();
            onRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Astronaut"))
        {
            craftInfoPanel.SetActive(false);
            onRange = false;
        }
    }

    private void PanelActive()
    {
        crafteableTools.Clear();

        for(int i = 0; i < craftData.CraftList.Count; i++)
        {
            if (craftData.CraftList[i].crafteable)
                crafteableTools.Add(i);
        }

        print(craftData.CraftList[crafteableTools[craftIndex]].resourceNameList.Count);

        if (crafteableTools.Count > 0)
        {
            int resourceTextIndex = 0;

            List<string> keyr = new List<string>(playerShipWarehouse.Resources.Keys);

            titleText.text = craftData.CraftList[crafteableTools[craftIndex]].m_name;

            resource1Text.gameObject.SetActive(false);
            resource2Text.gameObject.SetActive(false);
            resource3Text.gameObject.SetActive(false);

            craftInfoPanel.SetActive(true);

            for (int i = 0; i < craftData.CraftList[crafteableTools[craftIndex]].resourceAmountList.Count; i++)
            {
                if (craftData.CraftList[crafteableTools[craftIndex]].resourceAmountList[i] > 0 && resourceTextIndex == 0)
                {
                    resource1Text.gameObject.SetActive(true);
                    resource1Text.text = craftData.CraftList[crafteableTools[craftIndex]].resourceNameList[i] + " " + playerShipWarehouse.Resources[keyr[i]] + "/" + craftData.CraftList[crafteableTools[craftIndex]].resourceAmountList[i].ToString();

                    if (playerShipWarehouse.Resources[keyr[i]] > craftData.CraftList[crafteableTools[craftIndex]].resourceAmountList[i])
                    {
                        resource1Text.color = enoughColor;
                        enableCraft = true;
                    }

                    else
                    {
                        resource1Text.color = notEnoughColor;
                        enableCraft = false;
                    }

                    resourceTextIndex += 1;
                }

                else if (craftData.CraftList[crafteableTools[craftIndex]].resourceAmountList[i] > 0 && resourceTextIndex == 1)
                {
                    resource2Text.gameObject.SetActive(true);
                    resource2Text.text = craftData.CraftList[crafteableTools[craftIndex]].resourceNameList[i] + " " + playerShipWarehouse.Resources[keyr[i]] + "/" + craftData.CraftList[crafteableTools[craftIndex]].resourceAmountList[i].ToString();

                    if (playerShipWarehouse.Resources[keyr[i]] > craftData.CraftList[crafteableTools[craftIndex]].resourceAmountList[i])
                    {
                        resource2Text.color = enoughColor;
                        enableCraft = true;
                    }

                    else
                    {
                        resource2Text.color = notEnoughColor;
                        enableCraft = false;
                    }

                    resourceTextIndex += 1;
                }

                else if (craftData.CraftList[crafteableTools[craftIndex]].resourceAmountList[i] > 0 && resourceTextIndex == 2)
                {
                    resource3Text.gameObject.SetActive(true);
                    resource3Text.text = craftData.CraftList[crafteableTools[craftIndex]].resourceNameList[i] + " " + playerShipWarehouse.Resources[keyr[i]] + "/" + craftData.CraftList[crafteableTools[craftIndex]].resourceAmountList[i].ToString();

                    if (playerShipWarehouse.Resources[keyr[i]] > craftData.CraftList[crafteableTools[craftIndex]].resourceAmountList[i])
                    {
                        resource3Text.color = enoughColor;
                        enableCraft = true;
                    }

                    else
                    {
                        resource3Text.color = notEnoughColor;
                        enableCraft = false;
                    }
                }
            }
        }
    }

    public void CraftItem()
    {
        List<string> keyr = new List<string>(playerShipWarehouse.Resources.Keys);

        foreach (string k in keyr)
        {
            for (int p = 0; p < craftData.CraftList[crafteableTools[craftIndex]].resourceNameList.Count; p++)
            {
                if (craftData.CraftList[crafteableTools[craftIndex]].resourceNameList[p] == k)
                {
                    resourceListIndex = p;
                    break;
                }
            }

            if (craftData.CraftList[crafteableTools[craftIndex]].resourceAmountList[resourceListIndex] > 0)
            {
                for (int i = craftData.CraftList[crafteableTools[craftIndex]].resourceAmountList[resourceListIndex]; i > 0; i--)
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

        if(craftData.CraftList[crafteableTools[craftIndex]].craftType != CraftType.jumpCell)
            craftData.CraftList[crafteableTools[craftIndex]].crafteable = false;

        GenerateCraft();
    }

    private void GenerateCraft()
    {
        if(craftData.CraftList[crafteableTools[craftIndex]].craftType == CraftType.tools)
            astronautsActions.unlockedTools[craftData.CraftList[crafteableTools[craftIndex]].ToolNum] = true;

        if (craftData.CraftList[crafteableTools[craftIndex]].craftType == CraftType.jetpack)
            astronautMovement.unlockedJetpack = true;

        if (craftData.CraftList[crafteableTools[craftIndex]].craftType == CraftType.jumpCell)
            playerShipWarehouse.jumpCellAmount += 1;           

        if (craftData.CraftList[crafteableTools[craftIndex]].craftType == CraftType.miningLaser)
            playerShipActions.unlockedMiningLaser = true;

        if (craftData.CraftList[crafteableTools[craftIndex]].craftType == CraftType.spaceship)
            playerShipActions.unlockedMultiJump = true;

        if (craftData.CraftList[crafteableTools[craftIndex]].craftType == CraftType.spacewalk)
            playerShipActions.unlockedSpaceWalk = true;

        if (craftData.CraftList[crafteableTools[craftIndex]].craftType == CraftType.suits)
            playerShipActions.unlockedSuits[craftData.CraftList[crafteableTools[craftIndex]].SuitNum] = true;
    }
}