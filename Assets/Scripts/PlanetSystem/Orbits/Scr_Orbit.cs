using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Orbit : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LineRenderer orbitLine;
    [SerializeField] private Transform planet;
    [SerializeField] private Transform pivot;

    private float magnitude;

    void Start ()
    {
        magnitude = (planet.position - pivot.position).magnitude;
        CreateOrbit();
    }

    private void Update()
    {
        CreateOrbit();
    }

    private void CreateOrbit()
    {
        int index = 0;
        
        orbitLine.positionCount = 81;

        for (float i = 1; i >= 0; i -= 0.05f)
        {
            Vector3 vectorDirector = new Vector3(i, 1 - i, 0);
            orbitLine.SetPosition(index, (vectorDirector.normalized * magnitude) + pivot.position);
            index += 1;
        }

        for (float i = 0; i >= -1; i -= 0.05f)
        {
            Vector3 vectorDirector = new Vector3(i, 1 + i, 0);
            orbitLine.SetPosition(index, (vectorDirector.normalized * magnitude) + pivot.position);
            index += 1;
        }

        for (float i = -1; i <= 0; i += 0.05f)
        {
            Vector3 vectorDirector = new Vector3(i, -1 - i, 0);
            orbitLine.SetPosition(index, (vectorDirector.normalized * magnitude) + pivot.position);
            index += 1;
        }

        for (float i = 0; i <= 1; i += 0.05f)
        {
            Vector3 vectorDirector = new Vector3(i, -1 + i, 0);
            orbitLine.SetPosition(index, (vectorDirector.normalized * magnitude) + pivot.position);
            index += 1;
        }

        orbitLine.SetPosition(80, (new Vector3(1, 0, 0) * magnitude) + pivot.position);
    }
}