using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

// 1 REPAIRING TOOL
// 2 GAS EXTRACTOR
// 3 JETPACK

public class Scr_UpgradeButton : MonoBehaviour
{
    [SerializeField] private Color disabledColor;
    [SerializeField] private Color enabledColor;

    [HideInInspector] public bool notActive;
    [HideInInspector] public bool giveUpgrade;
    [HideInInspector] public int upgrade;

    private bool canBeChanged;
    private Scr_PlayerShipStats playerShipStats;

    private void Start()
    {
        playerShipStats = GameObject.Find("PlayerShip").GetComponent<Scr_PlayerShipStats>();

        canBeChanged = true;
    }

    private void Update()
    {
        if (notActive)
        {
            gameObject.GetComponent<Button>().interactable = false;
            gameObject.GetComponent<Image>().color = Color.grey;
            gameObject.GetComponentInChildren<TextMeshProUGUI>().color = disabledColor;
        }

        else
        {
            gameObject.GetComponent<Button>().interactable = true;
            gameObject.GetComponent<Image>().color = enabledColor;
            gameObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        }
    }

    public void BuyUpgrade()
    {
        if (canBeChanged && upgrade != 0)
        {
            if (upgrade == 1)
                playerShipStats.repairingTool = true;

            else if (upgrade == 2)
                playerShipStats.gasExtractor = true;

            else if (upgrade == 3)
                playerShipStats.jetpack = true;

            notActive = true;
            canBeChanged = false;
        }
    }
}