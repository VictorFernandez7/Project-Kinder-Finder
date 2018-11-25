using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Scr_Upgrade : MonoBehaviour
{
    [Header("Upgrade Parameters")]
    [SerializeField] private string upgradeName;
    [SerializeField] [TextArea] private string description;
    [SerializeField] private Image descriptionImage;
    [SerializeField] [TextArea] private string requirements;
    [SerializeField] [TextArea] private string price;
    [SerializeField] private bool givesUpgrade;
    [SerializeField] private bool isRepairingTool;
    [SerializeField] private bool isExtractor;
    [SerializeField] private bool isJetpack;
    [SerializeField] private bool cantBuyThis;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI upgradeNameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Image descriptionImageImage;
    [SerializeField] private TextMeshProUGUI requirementsText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Scr_UpgradeButton upgradeButton;

    public void ApplyTextChange()
    {
        upgradeNameText.text = upgradeName;
        descriptionText.text = description;
        descriptionImageImage = descriptionImage;
        requirementsText.text = requirements;
        priceText.text = price;

        if (cantBuyThis)
            upgradeButton.notActive = true;

        else
            upgradeButton.notActive = false;

        if (givesUpgrade)
        {
            upgradeButton.giveUpgrade = true;

            if (isRepairingTool)
                upgradeButton.upgrade = 1;

            else if (isExtractor)
                upgradeButton.upgrade = 2;

            else if (isJetpack)
                upgradeButton.upgrade = 3;

            else
                upgradeButton.upgrade = 0;
        }

        else
        {
            upgradeButton.giveUpgrade = false;
        }
    }
}