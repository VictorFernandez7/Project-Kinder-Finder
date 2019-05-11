using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_PlayerShipHalo : MonoBehaviour
{
    [Header("Halo Parameters")]
    [SerializeField] private float radius;
    [SerializeField] private float width;

    [Header("Color Parameters")]
    [SerializeField] private Color inSpace;
    [SerializeField] private Color inPlanet;
    [SerializeField] private float lerpSpeed;

    [Header("Delays")]
    [SerializeField] private float takingOffDelay;

    [Header("References")]
    [SerializeField] private Transform playership;

    private float savedTimer;
    private Color targetColor;
    private LineRenderer lineRenderer;
    private Scr_PlayerShipMovement playerShipMovement;

    private void Start()
    {
        playerShipMovement = playership.GetComponent<Scr_PlayerShipMovement>();
        lineRenderer = GetComponent<LineRenderer>();

        savedTimer = takingOffDelay;
    }

    void Update()
    {
        HaloPoints();
        HaloProperties();
        HaloAlphaControl();
    }

    private void HaloPoints()
    {
        int index = 0;

        lineRenderer.positionCount = 41;

        for (float i = 1; i >= 0; i -= 0.1f)
        {
            Vector3 vectorDirector = new Vector3(i, 1 - i, 0);
            lineRenderer.SetPosition(index, (vectorDirector.normalized * radius) + playership.position);
            index += 1;
        }

        for (float i = 0; i >= -1; i -= 0.1f)
        {
            Vector3 vectorDirector = new Vector3(i, 1 + i, 0);
            lineRenderer.SetPosition(index, (vectorDirector.normalized * radius) + playership.position);
            index += 1;
        }

        for (float i = -1; i <= 0; i += 0.1f)
        {
            Vector3 vectorDirector = new Vector3(i, -1 - i, 0);
            lineRenderer.SetPosition(index, (vectorDirector.normalized * radius) + playership.position);
            index += 1;
        }

        for (float i = 0; i <= 1; i += 0.1f)
        {
            Vector3 vectorDirector = new Vector3(i, -1 + i, 0);
            lineRenderer.SetPosition(index, (vectorDirector.normalized * radius) + playership.position);
            index += 1;
        }

        lineRenderer.SetPosition(40, (new Vector3(1, 0, 0) * radius) + playership.position);
    }

    private void HaloProperties()
    {
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
    }

    private void HaloAlphaControl()
    {
        if (playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.landed)
        {
            targetColor = inPlanet;
            savedTimer = takingOffDelay;
        }

        if (playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.landing)
            targetColor = inPlanet;

        if (playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.takingOff || playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.inSpace)
        {
            savedTimer -= Time.deltaTime;

            if (savedTimer <= 0)
                targetColor = inSpace;
        }

        lineRenderer.startColor = Color.Lerp(lineRenderer.startColor, targetColor, Time.deltaTime * lerpSpeed);
        lineRenderer.endColor = Color.Lerp(lineRenderer.endColor, targetColor, Time.deltaTime * lerpSpeed);
    }
}