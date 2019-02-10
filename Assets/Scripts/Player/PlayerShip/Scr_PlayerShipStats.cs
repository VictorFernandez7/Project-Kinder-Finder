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
    [Range(0, 100)] [SerializeField] private float shieldAlertPercentage;
    [SerializeField] private Color shieldColor0;
    [SerializeField] private Color shieldColor25;
    [SerializeField] private Color shieldColor50;
    [SerializeField] private Color shieldColor75;

    [Header("Fuel Properties")]
    [SerializeField] public float currentFuel;
    [SerializeField] public float maxFuel;
    [Range(0, 100)] [SerializeField] private float fuelAlertPercentage;
    [SerializeField] private float normalConsume;
    [SerializeField] private float boostConsume;
    [SerializeField] private Color fuelColor0;
    [SerializeField] private Color fuelColor25;
    [SerializeField] private Color fuelColor50;
    [SerializeField] private Color fuelColor75;

    [Header("References")]
    [SerializeField] private ParticleSystem deathParticles;
    [SerializeField] private GameObject shipVisuals;
    [SerializeField] private BoxCollider2D collider;
    [SerializeField] private Animator fadeImage;
    [SerializeField] private Image fuelSliderFill;
    [SerializeField] private Image shieldSliderFill;
    [SerializeField] private Slider fuelSlider;
    [SerializeField] private Slider shieldSlider;
    [SerializeField] public Scr_ReferenceManager referenceManager;
    [SerializeField] public Animator anim_FuelPanel;
    [SerializeField] public Animator anim_ShieldPanel;
    [SerializeField] public Scr_LevelData levelData;

    [Header("Inventorys")]
    [SerializeField] public GameObject[] resourceWarehouse;

    [HideInInspector] public int lastWarehouseEmpty;
    [HideInInspector] public int experience;
    [HideInInspector] public int level;
    [HideInInspector] public bool gasExtractor;
    [HideInInspector] public bool repairingTool;
    [HideInInspector] public bool jetpack;
    [HideInInspector] public bool inDanger;

    private Rigidbody2D rb;
    private Scr_PlayerShipMovement playerShipMovement;
    private Scr_PlayerShipEffects playerShipEffects;

    private void Start()
    {
        playerShipMovement = GetComponent<Scr_PlayerShipMovement>();
        playerShipEffects = GetComponent<Scr_PlayerShipEffects>();
        rb = GetComponent<Rigidbody2D>();

        fuelSlider.maxValue = maxFuel;
        shieldSlider.maxValue = maxShield;
    }

    private void Update()
    {
        Fuel();
        Shield();
    }

    private void Fuel()
    {
        FuelSliderColor();

        currentFuel = Mathf.Clamp(currentFuel, 0f, maxFuel);
        fuelSlider.value = currentFuel;

        if (currentFuel <= ((fuelAlertPercentage / 100) * maxFuel))
            anim_FuelPanel.SetBool("Alert", true);

        else
            anim_FuelPanel.SetBool("Alert", false);
    }

    private void Shield()
    {
        ShieldSliderColor();

        currentShield = Mathf.Clamp(currentShield, 0f, maxShield);
        shieldSlider.value = currentShield;

        if (currentShield <= (0.1f * maxShield))
            playerShipMovement.damaged = true;

        else
            playerShipMovement.damaged = false;


        if (currentShield <= ((shieldAlertPercentage / 100) * maxShield))
        {
            anim_ShieldPanel.SetBool("Alert", true);

            if (currentShield < 0)
                Death();
        }

        else
            anim_ShieldPanel.SetBool("Alert", false);

        if (currentShield <= (0.25 * maxShield))
            playerShipEffects.DamageParticleSet(true);

        else
            playerShipEffects.DamageParticleSet(false);
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
        playerShipMovement.canRotateShip = false;
        playerShipMovement.dead = true;
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        shipVisuals.gameObject.SetActive(false);
        deathParticles.Play();
        playerShipEffects.thrusterParticles.Stop();
        collider.enabled = false;
        fadeImage.SetBool("Fade", false);
        GetComponentInChildren<Scr_PlayerShipDeathCheck>().enabled = false;
        GetComponent<Scr_PlayerShipPrediction>().enabled = false;

        Invoke("Respawn", respawnTime);
    }

    private void Respawn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GetExperience (int amount)
    {
        experience += amount;

        if(levelData.LevelList[level - 1].experienceNeeded < experience)
        {
            level += 1;
            //animacion de subida de nivel
            //activar los rewards del nivel
        }
    }
}