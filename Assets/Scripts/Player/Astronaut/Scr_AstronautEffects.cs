using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_AstronautEffects : MonoBehaviour
{

    //lo renombré con SFX pero daba fallo el el script de movimiento.
    [Header("References")]
    [SerializeField] private Scr_AstronautMovement astronautMovement;

    [Header("Sounds")]
    [SerializeField] private SoundDefinition steps;
    [SerializeField] private SoundDefinition jump;
    [SerializeField] private SoundDefinition mine;
    [SerializeField] private SoundDefinition stepOutOfShip;
    [SerializeField] private SoundDefinition repair;

   /* void update()
    
    {
        if ()
        { 
        Scr_MusicManager.Instance.PlaySound(steps.Sound, 0);
        }
        if ()
        {
            Scr_MusicManager.Instance.PlaySound(jump.Sound, 0);
        }
        if ()
        {
            Scr_MusicManager.Instance.PlaySound(stepOutOfShip.Sound, 0);
        }
        if ()
        {
            Scr_MusicManager.Instance.PlaySound(repair.Sound, 0);
        }
    }
    */
}