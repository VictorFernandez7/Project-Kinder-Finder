using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Scr_PlayerShipStats : MonoBehaviour
{
    [Header("Respawn Properties")]
    [SerializeField] private float respawnTime;

    [Header("Shield Properties")]
    [SerializeField] public float currentShield;
    [SerializeField] public float maxShield;
    [SerializeField] private Color shieldColor0;
    [SerializeField] private Color shieldColor25;
    [SerializeField] private Color shieldColor50;
    [SerializeField] private Color shieldColor75;

    [Header("Fuel Properties")]
    [SerializeField] public float currentFuel;
    [SerializeField] public float maxFuel;
    [SerializeField] private float normalConsume;
    [SerializeField] private float boostConsume;
    [SerializeField] private Color fuelColor0;
    [SerializeField] private Color fuelColor25;
    [SerializeField] private Color fuelColor50;
    [SerializeField] private Color fuelColor75;

    [Header("References")]
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] GameObject shipVisuals;

    [SerializeField] public GameObject[] toolWarehouse;
    [SerializeField] public GameObject[] resourceWarehouse;
    [SerializeField] public Scr_ReferenceManager referenceManager;

    [HideInInspector] public int lastWarehouseEmpty;
    [HideInInspector] public bool gasExtractor;
    [HideInInspector] public bool repairingTool;
    [HideInInspector] public bool jetpack;
    [HideInInspector] public bool inDanger;

    private int numberOfSlotsWithTools;
    private bool alarm;
    private Image fuelSliderFill;
    private Image shieldSliderFill;
    private Slider fuelSlider;
    private Slider shieldSlider;
    private Rigidbody2D rb;
    private Scr_PlayerShipMovement playerShipMovement;
    private Scr_PlayerShipEffects playerShipEffects;

    private void Start()
    {
        fuelSlider = GameObject.Find("FuelSlider").GetComponent<Slider>();
        shieldSlider = GameObject.Find("ShieldSlider").GetComponent<Slider>();
        fuelSliderFill = GameObject.Find("FuelFill").GetComponent<Image>();
        shieldSliderFill = GameObject.Find("ShieldFill").GetComponent<Image>();

        playerShipMovement = GetComponent<Scr_PlayerShipMovement>();
        playerShipEffects = GetComponent<Scr_PlayerShipEffects>();
        rb = GetComponent<Rigidbody2D>();

        fuelSlider.maxValue = maxFuel;
        shieldSlider.maxValue = maxShield;
    }

    private void Update()
    {
        currentFuel = Mathf.Clamp(currentFuel, 0f, maxFuel);
        currentShield = Mathf.Clamp(currentShield, 0f, maxShield);
        fuelSlider.value = currentFuel;
        shieldSlider.value = currentShield;

        FuelSliderColor();
        ShieldSliderColor();

        if (currentFuel < 100 && !alarm)
            alarm = true;

        if (currentShield < 0)
            Death();

        if (gasExtractor)
        {
            for(int i = 0; i < toolWarehouse.Length; i++)
            {
                if (toolWarehouse[i] == null)
                {
                    toolWarehouse[i] = referenceManager.GasExtractor;
                    gasExtractor = false;
                    break;
                }
            }
        }

        if (repairingTool)
        {
            for (int i = 0; i < toolWarehouse.Length; i++)
            {
                if (toolWarehouse[i] == null)
                {
                    toolWarehouse[i] = referenceManager.RepairingTool;
                    repairingTool = false;
                    break;
                }
            }
        }

        if (jetpack)
        {
            for (int i = 0; i < toolWarehouse.Length; i++)
            {
                if (toolWarehouse[i] == null)
                {
                    toolWarehouse[2] = referenceManager.Jetpack;
                    jetpack = false;
                    break;
                }
            }
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
        if (currentFuel < maxFuel)
            currentFuel += amount;
    }

    public void RepairShield(float amount)
    {
        if (currentShield < maxShield)
            currentShield += amount;
    }

    private void FuelSliderColor()
    {
        float colorChangeSpeed = 2;

        if (fuelSlider.value >= (0.75f * fuelSlider.maxValue))
            fuelSliderFill.color = Color.Lerp(fuelSliderFill.color, fuelColor75, Time.deltaTime * colorChangeSpeed);
        if (fuelSlider.value <= (0.75f * fuelSlider.maxValue))
            fuelSliderFill.color = Color.Lerp(fuelSliderFill.color, fuelColor50, Time.deltaTime * colorChangeSpeed);
        if (fuelSlider.value <= (0.50f * fuelSlider.maxValue))
            fuelSliderFill.color = Color.Lerp(fuelSliderFill.color, fuelColor25, Time.deltaTime * colorChangeSpeed);
        if (fuelSlider.value <= (0.25f * fuelSlider.maxValue))
            fuelSliderFill.color = Color.Lerp(fuelSliderFill.color, fuelColor0, Time.deltaTime * colorChangeSpeed);
    }

    private void ShieldSliderColor()
    {
        float colorChangeSpeed = 2;

        if (shieldSlider.value >= (0.75f * shieldSlider.maxValue))
            shieldSliderFill.color = Color.Lerp(shieldSliderFill.color, shieldColor75, Time.deltaTime * colorChangeSpeed);
        if (shieldSlider.value <= (0.75f * shieldSlider.maxValue))
            shieldSliderFill.color = Color.Lerp(shieldSliderFill.color, shieldColor50, Time.deltaTime * colorChangeSpeed);
        if (shieldSlider.value <= (0.50f * shieldSlider.maxValue))
            shieldSliderFill.color = Color.Lerp(shieldSliderFill.color, shieldColor25, Time.deltaTime * colorChangeSpeed);
        if (shieldSlider.value <= (0.25f * shieldSlider.maxValue))
            shieldSliderFill.color = Color.Lerp(shieldSliderFill.color, shieldColor0, Time.deltaTime * colorChangeSpeed);
    }

    public void Death()
    {
        playerShipMovement.canControlShip = false;
        playerShipMovement.dead = true;
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        shipVisuals.gameObject.SetActive(false);
        deathParticles.Play();
        playerShipEffects.thrusterParticles.Stop();

        Invoke("Respawn", respawnTime);
    }

    private void Respawn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}