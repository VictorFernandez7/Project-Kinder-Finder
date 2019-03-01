using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_LiquidTool : Scr_ToolBase
{
    [Header("Tool Parameters")]
    [SerializeField] private float extractionSpeed;

    [HideInInspector] public bool onRange;
    [HideInInspector] public GameObject zone;

    private float amount;

    public override void Update()
    {
        if (Input.GetMouseButton(0) && onRange)
            ExtractLiquid();
    }

    public override void UseTool() { }

    public override void Function() { }

    public override void RecoverTool() { }

    public override void OnMouseEnter() { }

    public override void OnMouseExit() { }

    private void ExtractLiquid()
    {
        if (resource == null)
            resource = zone.GetComponent<Scr_LiquidZone>().currentResource;

        else if(resource != zone.GetComponent<Scr_LiquidZone>().currentResource)
        {
            resource = zone.GetComponent<Scr_LiquidZone>().currentResource;
            amount = 0;
        }

        amount += extractionSpeed * Time.deltaTime;
        zone.GetComponent<Scr_LiquidZone>().amount -= extractionSpeed * Time.deltaTime;

        if(amount >= 1)
        {
            //Create Resource
        }
    }
}