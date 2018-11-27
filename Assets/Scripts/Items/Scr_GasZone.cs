using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_GasZone : MonoBehaviour
{
    [Header("Gas Zone Type")]
    [SerializeField] private GasType gasType;

    [Header("Resource Properties")]
    [SerializeField] public float amount;

    [Header("Particle Properties")]
    [SerializeField] private float initialEmission;

    [Header("Resource References")]
    [SerializeField] public GameObject fuelResource;

    [HideInInspector] public GameObject currentResource;

    private float initialAmount;
    private ParticleSystem gasParticles;

    private enum GasType
    {
        fuel
    }

    private void Start()
    {
        gasParticles = GetComponentInChildren<ParticleSystem>();

        initialAmount = amount;

        switch (gasType)
        {
            case GasType.fuel:
                currentResource = fuelResource;
                break;
        }
    }

    private void Update ()
    {
        CheckAmount();
        ParticleAmount();
    }

    private void CheckAmount()
    {
        if (amount <= 0)
            Destroy(gameObject);
    }

    private void ParticleAmount()
    {
        var emission = gasParticles.emission;

        emission.rateOverTime = amount * (initialEmission / initialAmount);
    }
}