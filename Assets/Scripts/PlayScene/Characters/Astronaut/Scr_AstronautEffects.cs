using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_AstronautEffects : MonoBehaviour
{
    [Header("Particle References")]
    [SerializeField] private ParticleSystem movingParticles;
    [SerializeField] private ParticleSystem jumpParticles;
    [SerializeField] private ParticleSystem fallParticles;
    [SerializeField] private ParticleSystem normalDeathParticles;
    [SerializeField] private ParticleSystem fireDeathParticles;
    [SerializeField] private ParticleSystem IceDeathParticles;
    [SerializeField] private ParticleSystem poisonDeathParticles;

    [Header("Sound References")]
    [SerializeField] private SoundDefinition steps;
    [SerializeField] private SoundDefinition jump;
    [SerializeField] private SoundDefinition mine;
    [SerializeField] private SoundDefinition stepOutOfShip;
    [SerializeField] private SoundDefinition repair;
    [SerializeField] private SoundDefinition breathing;
    [SerializeField] private SoundDefinition chatter;

    [Header("Other References")]
    [SerializeField] private Scr_MusicManager musicManager;
    [SerializeField] private Scr_PlayerShipMovement playerShipMovement;

    [HideInInspector] public bool breathingBool;

    private Scr_AstronautMovement astronautMovement;

    public enum DeathType
    {
        Normal,
        Fire,
        Ice,
        Posion
    }

    private void Start()
    {
        astronautMovement = GetComponent<Scr_AstronautMovement>();

        breathingBool = true;
    }

    private void Update()
    {
        SoundManager();
        MovingParticles();
    }

    private void MovingParticles()
    {
        if (movingParticles.isPlaying && astronautMovement.jumping)
            movingParticles.Stop();

        else
            PlayParticleSystem(movingParticles);
    }

    public void JumpParticles()
    {
        PlayParticleSystem(jumpParticles);
    }

    public void FallParticles()
    {
        PlayParticleSystem(fallParticles);
    }

    public void DeathParticles(DeathType deathType)
    {
        switch (deathType)
        {
            case DeathType.Normal:
                PlayParticleSystem(normalDeathParticles);
                break;
            case DeathType.Fire:
                PlayParticleSystem(fireDeathParticles);
                break;
            case DeathType.Ice:
                PlayParticleSystem(IceDeathParticles);
                break;
            case DeathType.Posion:
                PlayParticleSystem(poisonDeathParticles);
                break;
        }
    }

    private void PlayParticleSystem(ParticleSystem desiredParticles)
    {
        if (!desiredParticles.isPlaying)
            desiredParticles.Play();
    }

    private void SoundManager()
    {/*
        if (!musicManager.transform.GetChild(2).GetComponent<AudioSource>().isPlaying)
        {
            Scr_MusicManager.Instance.PlaySound(steps.Sound, 0);
        }

        else if ( musicManager.transform.GetChild(2).GetComponent<AudioSource>().isPlaying)
        {
            //Scr_MusicManager.Instance.StopSound(Scr_MusicManager.SoundType.LOOP_SOUNDS);
        }

        /*
        if (breathingBool && !musicManager.transform.GetChild(2).GetComponent<AudioSource>().isPlaying)
        {
            Scr_MusicManager.Instance.PlaySound(jump.Sound, 0);
        }

        else if (!breathing && musicManager.transform.GetChild(2).GetComponent<AudioSource>().isPlaying)
        {
            Scr_MusicManager.Instance.StopSound(Scr_MusicManager.SoundType.LOOP_SOUNDS);
        }

        
        if ()
        {
           Scr_MusicManager.Instance.PlaySound(stepOutOfShip.Sound, 0);
        }

        if ()
        {
           Scr_MusicManager.Instance.PlaySound(repair.Sound, 0);
        }

        if (breathingBool && !musicManager.transform.GetChild(2).GetComponent<AudioSource>().isPlaying)
        {

        }

        else if (!breathingBool && musicManager.transform.GetChild(2).GetComponent<AudioSource>().isPlaying)
        {

        }
        
       if ()
       {
           Scr_MusicManager.Instance.PlaySound(chatter.Sound, 0);
       }*/
    }
}