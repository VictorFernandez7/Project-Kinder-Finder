using UnityEngine;
using System;

public class Scr_PlanetClass
{
    public string name;
    public GameObject body;
    public float distanceToShip;
    public Vector3 currentPos;

    public Scr_PlanetClass(string newName, GameObject newBody, float newDistanceToShip, Vector3 newCurrentPos)
    {
        name = newName;
        body = newBody;
        distanceToShip = newDistanceToShip;
        currentPos = newCurrentPos;
    }
}