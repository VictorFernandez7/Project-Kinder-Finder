using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scr_AstronautStats : MonoBehaviour
{
    [Header("Oxygen System")]
    [SerializeField] private float oxygenCapacity;

    [Header("References")]
    [SerializeField] public GameObject[] toolSlots;
    [SerializeField] public GameObject[] physicToolSlots;
    [SerializeField] private Slider oxygenSlider;
    [SerializeField] public Animator anim_OxygenPanel;

    [HideInInspector] public float currentOxygen;

    private void Start()
    {
        oxygenSlider.maxValue = oxygenCapacity;
        currentOxygen = oxygenCapacity;
    }

    private void Update()
    {
        if (GetComponent<Scr_AstronautMovement>().breathable == false)
            currentOxygen -= 0.5f * Time.deltaTime;

        oxygenSlider.value = currentOxygen;


    }
}