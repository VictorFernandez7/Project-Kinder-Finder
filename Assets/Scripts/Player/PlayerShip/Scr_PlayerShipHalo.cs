using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_PlayerShipHalo : MonoBehaviour
{
    [Header("Halo Sttings")]
    [SerializeField] private float radius;
    [SerializeField] private float width;
    [SerializeField] private Color haloColor;

    [Header("References")]
    [SerializeField] private Transform playership;

    private LineRenderer haloLine;

    private void Start()
    {
        haloLine = GetComponent<LineRenderer>();
    }

    void Update()
    {
        HaloPoints();
        HaloProperties();
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
        haloLine.startColor = haloColor;
        haloLine.endColor = haloColor;
    }
}
