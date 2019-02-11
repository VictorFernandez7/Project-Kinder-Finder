using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Scr_PlayerShipCraft : MonoBehaviour {

    [Header("References")]
    [SerializeField] private Scr_CraftData craftData;
    [SerializeField] private GameObject craftInfoPanel;
    [SerializeField] private int craftIndex;
    [SerializeField] private Color enoughColor;
    [SerializeField] private Color notEnoughColor;

    [Header("Info References")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI resource1Text;
    [SerializeField] private TextMeshProUGUI resource2Text;
    [SerializeField] private TextMeshProUGUI resource3Text;

    private Scr_PlayerShipStats playerShipStats;
    private Scr_PlayerShipWarehouse playerShipWarehouse;
    private int resourceListIndex;
    private bool enableCraft;
    private bool onRange;

    private void Start()
    {
        playerShipStats = GetComponentInParent<Scr_PlayerShipStats>();
        playerShipWarehouse = GetComponentInParent<Scr_PlayerShipWarehouse>();
    }

    private void Update()
    {
        if (onRange && enableCraft && Input.GetKeyDown(KeyCode.E))
            CraftItem();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PanelActive();
            onRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            craftInfoPanel.SetActive(false);
            onRange = false;
        }
    }

    public void PanelActive()
    {
        int resourceTextIndex = 0;

        List<string> keyr = new List<string>(playerShipWarehouse.Resources.Keys);

        titleText.text = craftData.CraftList[craftIndex].m_name;
        descriptionText.text = craftData.CraftList[craftIndex].m_info;

        resource1Text.gameObject.SetActive(false);
        resource2Text.gameObject.SetActive(false);
        resource3Text.gameObject.SetActive(false);

        craftInfoPanel.SetActive(true);

        for (int i = 0; i < craftData.CraftList[craftIndex].resourceAmountList.Count; i++)
        {
            if (craftData.CraftList[craftIndex].resourceAmountList[i] > 0 && resourceTextIndex == 0)
            {
                resource1Text.gameObject.SetActive(true);
                resource1Text.text = craftData.CraftList[craftIndex].resourceNameList[i] + " " + playerShipWarehouse.Resources[keyr[i]] + "/" + craftData.CraftList[craftIndex].resourceAmountList[i].ToString();

                if (playerShipWarehouse.Resources[keyr[i]] > craftData.CraftList[craftIndex].resourceAmountList[i])
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

            else if (craftData.CraftList[craftIndex].resourceAmountList[i] > 0 && resourceTextIndex == 1)
            {
                resource2Text.gameObject.SetActive(true);
                resource2Text.text = craftData.CraftList[craftIndex].resourceNameList[i] + " " + playerShipWarehouse.Resources[keyr[i]] + "/" + craftData.CraftList[craftIndex].resourceAmountList[i].ToString();

                if (playerShipWarehouse.Resources[keyr[i]] > craftData.CraftList[craftIndex].resourceAmountList[i])
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

            else if (craftData.CraftList[craftIndex].resourceAmountList[i] > 0 && resourceTextIndex == 2)
            {
                resource3Text.gameObject.SetActive(true);
                resource3Text.text = craftData.CraftList[craftIndex].resourceNameList[i] + " " + playerShipWarehouse.Resources[keyr[i]] + "/" + craftData.CraftList[craftIndex].resourceAmountList[i].ToString();

                if (playerShipWarehouse.Resources[keyr[i]] > craftData.CraftList[craftIndex].resourceAmountList[i])
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

    public void CraftItem()
    {
        List<string> keyr = new List<string>(playerShipWarehouse.Resources.Keys);

        foreach (string k in keyr)
        {
            for (int p = 0; p < craftData.CraftList[craftIndex].resourceNameList.Count; p++)
            {
                if (craftData.CraftList[craftIndex].resourceNameList[p] == k)
                {
                    resourceListIndex = p;
                    break;
                }
            }

            if (craftData.CraftList[craftIndex].resourceAmountList[resourceListIndex] > 0)
            {
                for (int i = craftData.CraftList[craftIndex].resourceAmountList[resourceListIndex]; i > 0; i--)
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

        craftData.CraftList[craftIndex].crafteable = false;
        GenerateCraft();
    }

    private void GenerateCraft()
    {
        switch (craftData.CraftList[craftIndex].craftResultType)
        {
            case CraftResultType.item:
                GetComponent<Scr_CraftingResultBase>().ObjectGeneration();
                break;

            case CraftResultType.improvement:
                GetComponent<Scr_CraftingResultBase>().MakingImprovement();
                break;
        }
    }
}