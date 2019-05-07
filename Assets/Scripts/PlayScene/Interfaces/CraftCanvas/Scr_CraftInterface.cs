using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Scr_CraftInterface : MonoBehaviour
{
    [Header("Interface References")]
    [SerializeField] private Image craftIcon;
    [SerializeField] private TextMeshProUGUI craftName;
    [SerializeField] private TextMeshProUGUI craftDescription;
    [SerializeField] private Image resource1Icon;
    [SerializeField] private TextMeshProUGUI resource1Amount;
    [SerializeField] private GameObject resource2;
    [SerializeField] private Image resource2Icon;
    [SerializeField] private TextMeshProUGUI resource2Amount;
    [SerializeField] private GameObject resource3;
    [SerializeField] private Image resource3Icon;
    [SerializeField] private TextMeshProUGUI resource3Amount;
    [SerializeField] private Button craftButton;
    
    [Header("Interface Style")]
    [SerializeField] private Color colorWithoutResources;
    [SerializeField] private Color colorWithResources;

    [Header("References")]
    [SerializeField] private Scr_CraftData craftData;
    [SerializeField] private Scr_ReferenceManager referenceManager;
    [SerializeField] private Scr_PlayerShipWarehouse playerShipWarehouse;

    [HideInInspector] public int index;

    private bool enableCraft;

    public enum TypeOfCraft
    {
        Ship,
        Tool,
        Suit
    }

    public void UpdateInfo(TypeOfCraft typeOfCraft, int id)
    {
        switch (typeOfCraft)
        {
            case TypeOfCraft.Ship:
                if (id == 0)
                    index = 9;
                else if (id == 1)
                    index = 12;
                else if (id == 2)
                    index = 11;
                else if (id == 3)
                    index = 10;
                break;

            case TypeOfCraft.Suit:
                if (id == 0)
                    index = 7;
                else if (id == 1)
                    index = 6;
                else if (id == 2)
                    index = 5;
                break;

            case TypeOfCraft.Tool:
                if (id == 0)
                    index = 2;
                else if (id == 1)
                    index = 1;
                else if (id == 2)
                    index = 3;
                else if (id == 3)
                    index = 4;
                else if (id == 4)
                    index = 8;
                break;
        }

        List<string> keyr = new List<string>(playerShipWarehouse.Resources.Keys);

        craftIcon = craftData.CraftList[index].m_icon;
        craftName.text = craftData.CraftList[index].m_name;
        craftDescription.text = craftData.CraftList[index].m_info;

        resource2.SetActive(false);
        resource3.SetActive(false);

        int res = 0;

        playerShipWarehouse.InventoryInfo();

        for(int i = 0; i < craftData.CraftList[index].resourceNameList.Count; i++)
        {
            if(res == 0 && craftData.CraftList[index].resourceAmountList[i] != 0)
            {
                resource1Icon.sprite = referenceManager.Resources[i].GetComponent<Scr_Resource>().icon;
                resource1Amount.text = "x " + craftData.CraftList[index].resourceAmountList[i];

                if (playerShipWarehouse.Resources[keyr[i]] >= craftData.CraftList[index].resourceAmountList[i])
                {
                    resource1Amount.color = colorWithResources;
                    enableCraft = true;
                }

                else
                {
                    resource1Amount.color = colorWithoutResources;
                    enableCraft = false;
                }

                res += 1;
            }

            else if(res == 1 && craftData.CraftList[index].resourceAmountList[i] != 0)
            {
                resource2.SetActive(true);

                resource2Icon.sprite = referenceManager.Resources[i].GetComponent<Scr_Resource>().icon;
                resource2Amount.text = "x " + craftData.CraftList[index].resourceAmountList[i];

                if (playerShipWarehouse.Resources[keyr[i]] >= craftData.CraftList[index].resourceAmountList[i])
                {
                    resource2Amount.color = colorWithResources;
                    enableCraft = true;
                }

                else
                {
                    resource2Amount.color = colorWithoutResources;
                    enableCraft = false;
                }

                res += 1;
            }

            else if(res == 2 && craftData.CraftList[index].resourceAmountList[i] != 0)
            {
                resource3.SetActive(true);

                resource3Icon.sprite = referenceManager.Resources[i].GetComponent<Scr_Resource>().icon;
                resource3Amount.text = "x " + craftData.CraftList[index].resourceAmountList[i];

                if (playerShipWarehouse.Resources[keyr[i]] >= craftData.CraftList[index].resourceAmountList[i])
                {
                    resource3Amount.color = colorWithResources;
                    enableCraft = true;
                }

                else
                {
                    resource3Amount.color = colorWithoutResources;
                    enableCraft = false;
                }
            }
        }

        if (!enableCraft)
            craftButton.interactable = false;
        else
            craftButton.interactable = true;
    }
}
