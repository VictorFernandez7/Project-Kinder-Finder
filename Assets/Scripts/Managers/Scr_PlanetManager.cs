using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_PlanetManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public GameObject[] planets;

    public void Gravity(bool active)
    {
        if (active)
        {
            for (int i = 0; i < planets.Length; i++)
            {
                planets[i].GetComponent<Scr_Planet>().switchGravity = true;
            }
        }

        else
        {
            for (int i = 0; i < planets.Length; i++)
            {
                planets[i].GetComponent<Scr_Planet>().switchGravity = false;
            }
        }
    }
}