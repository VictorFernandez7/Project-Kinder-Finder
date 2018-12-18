using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_PlanetStats : MonoBehaviour {

    [Header("Data Requirements")]
    [SerializeField] private Scr_SystemData planetData;
    [SerializeField] private int systemIndex;
    [SerializeField] private int planetIndex;

    private float radius;

	void Start ()
    {
        radius = GetComponentInChildren<Renderer>().bounds.size.x / 2;
	}
	
	void Update () {
		
	}
}