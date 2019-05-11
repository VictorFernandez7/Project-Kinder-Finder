using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_LiquidZone : MonoBehaviour
{
    [Header("Liquid Zone Type")]
    [SerializeField] public LiquidType liquidType;

    [Header("Resource Properties")]
    [SerializeField] public float amount;

    [Header("References")]
    [SerializeField] private Scr_ReferenceManager referenceManager;
    [SerializeField] private GameObject fuelVisuals;
    [SerializeField] private GameObject mercuryVisuals;
    [SerializeField] private GameObject termatiteVisuals;

    [HideInInspector] public GameObject currentResource;
    [HideInInspector] public float initialAmount;

    public enum LiquidType
    {
        Fuel,
        Mercury,
        Termatite
    }

    void Start()
    {
        initialAmount = amount;

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

    void Update()
    {
        CheckAmount();
    }

    private void CheckAmount()
    {
        if (amount <= 0 /*&& gasParticles.particleCount <= 0*/)
            Destroy(gameObject);
    }

    private void ChangeVisuals()
    {
        switch (liquidType)
        {
            case LiquidType.Fuel:
                fuelVisuals.SetActive(true);
                mercuryVisuals.SetActive(false);
                termatiteVisuals.SetActive(false);
                break;
            case LiquidType.Mercury:
                fuelVisuals.SetActive(false);
                mercuryVisuals.SetActive(true);
                termatiteVisuals.SetActive(false);
                break;
            case LiquidType.Termatite:
                fuelVisuals.SetActive(false);
                mercuryVisuals.SetActive(false);
                termatiteVisuals.SetActive(true);
                break;
        }
    }
}
