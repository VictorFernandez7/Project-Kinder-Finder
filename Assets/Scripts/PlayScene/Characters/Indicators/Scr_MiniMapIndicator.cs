using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_MiniMapIndicator : MonoBehaviour
{
    [Header("Choose Target")]
    [SerializeField] private Target target;

    [Header("References")]
    [SerializeField] private Scr_PlayerShipMovement playerShipMovement;
    [SerializeField] private SpriteRenderer spriteRenderer1;
    [SerializeField] private SpriteRenderer spriteRenderer2;

    private enum Target
    {
        Astronaut,
        PlayerShip
    }

    private void Update()
    {
        if (target == Target.Astronaut)
        {
            if (playerShipMovement.astronautOnBoard)
                spriteRenderer1.enabled = false;

            else
                spriteRenderer1.enabled = true;
        }

        else if (target == Target.PlayerShip)
        {
            if (playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.landed)
            {
                spriteRenderer1.enabled = true;
                spriteRenderer2.enabled = false;
            }

            else if (playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.takingOff)
            {
                spriteRenderer1.enabled = false;
                spriteRenderer2.enabled = true;
            }
        }
    }
}