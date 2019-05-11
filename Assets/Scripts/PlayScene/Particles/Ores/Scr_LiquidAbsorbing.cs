using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_LiquidAbsorbing : MonoBehaviour
{
    [Header("Absorbing Parameters")]
    [SerializeField] public float force;
    [SerializeField] private bool physics;

    private int numberOfParticles;
    private Vector3[] directionArray;
    private ParticleSystem desiredParticles;
    private ParticleSystem.Particle[] particleArray;

    private void Start()
    {
        desiredParticles = GetComponentInParent<ParticleSystem>();

        particleArray = new ParticleSystem.Particle[desiredParticles.main.maxParticles];
        directionArray = new Vector3[desiredParticles.main.maxParticles];
    }

    private void Update()
    {
        numberOfParticles = desiredParticles.GetParticles(particleArray);
    }

    public void AbsorbParticles()
    {
        var main = desiredParticles.main;

        main.startSpeed = 0.5f;
        desiredParticles.GetComponentInChildren<Scr_GravityForParticles>().enabled = false;

        for (int i = 0; i < numberOfParticles; i++)
        {
            directionArray[i] = (transform.localPosition - particleArray[i].position).normalized;

            if (physics)
                particleArray[i].velocity += directionArray[i] * force * Time.deltaTime;

            else
                particleArray[i].position += directionArray[i] * force * Time.deltaTime;
        }

        desiredParticles.SetParticles(particleArray, numberOfParticles);
    }

    public void StopAbsorbing()
    {
        var main = desiredParticles.main;

        main.startSpeed = 1f;
        desiredParticles.GetComponentInChildren<Scr_GravityForParticles>().enabled = true;
    }
}