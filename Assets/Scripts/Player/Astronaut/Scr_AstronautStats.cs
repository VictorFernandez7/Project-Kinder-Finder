using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scr_AstronautStats : MonoBehaviour
{
    [Header("Oxygen System")]
    [SerializeField] private float maxOxygen;
    [Range(0, 100)] [SerializeField] private float oxygenAlertPercentage;

    [Header("Health System")]
    [SerializeField] private float maxHealth;
    [Range(0, 100)] [SerializeField] private float healthAlertPercentage;

    [Header("References")]
    [SerializeField] private Slider oxygenSlider;
    [SerializeField] private Slider healthSlider;
    [SerializeField] public Animator anim_OxygenPanel;
    [SerializeField] public Animator anim_HealthPanel;
    [SerializeField] public List<GameObject> toolSlots;

    [HideInInspector] public float currentOxygen;
    [HideInInspector] public float currentHealth;

    private void Start()
    {
        InitialSet();

        toolSlots = new List<GameObject>();
    }

    private void Update()
    {
        Oxygen();
        Health();
    }

    private void InitialSet()
    {
        oxygenSlider.maxValue = maxOxygen;
        healthSlider.maxValue = maxHealth;
        currentOxygen = maxOxygen;
        currentHealth = maxHealth;
    }

    private void Oxygen()
    {
        oxygenSlider.value = currentOxygen;

        if (GetComponent<Scr_AstronautMovement>().breathable == false)
            currentOxygen -= 0.5f * Time.deltaTime;

        if (currentOxygen <= ((oxygenAlertPercentage / 100) * maxOxygen))
            anim_OxygenPanel.SetBool("Alert", true);

        else
            anim_OxygenPanel.SetBool("Alert", false);
    }

    private void Health()
    {
        healthSlider.value = currentHealth;

        if (currentHealth <= ((healthAlertPercentage / 100) * maxHealth))
            anim_HealthPanel.SetBool("Alert", true);

        else
            anim_HealthPanel.SetBool("Alert", false);
    }

    public void TakeDamaged(float damage)
    {
        currentHealth -= damage;
    }
}