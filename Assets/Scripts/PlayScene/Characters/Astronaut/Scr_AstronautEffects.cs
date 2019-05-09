﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_AstronautEffects : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ParticleSystem movingParticles;
    [SerializeField] private ParticleSystem jumpParticles;
    [SerializeField] private Scr_MusicManager musicManager;
    [SerializeField] private Scr_PlayerShipMovement playerShipMovement;

    [Header("Sounds")]
    [SerializeField] private SoundDefinition steps;
    [SerializeField] private SoundDefinition jump;
    [SerializeField] private SoundDefinition mine;
    [SerializeField] private SoundDefinition stepOutOfShip;
    [SerializeField] private SoundDefinition repair;
    [SerializeField] private SoundDefinition breathing;
    [SerializeField] private SoundDefinition chatter;

    [HideInInspector] public bool breathingBool;

    private Scr_AstronautMovement astronautMovement;

    private void Start()
    {
        astronautMovement = GetComponent<Scr_AstronautMovement>();

        breathingBool = true;
        movingParticles.Play();
    }

    private void Update()
    {
        SoundManager();

        if (movingParticles.isPlaying && astronautMovement.jumping)
            movingParticles.Stop();

        else if (!movingParticles.isPlaying)
            movingParticles.Play();
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

    public void JumpParticles()
    {
        if (!jumpParticles.isPlaying)
            jumpParticles.Play();
    }
}