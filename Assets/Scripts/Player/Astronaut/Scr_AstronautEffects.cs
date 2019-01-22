using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_AstronautEffects : MonoBehaviour
{

    //lo renombré con SFX pero daba fallo el el script de movimiento.
    [Header("References")]
    [SerializeField] private Scr_AstronautMovement astronautMovement;
    [SerializeField] private Scr_MusicManager musicManager;

    [Header("Sounds")]
    [SerializeField] private SoundDefinition steps;
    [SerializeField] private SoundDefinition jump;
    [SerializeField] private SoundDefinition mine;
    [SerializeField] private SoundDefinition stepOutOfShip;
    [SerializeField] private SoundDefinition repair;
    [SerializeField] private SoundDefinition breathing;
    [SerializeField] private SoundDefinition chatter;

    [HideInInspector] public bool breathingBool;

    private void Start()
    {
        breathingBool = true;
    }

    private void Update()
    {

        if (astronautMovement.walking && !musicManager.transform.GetChild(2).GetComponent<AudioSource>().isPlaying)
        {
            Scr_MusicManager.Instance.PlaySound(steps.Sound, 0);
        }

        else if (!astronautMovement.walking && musicManager.transform.GetChild(2).GetComponent<AudioSource>().isPlaying)
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
        }*/

        if (breathingBool && !musicManager.transform.GetChild(2).GetComponent<AudioSource>().isPlaying)
        {
           Scr_MusicManager.Instance.PlaySound(breathing.Sound, 0);
        }

        else if (!breathingBool && musicManager.transform.GetChild(2).GetComponent<AudioSource>().isPlaying)
        {
           Scr_MusicManager.Instance.StopSound(Scr_MusicManager.SoundType.LOOP_SOUNDS);
        }
        /*
       if ()
       {
           Scr_MusicManager.Instance.PlaySound(chatter.Sound, 0);
       } */
    }
}