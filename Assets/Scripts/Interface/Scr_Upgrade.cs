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

    [Header("References")]
    [SerializeField] private TextMeshProUGUI upgradeNameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Image descriptionImageImage;
    [SerializeField] private TextMeshProUGUI requirementsText;
    [SerializeField] private TextMeshProUGUI priceText;

    public void ApplyTextChange()
    {
        upgradeNameText.text = upgradeName;
        descriptionText.text = description;
        descriptionImageImage = descriptionImage;
        requirementsText.text = requirements;
        priceText.text = price;
    }
}