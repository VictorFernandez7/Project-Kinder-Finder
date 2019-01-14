using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Scr_PlayerShipCraft : MonoBehaviour {

    [Header("References")]
    [SerializeField] public Dictionary<string, int> Resources = new Dictionary<string, int>();
    [SerializeField] private Scr_CraftData craftData;
    [SerializeField] private Button craftButton;

    [Header("Info References")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI resource1Text;
    [SerializeField] private TextMeshProUGUI resource2Text;
    [SerializeField] private TextMeshProUGUI resource3Text;

    private Scr_PlayerShipStats playerShipStats;
    private Scr_PlayerShipLaboratory playerShipLaboratory;
    private int resourceListIndex;
    private int craftIndex;

    private void Start()
    {
        playerShipStats = GetComponentInParent<Scr_PlayerShipStats>();
        playerShipLaboratory = GetComponent<Scr_PlayerShipLaboratory>();

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

    private void CraftButtonActivation(int index)
    {
        InventoryInfo();

        craftButton.interactable = true;

        List<string> keyr = new List<string>(Resources.Keys);

        foreach (string k in keyr)
        {
            for (int p = 0; p < craftData.CraftList[index].resourceNameList.Count; p++)
            {
                if (craftData.CraftList[index].resourceNameList[p] == k)
                {
                    resourceListIndex = p;
                    break;
                }
            }

            if (craftData.CraftList[index].resourceAmountList[resourceListIndex] > Resources[k])
            {
                craftButton.interactable = false;
                break;
            }
        }
    }

    public void RecipeClick(int index)
    {
        int resourceTextIndex = 0;

        craftIndex = index;

        CraftButtonActivation(index);

        List<string> keyr = new List<string>(Resources.Keys);

        titleText.text = craftData.CraftList[index].m_name;
        descriptionText.text = craftData.CraftList[index].m_info;
        
        for(int i = 0; i < craftData.CraftList[index].resourceAmountList.Count; i++)
        {
            if (craftData.CraftList[index].resourceAmountList[i] > 0 && resourceTextIndex == 0)
            {
                resource1Text.text = craftData.CraftList[index].resourceNameList[i] + " " + Resources[keyr[i]] + "/" + craftData.CraftList[index].resourceAmountList[i].ToString();
                resourceTextIndex += 1;
            }

            else if (craftData.CraftList[index].resourceAmountList[i] > 0 && resourceTextIndex == 1)
            {
                resource2Text.text = craftData.CraftList[index].resourceNameList[i] + " " + Resources[keyr[i]] + "/" + craftData.CraftList[index].resourceAmountList[i].ToString();
                resourceTextIndex += 1;
            }

            else if (craftData.CraftList[index].resourceAmountList[i] > 0 && resourceTextIndex == 2)
            {
                resource3Text.text = craftData.CraftList[index].resourceNameList[i] + " " + Resources[keyr[i]] + "/" + craftData.CraftList[index].resourceAmountList[i].ToString();
            }
        }
    }

    public void CraftButton()
    {
        List<string> keyr = new List<string>(Resources.Keys);

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

        CraftButtonActivation(craftIndex);
        GenerateCraft();
    }

    private void GenerateCraft()
    {
        switch (craftData.CraftList[craftIndex].craftResultType)
        {
            case CraftResultType.item:
                playerShipLaboratory.buttonArray[craftIndex].GetComponent<Scr_CraftingResultBase>().ObjectGeneration();
                break;

            case CraftResultType.improvement:
                playerShipLaboratory.buttonArray[craftIndex].GetComponent<Scr_CraftingResultBase>().MakingImprovement();
                break;
        }
        
        
    }
}