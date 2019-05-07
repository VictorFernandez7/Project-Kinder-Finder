using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_PlayerShipHalo : MonoBehaviour
{
    [Header("Halo Sttings")]
    [SerializeField] private float radius;
    [SerializeField] private float width;
    [SerializeField] private float colorLerpSpeed;
    [SerializeField] private float colorChangeDelay;
    [SerializeField] private Color activeHalo;
    [SerializeField] private Color notActiveHalo;

    [Header("References")]
    [SerializeField] private Transform playership;
    [SerializeField] private Scr_PlayerShipMovement playerShipMovement;

    private float initialTimerValue;
    private LineRenderer haloLine;

    private void Start()
    {
        haloLine = GetComponent<LineRenderer>();
        initialTimerValue = colorChangeDelay;
    }

    void Update()
    {
        HaloColor();
        HaloPoints();
        HaloProperties();
    }

    private void HaloColor()
    {
        if (playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.landed)
        {
            haloLine.startColor = notActiveHalo;
            haloLine.endColor = notActiveHalo;

            colorChangeDelay = initialTimerValue;
        }

        if (playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.landing)
        {
            haloLine.startColor = Color.Lerp(haloLine.startColor, notActiveHalo, Time.deltaTime * colorLerpSpeed);
            haloLine.endColor = Color.Lerp(haloLine.startColor, notActiveHalo, Time.deltaTime * colorLerpSpeed);
        }

        if (playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.takingOff || playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.inSpace)
        {
            colorChangeDelay -= Time.deltaTime;

            if (colorChangeDelay <= 0)
            {
                haloLine.startColor = Color.Lerp(haloLine.startColor, activeHalo, Time.deltaTime * colorLerpSpeed);
                haloLine.endColor = Color.Lerp(haloLine.startColor, activeHalo, Time.deltaTime * colorLerpSpeed);
            }
        }
    }

    private void HaloPoints()
    {
        int index = 0;

        haloLine.positionCount = 41;

        for (float i = 1; i >= 0; i -= 0.1f)
        {
            Vector3 vectorDirector = new Vector3(i, 1 - i, 0);
            haloLine.SetPosition(index, (vectorDirector.normalized * radius) + playership.position);
            index += 1;
        }

        for (float i = 0; i >= -1; i -= 0.1f)
        {
            Vector3 vectorDirector = new Vector3(i, 1 + i, 0);
            haloLine.SetPosition(index, (vectorDirector.normalized * radius) + playership.position);
            index += 1;
        }

        for (float i = -1; i <= 0; i += 0.1f)
        {
            Vector3 vectorDirector = new Vector3(i, -1 - i, 0);
            haloLine.SetPosition(index, (vectorDirector.normalized * radius) + playership.position);
            index += 1;
        }

        for (float i = 0; i <= 1; i += 0.1f)
        {
            Vector3 vectorDirector = new Vector3(i, -1 + i, 0);
            haloLine.SetPosition(index, (vectorDirector.normalized * radius) + playership.position);
            index += 1;
        }

        haloLine.SetPosition(40, (new Vector3(1, 0, 0) * radius) + playership.position);
    }

    private void HaloProperties()
    {
        haloLine.startWidth = width;
        haloLine.endWidth = width;
    }
}
