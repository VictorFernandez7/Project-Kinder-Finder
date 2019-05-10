using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_MiniMapIndicator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Scr_PlayerShipMovement playerShipMovement;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (playerShipMovement.currentPlanet != null)
            spriteRenderer.enabled = true;

        else
            spriteRenderer.enabled = false;
    }
}