using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Breakeable : MonoBehaviour
{
    [Header("Door Parameters")]
    [SerializeField] public float amount;

    [Header("References")]
    [SerializeField] private Scr_MainCamera mainCamera;

    private bool playedOnce;
    private GameObject canvas;
    private ParticleSystem explosionParticles;
    private BoxCollider2D boxCollider;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        explosionParticles = GetComponentInChildren<ParticleSystem>();
        canvas = GetComponentInChildren<Canvas>().gameObject;
    }

    private void Update()
    {
        if (amount <= 0)
        {
            if (!explosionParticles.isPlaying && !playedOnce)
            {
                explosionParticles.Play();
                mainCamera.CameraShake(0.25f, 5, 2);
                canvas.SetActive(false);
                boxCollider.enabled = false;

                playedOnce = true;

                Destroy(gameObject, 2.5f);
            }
        }
    }
}
