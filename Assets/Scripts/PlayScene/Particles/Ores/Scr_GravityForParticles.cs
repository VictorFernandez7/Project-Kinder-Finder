using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_GravityForParticles : MonoBehaviour
{
    [Header("Select Desired Transform")]
    [SerializeField] private TransformSelection transformSelection;

    [Header("Particle Parameters")]
    [SerializeField] private float gravityAmount;

    private int numberOfParticles;
    private Vector3 desiredDirection;
    private ParticleSystem desiredParticles;
    private ParticleSystem.Particle[] particleArray;

    private enum TransformSelection
    {
        Up,
        Down,
        Right,
        Left,
        Forward,
        Backward
    }

    private void Start()
    {
        desiredParticles = GetComponent<ParticleSystem>();
        particleArray = new ParticleSystem.Particle[desiredParticles.main.maxParticles];
    }

    private void Update()
    {
        SelectTransform();

        numberOfParticles = desiredParticles.GetParticles(particleArray);

        for (int i = 0; i < numberOfParticles; i++)
        {
            particleArray[i].velocity += desiredDirection * gravityAmount * Time.deltaTime;
        }

        desiredParticles.SetParticles(particleArray, numberOfParticles);
    }

    private void SelectTransform()
    {
        switch (transformSelection)
        {
            case TransformSelection.Up:
                desiredDirection = -transform.up;
                break;
            case TransformSelection.Down:
                desiredDirection = transform.up;
                break;
            case TransformSelection.Right:
                desiredDirection = transform.right;
                break;
            case TransformSelection.Left:
                desiredDirection = -transform.right;
                break;
            case TransformSelection.Forward:
                desiredDirection = transform.forward;
                break;
            case TransformSelection.Backward:
                desiredDirection = -transform.forward;
                break;
        }
    }
}