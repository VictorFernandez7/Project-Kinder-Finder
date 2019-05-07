﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_BreakeableDetection : MonoBehaviour
{
    [SerializeField] private Scr_AstronautsActions astronautsActions;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Astronaut"))
        {
            astronautsActions.miningSpot = this.gameObject;
            astronautsActions.spotType = Scr_AstronautsActions.SpotType.breakeable;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Astronaut"))
            astronautsActions.miningSpot = null;
    }
}