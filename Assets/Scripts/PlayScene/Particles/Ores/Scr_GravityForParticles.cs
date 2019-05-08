using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_GravityForParticles : MonoBehaviour
{
    [Header("Particle Parameters")]
    [SerializeField] private float gravityAmount;

    private int numberOfParticles;
    private Vector3 desiredDirection;
    private ParticleSystem desiredParticles;
    private ParticleSystem.Particle[] particleArray;

    private void Start()
    {
        desiredParticles = GetComponent<ParticleSystem>();
        particleArray = new ParticleSystem.Particle[desiredParticles.main.maxParticles];

        desiredDirection = -transform.up;
    }

    private void Update()
    {
        numberOfParticles = desiredParticles.GetParticles(particleArray);

        for (int i = 0; i < numberOfParticles; i++)
        {
            particleArray[i].velocity += desiredDirection * gravityAmount * Time.deltaTime;
        }

        desiredParticles.SetParticles(particleArray, numberOfParticles);
    }
}