using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Scr_AsteroidStats : MonoBehaviour
{
    [Header("Mining System")]
    [Range(0, 1)] [SerializeField] public float explosionZone;
    [Range(0, 1)] [SerializeField] public float resourceZone;
    [Range(0, 1)] [SerializeField] public float resistentZone;
    [SerializeField] public float extractigResourceSpeed;
    [SerializeField] public float deathSpeed;
    [SerializeField] public float powerRegenSpeed;

    [Header("Resources: Steel")]
    [SerializeField] public float steelAmount;
    [SerializeField] public GameObject steelBlock;

    [Header("References")]
    [SerializeField] private Slider explosionSlider;
    [SerializeField] private Slider resourceSlider;
    [SerializeField] private Slider resistentSlider;
    [SerializeField] private Slider currentPowerSlider;
    [SerializeField] private TextMeshProUGUI explosionText;
    [SerializeField] private TextMeshProUGUI resourceText;
    [SerializeField] private ParticleSystem deathParticles;
    [SerializeField] private GameObject asteroidVisuals;
    [SerializeField] private GameObject asteroidCanvas;

    [HideInInspector] public float currentPower;
    [HideInInspector] public bool mining;
    [HideInInspector] public bool dead;

    private float regenSpeed;
    private float explosionAmount;
    private float resourceAmount;
    private float newCurrentPower;
    private float resourceZoneMultiplier;
    private Scr_PlayerShipActions playerShipActions;
    private Scr_PlayerShipEffects playerShipEffects;
    private Scr_AsteroidBehaviour asteroidBehaviour;

    private void Start()
    {
        playerShipActions = GameObject.Find("PlayerShip").GetComponent<Scr_PlayerShipActions>();
        playerShipEffects = GameObject.Find("PlayerShip").GetComponent<Scr_PlayerShipEffects>();

        asteroidBehaviour = GetComponent<Scr_AsteroidBehaviour>();

        resourceAmount = 100;
        resourceZoneMultiplier = (resourceZone - resistentZone) / 10;
    }

    private void Update()
    {
        if (!dead)
        {
            SliderSet();

            resourceZone = Mathf.Clamp(resourceZone, resistentZone, resourceZone);
            newCurrentPower = Mathf.Clamp(newCurrentPower, 0, 1);

            newCurrentPower = currentPower / 100;
            currentPowerSlider.value = newCurrentPower;

            if (!mining && currentPowerSlider.value > 0)
            {
                regenSpeed = currentPowerSlider.value * powerRegenSpeed;
                currentPower -= regenSpeed * Time.deltaTime;
            }

            if (newCurrentPower >= resistentZone && newCurrentPower <= resourceZone)
                ResourceZone();

            if (newCurrentPower >= resourceZone)
                ExplosionZone();
        }
    }

    private void SliderSet()
    {
        explosionSlider.value = explosionZone;
        resourceSlider.value = resourceZone;
        resistentSlider.value = resistentZone;
    }

    private void ResourceZone()
    {
        if (resourceAmount > 0)
        {
            resourceAmount -= Time.deltaTime * extractigResourceSpeed;

            resourceZone -= resourceZoneMultiplier * Time.deltaTime;

            resourceText.text = "" + (int)resourceAmount + " %";
        }
    }

    private void ExplosionZone()
    {
        explosionAmount += Time.deltaTime * deathSpeed;

        explosionText.text = "" + (int)explosionAmount + " %";

        if (explosionAmount >= 100)
            Death();
    }

    private void Death()
    {
        playerShipActions.MiningState(false);

        dead = true;
        asteroidBehaviour.move = false;
        asteroidVisuals.SetActive(false);
        asteroidCanvas.SetActive(false);
        GetComponent<CircleCollider2D>().enabled = false;

        if (!deathParticles.isPlaying)
            deathParticles.Play();

        if (playerShipEffects.miningParticles.isPlaying)
            playerShipEffects.miningParticles.Stop();

        Destroy(gameObject, 1.5f);
    }

}