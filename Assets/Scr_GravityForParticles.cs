using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_GravityForParticles : MonoBehaviour
{
    [Header("Gravity Parameters")]
    [SerializeField] private float gravityAmount;

    [Header("References")]
    [SerializeField] private Scr_PlayerShipMovement playerShipMovement;

    private int numberOfParticles;
    private Vector3 desiredDirection;
    private ParticleSystem liquidParticles;
    private ParticleSystem.Particle[] currentParticles;

    private void Start()
    {
        liquidParticles = GetComponent<ParticleSystem>();
        currentParticles = new ParticleSystem.Particle[liquidParticles.main.maxParticles];
    }

    private void Update()
    {
        numberOfParticles = liquidParticles.GetParticles(currentParticles);
        if(playerShipMovement.currentPlanet != null)
            desiredDirection = playerShipMovement.currentPlanet.transform.position - transform.position;

        for (int i = 0; i < numberOfParticles; i++)
        {
            currentParticles[i].velocity += desiredDirection * gravityAmount * Time.deltaTime;
        }

        liquidParticles.SetParticles(currentParticles, numberOfParticles);
    }
}