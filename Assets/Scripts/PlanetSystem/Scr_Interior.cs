﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Interior : MonoBehaviour
{
    [SerializeField] private Renderer[] walls;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Astronaut"))
        {
            for(int i = 0; i < walls.Length; i++)
            {
                walls[i].enabled = false;
            }
        }   
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Astronaut"))
        {
            for (int i = 0; i < walls.Length; i++)
            {
                walls[i].enabled = true;
            }
        }
    }
}