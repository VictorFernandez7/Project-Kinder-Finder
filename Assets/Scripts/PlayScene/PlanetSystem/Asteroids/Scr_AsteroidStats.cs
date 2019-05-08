﻿using System.Collections;
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
    [SerializeField] public float deathForce;
    [SerializeField] public float deathDamage;
    [SerializeField] public float powerRegenSpeed;

    [Header("Resources: Iron")]
    [SerializeField] public float ironAmount;
    [SerializeField] public GameObject resourceBlock;

    [Header("References")]
    [SerializeField] private Slider explosionSlider;
    [SerializeField] private Slider resourceSlider;
    [SerializeField] private Slider resistentSlider;
    [SerializeField] private Slider currentPowerSlider;
    [SerializeField] private TextMeshProUGUI explosionText;
    [SerializeField] private TextMeshProUGUI resourceText;
    [SerializeField] private ParticleSystem deathParticles;
    [SerializeField] private MeshRenderer asteroidVisuals;
    [SerializeField] private GameObject asteroidCanvas;
    [SerializeField] private GameObject asteroidCollider;
    [SerializeField] private TextMeshProUGUI resourceFeedback;
    [SerializeField] private GameObject playerShip;
    [SerializeField] private Scr_MainCamera mainCamera;

    [HideInInspector] public float currentPower;
    [HideInInspector] public bool mining;
    [HideInInspector] public bool dead;

    private float regenSpeed;
    private float explosionAmount;
    private float resourceAmount;
    private float newCurrentPower;
    private float initialResourceZone;
    private float initialColliderSize;
    private float resourceZoneMultiplier;
    private Vector3 initialScale;    
    private Scr_PlayerShipStats playerShipStats;
    private Scr_PlayerShipActions playerShipActions;
    private Scr_PlayerShipEffects playerShipEffects;
    private Scr_AsteroidBehaviour asteroidBehaviour;

    private void Start()
    {
        playerShipActions = playerShip.GetComponent<Scr_PlayerShipActions>();
        playerShipEffects = playerShip.GetComponent<Scr_PlayerShipEffects>();
        playerShipStats = playerShip.GetComponent<Scr_PlayerShipStats>();

        asteroidBehaviour = GetComponent<Scr_AsteroidBehaviour>();

        resourceAmount = 100;
        initialScale = asteroidVisuals.gameObject.transform.localScale;
        initialResourceZone = resourceZone;
        initialColliderSize = asteroidCollider.transform.localScale.x;
        resourceZoneMultiplier = (resourceZone - resistentZone) / 10;
    }

    private void Update()
    {
        if (!dead)
        {
            resourceZone = Mathf.Clamp(resourceZone, resistentZone, resourceZone);
            newCurrentPower = Mathf.Clamp(newCurrentPower, 0, 1);

            newCurrentPower = currentPower / 100;
            newCurrentPower = Mathf.Clamp(newCurrentPower, 0, 1);

            currentPowerSlider.value = newCurrentPower;
            currentPowerSlider.value = Mathf.Clamp(currentPowerSlider.value, 0, 1);

            SliderSet();
            SetCanvasPositionAndRotation();

            if (!mining && currentPowerSlider.value > 0)
            {
                regenSpeed = currentPowerSlider.value * powerRegenSpeed;
                currentPower -= regenSpeed * Time.deltaTime;
            }

            if (newCurrentPower >= resistentZone && newCurrentPower <= resourceZone)
                ResourceZone();

            if (newCurrentPower >= resourceZone)
                ExplosionZone();

            currentPower = Mathf.Clamp(currentPower, 0, 100);
            resourceFeedback.text = "" + (int)((resourceAmount / 100) * ironAmount);
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
        if (resourceAmount > 0.5f)
        {
            resourceAmount -= Time.deltaTime * extractigResourceSpeed;

            resourceText.text = "" + (int)resourceAmount + " %";

            if (asteroidVisuals.gameObject.transform.localScale.x >= 0.2f * initialScale.x)
            {
                asteroidVisuals.gameObject.transform.localScale = resourceAmount / 100 * initialScale;
                asteroidCollider.transform.localScale = Vector3.one * (resourceAmount / 100 * initialColliderSize);
            }

            if (resourceZone > (resistentZone + (0.3f * (initialResourceZone - resistentZone))))
                resourceZone = resistentZone + ((resourceAmount / 100) * (initialResourceZone - resistentZone));
        }

        else
            NoResources();
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
        Vector3 impulseDirection = new Vector3(playerShip.transform.position.x - transform.position.x, playerShip.transform.position.y - transform.position.y, playerShip.transform.position.z - transform.position.z);

        playerShipActions.MiningState(false);

        dead = true;
        asteroidBehaviour.move = false;
        asteroidVisuals.enabled = false;
        asteroidCanvas.SetActive(false);
        asteroidCollider.SetActive(false);

        if (!deathParticles.isPlaying)
            deathParticles.Play();

        if (playerShipEffects.miningParticles.isPlaying)
            playerShipEffects.miningParticles.Stop();

        playerShip.GetComponent<Rigidbody2D>().AddForce(impulseDirection * deathForce);
        playerShipStats.currentShield -= deathDamage;

        mainCamera.CameraShake(0.25f, 5, 12);

        Destroy(gameObject, 1.5f);
    }

    private void NoResources()
    {
        playerShipActions.MiningState(false);

        Destroy(gameObject, 0.5f);
    }

    private void SetCanvasPositionAndRotation()
    {
        asteroidCanvas.transform.position = transform.position + mainCamera.transform.up * 1.35f;
        asteroidCanvas.transform.up = mainCamera.transform.up;
    }
}