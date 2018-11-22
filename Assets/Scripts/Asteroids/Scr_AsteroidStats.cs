using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Scr_AsteroidStats : MonoBehaviour
{
    [Header("Mining System")]
    [Range(0, 1)] [SerializeField] public float explosionZone;
    [Range(0, 1)] [SerializeField] public float resourceZone;
    [Range(0, 1)] [SerializeField] public float resistentZone;

    [Header("Resources: Steel")]
    [SerializeField] public float steelAmount;
    [SerializeField] public GameObject steelBlock;

    [Header("References")]
    [SerializeField] private Slider explosionSlider;
    [SerializeField] private Slider resourceSlider;
    [SerializeField] private Slider resistentSlider;
    [SerializeField] private Slider currentPowerSlider;

    [HideInInspector] public float currentPower;
    [HideInInspector] public bool mining;

    private float regenSpeed;
    private Scr_PlayerShipActions playerShipActions;

    private void Start()
    {
        playerShipActions = GameObject.Find("PlayerShip").GetComponent<Scr_PlayerShipActions>();

        SliderSet();
    }

    private void Update()
    {
        currentPowerSlider.value = currentPower / 100;

        if (!mining && currentPowerSlider.value > 0)
        {
            regenSpeed = currentPowerSlider.value * 25;
            currentPower -= regenSpeed * Time.deltaTime;
        }
    }

    private void SliderSet()
    {
        explosionSlider.value = explosionZone;
        resourceSlider.value = resourceZone;
        resistentSlider.value = resistentZone;
    }
}