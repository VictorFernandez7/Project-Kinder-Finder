using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Jetpack : Scr_ToolBase
{
    [Header("Jetpack Properties")]
    [SerializeField] private float speedJetpack;
    [SerializeField] private float timeCharge;

    private Scr_AstronautMovement astronautMovement;
    private GameObject astronaut;
    private float savedTimeCharge;
    private bool charge;

    void Start ()
    {
        astronaut = GameObject.Find("Astronaut");

        astronautMovement = astronaut.GetComponent<Scr_AstronautMovement>();

        savedTimeCharge = timeCharge;
        charge = true;
    }

	public override void Update ()
    {
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

    public override void Function() { }

    public override void RecoverTool() { }

    public override void OnMouseEnter() { }

    public override void OnMouseExit() { }
}
