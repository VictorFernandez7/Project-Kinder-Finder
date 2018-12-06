using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Jetpack : Scr_ToolBase {

    [SerializeField] private float speedJetpack;

    private Scr_AstronautMovement astronautMovement;
    private GameObject astronaut;

    // Use this for initialization
    void Start () {
        astronautMovement = GameObject.Find("Astronaut").GetComponent<Scr_AstronautMovement>();
        astronaut = GameObject.Find("Astronaut");
    }
	
	// Update is called once per frame
	public override void Update () {
		
	}

    public override void UseTool()
    {
            astronautMovement.vectorJump = (astronaut.transform.position - astronautMovement.currentPlanet.transform.position).normalized * speedJetpack;
            astronautMovement.jumping = true;
    }

    public override void Function()
    {
        
    }

    public override void RecoverTool()
    {
        
    }
}
