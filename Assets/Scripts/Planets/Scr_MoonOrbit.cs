using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_MoonOrbit : MonoBehaviour
{
    [SerializeField] private GameObject planet;

	void Update () {
        transform.position = planet.transform.position;
	}
}
