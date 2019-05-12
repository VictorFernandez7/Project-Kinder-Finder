using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_GasZone : MonoBehaviour
{
    [Header("Gas Zone Type")]
    [SerializeField] public GasType gasType;

    [Header("Resource Properties")]
    [SerializeField] public float amount;
    [SerializeField] public float partResource;
    [SerializeField] public float zoneSize;

    [Header("Particle Properties")]
    [SerializeField] private float initialEmission;

    [Header("References")]
    [SerializeField] private Scr_ReferenceManager referenceManager;
    [SerializeField] private GameObject oxygenVisuals;
    [SerializeField] private GameObject fuelVisuals;
    [SerializeField] private GameObject heliumVisuals;
    [SerializeField] private GameObject aerogelVisuals;

    [HideInInspector] public float initialAmount;
    [HideInInspector] public GameObject currentResource;
    [HideInInspector] public Scr_IAMovement iAMovement;

    private ParticleSystem gasParticles;
    private int realAmount;
    private float savedEmission;

    public enum GasType
    {
        Oxygen,
        Fuel,
        Helium,
        Aerogel
    }

    private void Start()
    {
        initialAmount = amount;
        realAmount = 0;
        savedEmission = initialEmission;

        ChangeVisuals();

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
        GasZoneSize();
        ParticleAmount();
    }

    private void CheckAmount()
    {
        if (amount <= 0 && gasParticles.particleCount <= 0) {
            iAMovement.isMining = false;

            if(gasType == GasType.Fuel)
            {
                GameObject newGasZone = Instantiate(referenceManager.Zones[2], transform.position, transform.rotation, transform.parent);
                newGasZone.SetActive(false);
                newGasZone.GetComponent<Scr_GasZone>().amount = initialAmount;
                newGasZone.GetComponent<Scr_GasZone>().initialEmission = savedEmission;
                newGasZone.GetComponent<Scr_GasZone>().referenceManager = referenceManager;
                newGasZone.GetComponent<Scr_GasZone>().gasType = GasType.Fuel;
                newGasZone.GetComponentInChildren<Scr_GasDetection>().astronautsActions = GetComponentInChildren<Scr_GasDetection>().astronautsActions;
                referenceManager.respawnFuelResources.Add(newGasZone);
            }

            Destroy(gameObject);
        }

        else if (partResource >= 1 && realAmount != initialAmount)
        {
            iAMovement.gameObject.GetComponentInChildren<Scr_GasTool>().GenerateResource();
            partResource = 0;
            realAmount += 1;
        }
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
        shape.radius = zoneSize * 2;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, zoneSize);
    }

    private void ChangeVisuals()
    {
        switch (gasType)
        {
            case GasType.Oxygen:
                oxygenVisuals.SetActive(true);
                fuelVisuals.SetActive(false);
                heliumVisuals.SetActive(false);
                aerogelVisuals.SetActive(false);
                gasParticles = oxygenVisuals.GetComponentInChildren<ParticleSystem>();
                break;
            case GasType.Fuel:
                oxygenVisuals.SetActive(false);
                fuelVisuals.SetActive(true);
                heliumVisuals.SetActive(false);
                aerogelVisuals.SetActive(false);
                gasParticles = fuelVisuals.GetComponentInChildren<ParticleSystem>();
                break;
            case GasType.Helium:
                oxygenVisuals.SetActive(false);
                fuelVisuals.SetActive(false);
                heliumVisuals.SetActive(true);
                aerogelVisuals.SetActive(false);
                gasParticles = heliumVisuals.GetComponentInChildren<ParticleSystem>();
                break;
            case GasType.Aerogel:
                oxygenVisuals.SetActive(false);
                fuelVisuals.SetActive(false);
                heliumVisuals.SetActive(false);
                aerogelVisuals.SetActive(true);
                gasParticles = aerogelVisuals.GetComponentInChildren<ParticleSystem>();
                break;
        }
    }
}