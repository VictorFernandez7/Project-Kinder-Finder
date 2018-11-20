using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Scr_PlayerShipActions))]
[RequireComponent(typeof(Scr_PlayerShipPrediction))]
[RequireComponent(typeof(Scr_PlayerShipMovement))]

public class Scr_PlayerShipStats : MonoBehaviour
{
    [Header("Respawn Properties")]
    [SerializeField] private float respawnTime;

    [Header("Fuel Properties")]
    [SerializeField] public float currentFuel;
    [SerializeField] private float maxFuel;
    [SerializeField] private float normalConsume;
    [SerializeField] private float boostConsume;
    [SerializeField] private Color color0;
    [SerializeField] private Color color25;
    [SerializeField] private Color color50;
    [SerializeField] private Color color75;

    [Header("References")]
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] GameObject shipVisuals;

    [Header("Audio")]
    [SerializeField] private AudioSource fuelAlarm;

    [HideInInspector] public GameObject[] toolWarehouse;
    [HideInInspector] public int lastWarehouseEmpty;
    [HideInInspector] public bool warehouseFull;

    private int numberOfSlotsWithTools;
    private bool alarm;
    private Scr_PlayerShipMovement playerShipMovement;
    private Slider fuelSlider;
    private Image fuelSliderFill;
    private Rigidbody2D rb;

    private void Start()
    {
        fuelSlider = GameObject.Find("FuelSlider").GetComponent<Slider>();
        fuelSliderFill = GameObject.Find("FuelFill").GetComponent<Image>();

        playerShipMovement = GetComponent<Scr_PlayerShipMovement>();
        rb = GetComponent<Rigidbody2D>();

        fuelSlider.maxValue = maxFuel;
        toolWarehouse = new GameObject[5];
    }

    private void Update()
    {
        currentFuel = Mathf.Clamp(currentFuel, 0f, maxFuel);
        fuelSlider.value = currentFuel;

        FuelSliderColor();

        if(currentFuel < 100 && !alarm)
        {
            fuelAlarm.Play();
            alarm = true;
        }
    }

    public void FuelConsumption(bool boost)
    {
        if (boost)
            currentFuel -= boostConsume;

        else
            currentFuel -= normalConsume;
    }

    public void ReFuel(float amount)
    {
        currentFuel += amount;
    }

    private void FuelSliderColor()
    {
        float colorChangeSpeed = 2;

        if (fuelSlider.value >= (0.75f * fuelSlider.maxValue))
            fuelSliderFill.color = Color.Lerp(fuelSliderFill.color, color75, Time.deltaTime * colorChangeSpeed);
        if (fuelSlider.value <= (0.75f * fuelSlider.maxValue))
            fuelSliderFill.color = Color.Lerp(fuelSliderFill.color, color50, Time.deltaTime * colorChangeSpeed);
        if (fuelSlider.value <= (0.50f * fuelSlider.maxValue))
            fuelSliderFill.color = Color.Lerp(fuelSliderFill.color, color25, Time.deltaTime * colorChangeSpeed);
        if (fuelSlider.value <= (0.25f * fuelSlider.maxValue))
            fuelSliderFill.color = Color.Lerp(fuelSliderFill.color, color0, Time.deltaTime * colorChangeSpeed);
    }

    public void Death()
    {
        playerShipMovement.canControlShip = false;
        playerShipMovement.dead = true;
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        shipVisuals.gameObject.SetActive(false);
        deathParticles.Play();
        playerShipMovement.thrusterParticles.Stop();

        Invoke("Respawn", respawnTime);
    }

    private void Respawn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RecalculateWarehouse(int warehouseSlot)
    {
        if (warehouseSlot < numberOfSlotsWithTools - 1)
        {
            for (int i = numberOfSlotsWithTools - 1; toolWarehouse[i] != null; i--)
            {
                toolWarehouse[warehouseSlot] = toolWarehouse[i];
                toolWarehouse[i] = null;
            }
        }
    }
}