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

    [Header("Speed Parameters")]
    [SerializeField] private float takingOffSpeed;
    [SerializeField] private float landingSpeed;

    [Header("Delays")]
    [SerializeField] private float takingOffDelay;
    [SerializeField] private float disablingDelay;
    [SerializeField] private float activateDelay;

    [Header("References")]
    [SerializeField] private Transform playership;

    private bool lerping;
    private float takingOffDelaySaved;
    private float disablingDelaySaved;
    private float activateDelaySaved;
    private float targetSpeed;
    private Color targetColor;
    private LineRenderer lineRenderer;
    private Scr_PlayerShipMovement playerShipMovement;

    private void Start()
    {
        playerShipMovement = playership.GetComponent<Scr_PlayerShipMovement>();
        lineRenderer = GetComponent<LineRenderer>();

        takingOffDelaySaved = takingOffDelay;
        disablingDelaySaved = disablingDelay;
        activateDelaySaved = activateDelay;
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
            takingOffDelaySaved = takingOffDelay;
            activateDelaySaved = activateDelay;
        }

        if (playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.landing)
        {
            lerping = true;

            targetColor = inPlanet;
            targetSpeed = landingSpeed;
            disablingDelaySaved -= Time.deltaTime;

            if (disablingDelaySaved <= 0)
                lerping = false;
        }

        if (playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.takingOff || playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.inSpace)
        {
            lerping = true;

            disablingDelaySaved = disablingDelay;
            takingOffDelaySaved -= Time.deltaTime;

            if (takingOffDelaySaved <= 0)
            {
                targetSpeed = takingOffSpeed;
                targetColor = inSpace;

                activateDelaySaved -= Time.deltaTime;

                if (activateDelaySaved <= 0)
                    lerping = false;
            }
        }

        if (lerping)
        {
            lineRenderer.startColor = Color.Lerp(lineRenderer.startColor, targetColor, Time.deltaTime * targetSpeed);
            lineRenderer.endColor = Color.Lerp(lineRenderer.endColor, targetColor, Time.deltaTime * targetSpeed);
        }

        else
        {
            lineRenderer.startColor = targetColor;
            lineRenderer.endColor = targetColor;
        }
    }
}