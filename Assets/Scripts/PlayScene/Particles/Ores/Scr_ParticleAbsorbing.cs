using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_ParticleAbsorbing : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float absorbingForce;
    [SerializeField] private bool physics;

    [Header("References")]
    [SerializeField] private ParticleSystem desiredParticles;

    private int numberOfParticles;
    private Vector3[] directionArray;
    private ParticleSystem.Particle[] particleArray;

    private void Start()
    {
        particleArray = new ParticleSystem.Particle[desiredParticles.main.maxParticles];
        directionArray = new Vector3[desiredParticles.main.maxParticles];
    }

    private void Update()
    {
        numberOfParticles = desiredParticles.GetParticles(particleArray);

        for (int i = 0; i < numberOfParticles; i++)
        {
            directionArray[i] = (transform.localPosition - particleArray[i].position).normalized;

            if (physics)
                particleArray[i].velocity += directionArray[i] * absorbingForce * Time.deltaTime;

            else
                particleArray[i].position += directionArray[i] * absorbingForce * Time.deltaTime;
        }

        desiredParticles.SetParticles(particleArray, numberOfParticles);
    }
}