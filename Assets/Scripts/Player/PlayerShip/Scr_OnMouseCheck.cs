using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scr_OnMouseCheck : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public GameObject panel;
    [SerializeField] private Slider fuelSlider;
    [SerializeField] private Slider shieldSlider;
    [SerializeField] private GameObject playerShip;

    private void Start()
    {
        fuelSlider.maxValue = playerShip.GetComponent<Scr_PlayerShipStats>().maxFuel;
        shieldSlider.maxValue = playerShip.GetComponent<Scr_PlayerShipStats>().maxShield;
    }

    private void Update()
    {
        fuelSlider.value = playerShip.GetComponent<Scr_PlayerShipStats>().currentFuel;
        shieldSlider.value = playerShip.GetComponent<Scr_PlayerShipStats>().currentShield;

        if (playerShip.GetComponent<Scr_PlayerShipMovement>().astronautOnBoard)
        {
            panel.SetActive(false);
        }
    }

    private void OnMouseEnter()
    {
        panel.SetActive(true);
    }

    private void OnMouseExit()
    {
        panel.SetActive(false);
    }
}
