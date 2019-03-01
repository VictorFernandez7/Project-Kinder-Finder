using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_LiquidZone : MonoBehaviour
{
    [Header("Liquid Zone Type")]
    [SerializeField] private LiquidType liquidType;

    [Header("Resource Properties")]
    [SerializeField] public float amount;

    [Header("References")]
    [SerializeField] private Scr_ReferenceManager referenceManager;

    [HideInInspector] public GameObject currentResource;
    [HideInInspector] public float initialAmount;

    private enum LiquidType
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
}
