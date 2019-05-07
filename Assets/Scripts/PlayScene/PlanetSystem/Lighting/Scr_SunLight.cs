using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_SunLight : MonoBehaviour
{
    [Header("Raycast Parameters")]
    [SerializeField] private LayerMask collisionMask;

    [Header("References")]
    [SerializeField] private GameObject playerShip;
    [SerializeField] private Transform astronautHead;

    [HideInInspector] public bool hitByLight;

    private void Update()
    {
        SunOrientation();
        LightCheck();
    }

    private void SunOrientation()
    {
        transform.LookAt(playerShip.transform);
    }

    private void LightCheck()
    {
        RaycastHit2D sunHit = Physics2D.Raycast(transform.position, astronautHead.transform.position - transform.position, Mathf.Infinity, collisionMask);

        if (sunHit)
        {
            Debug.DrawLine(transform.position, sunHit.point, Color.yellow);

            if (sunHit.transform.CompareTag("Astronaut"))
                hitByLight = true;

            else
                hitByLight = false;
        }

        else
            hitByLight = false;
    }
}