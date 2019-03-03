using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scr_SystemInfo : MonoBehaviour
{
    [Header("System Information")]
    [SerializeField] public bool earthLikePlanets;
    [SerializeField] public float earthAmount;
    [SerializeField] public bool volcanicPlanets;
    [SerializeField] public float volcanicAmount;
    [SerializeField] public bool aridPlanets;
    [SerializeField] public float aridAmount;
    [SerializeField] public bool frozenPlanets;
    [SerializeField] public float frozenAmount;
    [SerializeField] public bool moons;
    [SerializeField] public float moonAmount;

    [Header("References")]
    [SerializeField] private GameObject planetType1;
    [SerializeField] private TextMeshProUGUI planetType1Amount;
    [SerializeField] private GameObject planetType2;
    [SerializeField] private TextMeshProUGUI planetType2Amount;
    [SerializeField] private GameObject planetType3;
    [SerializeField] private TextMeshProUGUI planetType3Amount;
    [SerializeField] private GameObject planetType4;
    [SerializeField] private TextMeshProUGUI planetType4Amount;
    [SerializeField] private GameObject planetType5;
    [SerializeField] private TextMeshProUGUI planetType5Amount;

    private void Start()
    {
        planetType1.SetActive(earthLikePlanets);
        planetType2.SetActive(volcanicPlanets);
        planetType3.SetActive(aridPlanets);
        planetType4.SetActive(frozenPlanets);
        planetType5.SetActive(moons);

        planetType1Amount.text = "x " + earthAmount;
        planetType2Amount.text = "x " + volcanicAmount;
        planetType3Amount.text = "x " + aridAmount;
        planetType4Amount.text = "x " + frozenAmount;
        planetType5Amount.text = "x " + moonAmount;
    }
}