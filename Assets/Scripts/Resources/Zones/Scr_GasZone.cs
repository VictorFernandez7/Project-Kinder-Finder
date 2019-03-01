﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_GasZone : MonoBehaviour
{
    [Header("Gas Zone Type")]
    [SerializeField] private GasType gasType;

    [Header("Resource Properties")]
    [SerializeField] public float amount;
    [SerializeField] public float zoneSize;

    [Header("Particle Properties")]
    [SerializeField] private float initialEmission;

    [Header("References")]
    [SerializeField] private Scr_ReferenceManager referenceManager;

    [HideInInspector] public GameObject currentResource;
    [HideInInspector] public float initialAmount;


    private ParticleSystem gasParticles;

    private enum GasType
    {
        Oxygen,
        Fuel,
        Helium,
        Aerogel
    }

    private void Start()
    {
        gasParticles = GetComponentInChildren<ParticleSystem>();

        initialAmount = amount;

        switch (gasType)
        {
            case GasType.Oxygen:
                currentResource = referenceManager.Resources[0];
                break;

            case GasType.Fuel:
                currentResource = referenceManager.Resources[1];
                break;

            case GasType.Helium:
                currentResource = referenceManager.Resources[4];
                break;

            case GasType.Aerogel:
                currentResource = referenceManager.Resources[5];
                break;
        }
    }

    private void Update ()
    {
        CheckAmount();
        ParticleAmount();
        GasZoneSize();
    }

    private void CheckAmount()
    {
        if (amount <= 0 && gasParticles.particleCount <= 0)
            Destroy(gameObject);
    }

    private void ParticleAmount()
    {
        var emission = gasParticles.emission;

        emission.rateOverTime = amount * (initialEmission / initialAmount);
    }

    private void GasZoneSize()
    {
        var shape = gasParticles.shape;

        GetComponent<CircleCollider2D>().radius = zoneSize;
        shape.radius = zoneSize * 20;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, zoneSize);
    }
}