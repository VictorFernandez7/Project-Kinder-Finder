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

    [Header("Resource References")]
    [SerializeField] private GameObject fuelResource;

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
        GasZoneSize();
    }

    private void CheckAmount()
    {
        if (amount <= 0 || Input.GetKey(KeyCode.Z))
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
        shape.radius = zoneSize * 10;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, zoneSize);
    }
}