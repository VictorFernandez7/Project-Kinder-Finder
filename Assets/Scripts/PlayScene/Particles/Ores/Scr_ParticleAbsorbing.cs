using UnityEngine;

public class Scr_ParticleAbsorbing : MonoBehaviour
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
}