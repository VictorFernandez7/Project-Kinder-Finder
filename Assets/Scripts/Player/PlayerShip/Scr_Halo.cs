using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_PlayerShipHalo : MonoBehaviour {

    [SerializeField] private LineRenderer haloLine;
    [SerializeField] private float radius;
    [SerializeField] private Transform playership;

    // Use this for initialization
    void Start () {
        CreateOrbit();

    }
	
	// Update is called once per frame
	void Update () {
        CreateOrbit();
    }

    private void CreateOrbit()
    {
        int index = 0;

        orbitLine.positionCount = 41;

        for (float i = 1; i >= 0; i -= 0.1f)
        {
            Vector3 vectorDirector = new Vector3(i, 1 - i, 0);
            orbitLine.SetPosition(index, (vectorDirector.normalized * radius) + pivot.position);
            index += 1;
        }

        for (float i = 0; i >= -1; i -= 0.1f)
        {
            Vector3 vectorDirector = new Vector3(i, 1 + i, 0);
            orbitLine.SetPosition(index, (vectorDirector.normalized * radius) + pivot.position);
            index += 1;
        }

        for (float i = -1; i <= 0; i += 0.1f)
        {
            Vector3 vectorDirector = new Vector3(i, -1 - i, 0);
            orbitLine.SetPosition(index, (vectorDirector.normalized * radius) + pivot.position);
            index += 1;
        }

        for (float i = 0; i <= 1; i += 0.1f)
        {
            Vector3 vectorDirector = new Vector3(i, -1 + i, 0);
            orbitLine.SetPosition(index, (vectorDirector.normalized * radius) + pivot.position);
            index += 1;
        }

        orbitLine.SetPosition(40, (new Vector3(1, 0, 0) * radius) + pivot.position);
    }
}
