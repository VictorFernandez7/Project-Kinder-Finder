using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_LiquidZone : MonoBehaviour
{
    [Header("Liquid Zone Type")]
    [SerializeField] public LiquidType liquidType;

    [Header("Resource Properties")]
    [SerializeField] public float amount;

    [Header("Particle Properties")]
    [SerializeField] private float initialEmission;

    [Header("References")]
    [SerializeField] private Scr_ReferenceManager referenceManager;
    [SerializeField] private GameObject fuelVisuals;
    [SerializeField] private GameObject mercuryVisuals;
    [SerializeField] private GameObject termatiteVisuals;

    [HideInInspector] public float initialAmount;
    [HideInInspector] public GameObject currentResource;
    [HideInInspector] public Scr_IAMovement iAMovement;

    private ParticleSystem liquidParticles;

    public enum LiquidType
    {
        Fuel,
        Mercury,
        Termatite
    }

    void Start()
    {
        initialAmount = amount;

        SetVisuals();
        SetResource();
    }

    void Update()
    {
        CheckAmount();
        ParticleAmount();
    }

    private void CheckAmount()
    {
        if (amount <= 0 && liquidParticles.particleCount <= 0)
        {
            iAMovement.isMining = false;
            Destroy(gameObject);
        }
    }

    private void SetVisuals()
    {
        switch (liquidType)
        {
            case LiquidType.Fuel:
                fuelVisuals.SetActive(true);
                mercuryVisuals.SetActive(false);
                termatiteVisuals.SetActive(false);
                liquidParticles = fuelVisuals.GetComponentInChildren<ParticleSystem>();
                break;
            case LiquidType.Mercury:
                fuelVisuals.SetActive(false);
                mercuryVisuals.SetActive(true);
                termatiteVisuals.SetActive(false);
                liquidParticles = mercuryVisuals.GetComponentInChildren<ParticleSystem>();
                break;
            case LiquidType.Termatite:
                fuelVisuals.SetActive(false);
                mercuryVisuals.SetActive(false);
                termatiteVisuals.SetActive(true);
                liquidParticles = termatiteVisuals.GetComponentInChildren<ParticleSystem>();
                break;
        }
    }

    private void SetResource()
    {
        switch (liquidType)
        {
            case LiquidType.Fuel:
                currentResource = referenceManager.Resources[1];
                break;

            case LiquidType.Mercury:
                currentResource = referenceManager.Resources[9];
                break;

            case LiquidType.Termatite:
                currentResource = referenceManager.Resources[11];
                break;
        }
    }

    private void ParticleAmount()
    {
        var emission = liquidParticles.emission;

        emission.rateOverTime = amount * (initialEmission / initialAmount);
    }
}
