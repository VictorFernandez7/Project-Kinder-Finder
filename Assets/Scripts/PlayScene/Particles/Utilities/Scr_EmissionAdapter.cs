using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_EmissionAdapter : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float ratio;

    [Header("References")]
    [SerializeField] private ParticleSystem targetParticles;

    private ParticleSystem currentParticles;

    private void Start()
    {
        currentParticles = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        var targetEmission = targetParticles.emission;
        var currentEmission = currentParticles.emission;

        currentEmission.rateOverTime = targetEmission.rateOverTime.constant * ratio;
    }
}