using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Jetpack : Scr_ToolBase {

    [SerializeField] private float speedJetpack;
    [SerializeField] private float timeCharge;

    private Scr_AstronautMovement astronautMovement;
    private GameObject astronaut;
    private float savedTimeCharge;
    private bool charge;

    // Use this for initialization
    void Start () {
        astronautMovement = GameObject.Find("Astronaut").GetComponent<Scr_AstronautMovement>();
        astronaut = GameObject.Find("Astronaut");
        savedTimeCharge = timeCharge;
        charge = true;
    }
	
	// Update is called once per frame
	public override void Update () {
        if (!charge)
        {
            savedTimeCharge -= Time.deltaTime;
            if(savedTimeCharge <= 0)
            {
                charge = true;
                savedTimeCharge = timeCharge;
            }
        }
	}

    public override void UseTool()
    {
        if (charge)
        {
            astronautMovement.vectorJump = (astronaut.transform.position - astronautMovement.currentPlanet.transform.position).normalized * speedJetpack;
            astronautMovement.jumping = true;
            astronautMovement.timeAtAir = 0;
            charge = false;
        }
    }

    public override void Function()
    {
        
    }

    public override void RecoverTool()
    {
        
    }
}
