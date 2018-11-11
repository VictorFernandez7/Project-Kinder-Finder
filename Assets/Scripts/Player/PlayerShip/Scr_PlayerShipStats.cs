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
    [SerializeField] SpriteRenderer shipVisuals;
    [SerializeField] SpriteRenderer mapVisuals;
    [SerializeField] TrailRenderer trailRenderer;

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
    }

    private void Update()
    {
        currentFuel = Mathf.Clamp(currentFuel, 0f, maxFuel);
        fuelSlider.value = currentFuel;

        FuelSliderColor();
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
        mapVisuals.gameObject.SetActive(false);
        trailRenderer.enabled = false;
        deathParticles.Play();
        playerShipMovement.thrusterParticles.Stop();

        Invoke("Respawn", respawnTime);
    }

    private void Respawn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}