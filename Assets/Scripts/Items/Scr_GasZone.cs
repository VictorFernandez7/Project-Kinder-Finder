using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_GasZone : MonoBehaviour {

    [SerializeField] public GameObject resource;
    [SerializeField] public float amount;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(amount <= 0)
        {
            Destroy(gameObject);
        }
	}
}
