using System.Collections;
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

    [HideInInspector] public GameObject currentResource;

    private float initialAmount;
    private ParticleSystem gasParticles;
    private Scr_ReferenceManager referenceManager;

    private enum GasType
    {
        fuel
    }

    private void Start()
    {
        referenceManager = GameObject.Find("ReferenceManager").GetComponent<Scr_ReferenceManager>();

        gasParticles = GetComponentInChildren<ParticleSystem>();

        initialAmount = amount;

        switch (gasType)
        {
            case GasType.fuel:
                currentResource = referenceManager.GasResources[0];
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