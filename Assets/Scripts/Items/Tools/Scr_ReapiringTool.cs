using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_ReapiringTool : Scr_ToolBase {

    [SerializeField] private float distance;
    [SerializeField] private LayerMask masker;

    private LineRenderer laser;
    private bool executingRepairingTool;
    private RaycastHit2D hitLaser;


    // Use this for initialization
    void Start () {
        laser = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	public override void Update () {
        if (executingRepairingTool)
        {
            Multitool();
        }
    }

    public override void UseTool()
    {
        executingRepairingTool = !executingRepairingTool;
        laser.enabled = executingRepairingTool;
    }

    public override void Function()
    {
        
    }

    public override void RecoverTool()
    {
        
    }

    private void Multitool()
    {
        hitLaser = Physics2D.Raycast(transform.position + (transform.up * 0.01f), transform.right, distance, masker);

        laser.SetPosition(0, transform.position + (transform.up * 0.01f));

        if (hitLaser)
        {
            laser.SetPosition(1, hitLaser.point);
        }
        else
        {
            laser.SetPosition(1, (transform.position + (transform.up * 0.01f)) + transform.right * -distance);
        }

    }
}
