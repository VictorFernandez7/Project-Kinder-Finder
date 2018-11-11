using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Scr_InterfaceManager : MonoBehaviour
{
    [Header("Interface Properties")]
    [SerializeField] private Color disabledColor;
    [SerializeField] private Color activatedColor;

    [Header("References")]
    [SerializeField] private GameObject astronautButton;
    [SerializeField] private GameObject playerShipButton;

    public void ActivateAstronautButton()
    {
        astronautButton.GetComponent<Image>().color = activatedColor;
    }

    public void DeactivateAstronautButton()
    {
        astronautButton.GetComponent<Image>().color = disabledColor;
    }

    public void ActivatePlayerShipButton()
    {
        playerShipButton.GetComponent<Image>().color = activatedColor;
    }

    public void DeactivatePlayerShipButton()
    {
        playerShipButton.GetComponent<Image>().color = disabledColor;
    }
}