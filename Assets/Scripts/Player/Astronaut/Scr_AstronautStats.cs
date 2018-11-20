using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Scr_AstronautsActions))]
[RequireComponent(typeof(Scr_AstronautMovement))]

public class Scr_AstronautStats : MonoBehaviour
{
    [Header("Oxygen System")]
    [SerializeField] private float oxygenCapacity;

    [Header("Text References")]
    [SerializeField] private TextMeshProUGUI[] textToolSlots;
    [SerializeField] public GameObject[] toolSlots;

    [HideInInspector] public float currentOxygen;

    private Slider oxygenSlider;

    private void Start()
    {
        oxygenSlider = GameObject.Find("OxygenSlider").GetComponent<Slider>();

        oxygenSlider.maxValue = oxygenCapacity;
        currentOxygen = oxygenCapacity;

        for(int i = 0; i < toolSlots.Length; i++)
        {
            if (toolSlots[i] == null)
                textToolSlots[i].text = "null";
            else
                textToolSlots[i].text = toolSlots[i].name;
        }
    }

    private void Update()
    {
        if (GetComponent<Scr_AstronautMovement>().breathable == false)
            currentOxygen -= 0.5f * Time.deltaTime;

        oxygenSlider.value = currentOxygen;
    }
}