using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scr_SystemInfo : MonoBehaviour
{
    [Header("System Information")]
    [SerializeField] private bool earthLikePlanets;
    [SerializeField] private float earthAmount;
    [SerializeField] private bool volcanicPlanets;
    [SerializeField] private float volcanicAmount;
    [SerializeField] private bool aridPlanets;
    [SerializeField] private float aridAmount;
    [SerializeField] private bool frozenPlanets;
    [SerializeField] private float frozenAmount;

    [Header("References")]
    [SerializeField] private GameObject planetType1;
    [SerializeField] private TextMeshProUGUI planetType1Amount;
    [SerializeField] private GameObject planetType2;
    [SerializeField] private TextMeshProUGUI planetType2Amount;
    [SerializeField] private GameObject planetType3;
    [SerializeField] private TextMeshProUGUI planetType3Amount;
    [SerializeField] private GameObject planetType4;
    [SerializeField] private TextMeshProUGUI planetType4Amount;

    private void Start()
    {
        planetType1.SetActive(earthLikePlanets);
        planetType2.SetActive(volcanicPlanets);
        planetType3.SetActive(aridPlanets);
        planetType4.SetActive(frozenPlanets);

        planetType1Amount.text = "x " + earthAmount;
        planetType2Amount.text = "x " + volcanicAmount;
        planetType3Amount.text = "x " + aridAmount;
        planetType4Amount.text = "x " + frozenAmount;
    }
}