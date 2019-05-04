using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_MiningTool : Scr_ToolBase
{
    [Header("Repairing Tool Parameters")]
    [SerializeField] private float distance;
    [SerializeField] private float angleLimit;
    [SerializeField] private float miningSpeed;
    [SerializeField] private LayerMask masker;
    [SerializeField] private Color miningColor;

    [HideInInspector] public Camera mainCamera;

    private bool executingRepairingTool;
    private LineRenderer laser;
    private RaycastHit2D hitLaser;
    private Vector3 lastDirection;

    void Start()
    {
        mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();

        laser = GetComponent<LineRenderer>();
        laser.material.color = miningColor;
    }

    public override void Update()
    {
        if (Input.GetMouseButton(0) && transform.parent.GetComponent<Scr_IAMovement>().isMining)
        {
            executingRepairingTool = true;
            laser.enabled = executingRepairingTool;

            if (executingRepairingTool)
                MiningTool();
        }

        else
        {
            executingRepairingTool = false;
            laser.enabled = executingRepairingTool;
        }
    }

    public override void UseTool() { }

    public override void Function() { }

    public override void RecoverTool() { }

    public override void OnMouseEnter() { }

    public override void OnMouseExit() { }

    private void MiningTool()
    {
        LaserPosition();
        LaserFunction();
    }

    private void LaserPosition()
    {
        Vector3 direction = (mainCamera.ScreenToWorldPoint(Input.mousePosition) - (transform.position + (transform.up * 0.01f)));
        direction = new Vector3(direction.x, direction.y, 0).normalized;
        hitLaser = Physics2D.Raycast(transform.position, -transform.up, Mathf.Infinity, masker);

        laser.SetPosition(0, transform.position);
        laser.SetPosition(1, hitLaser.point);
    }

    private void LaserFunction()
    {
        if (hitLaser)
        {
            if (hitLaser.collider.transform.CompareTag("Block"))
                hitLaser.collider.transform.gameObject.GetComponent<Scr_Ore>().amount -= miningSpeed * Time.deltaTime;

            else if(hitLaser.collider.transform.CompareTag("Breakeable"))
                hitLaser.collider.transform.gameObject.GetComponent<Scr_Breakeable>().amount -= miningSpeed * Time.deltaTime;
        }
    }
}