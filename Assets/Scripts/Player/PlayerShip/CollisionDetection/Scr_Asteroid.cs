using UnityEngine;
using System;

public class Scr_Asteroid: IComparable<Scr_Asteroid>
{
    public string name;
    public GameObject body;
    public float distanceToShip;
    public Vector3 currentPos;

    public Scr_Asteroid (string newName, GameObject newBody,float newDistanceToShip, Vector3 newCurrentPos)
    {
        name = newName;
        body = newBody;
        distanceToShip = newDistanceToShip;
        currentPos = newCurrentPos;
    }

    public int CompareTo(Scr_Asteroid other)
    {
        if (other == null)
            return 1;

        return (int) (distanceToShip - other.distanceToShip);
    }
}