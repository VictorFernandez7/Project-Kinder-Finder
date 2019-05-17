using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_RepairingTool : Scr_ToolBase
{
    [Header("Repairing Tool Parameters")]
    [SerializeField] private float distance;
    [SerializeField] private float angleLimit;
    [SerializeField] private float repairingFactor;
    [SerializeField] private float laserSpeed;
    [SerializeField] private LayerMask masker;
    [SerializeField] private Color repairingColor;

    [HideInInspector] public Camera mainCamera;

    private bool executingRepairingTool;
    private bool isMining;
    private float laserPercent = -2f;
    private GameObject playerShip;
    private LineRenderer laser;
    private RaycastHit2D hitLaser;
    private Vector3 hitLaserPoint;
    private Vector3 laserPoint;

    void Start ()
    {
        mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        playerShip = GameObject.Find("PlayerShip");

        laser = GetComponent<LineRenderer>();
        laser.material.color = repairingColor;
    }

    public override void Update ()
    {
        if (GetComponentInParent<Scr_IAMovement>().target)
            hitLaser = Physics2D.Raycast(transform.position, GetComponentInParent<Scr_IAMovement>().target.parent.transform.position - transform.position, Mathf.Infinity, masker);

        if (Input.GetButton("Interact") && GetComponentInParent<Scr_IAMovement>().isMining && hitLaser)
        {
            executingRepairingTool = true;
            laser.enabled = executingRepairingTool;

            if (executingRepairingTool)
            {
                RepairingTool(true);
                isMining = true;
                laserPercent += Time.deltaTime * laserSpeed;
            }
        }

        else if (executingRepairingTool && laserPercent > 0)
        {
            RepairingTool(false);
            laserPercent -= Time.deltaTime * laserSpeed;
        }

        else
        {
            executingRepairingTool = false;
            laser.enabled = executingRepairingTool;
            laserPercent = -2f;

            if (!hitLaser && isMining)
                GetComponentInParent<Scr_IAMovement>().isMining = false;

            isMining = false;
        }
    }

    public override void UseTool() { }

    public override void Function() { }

    public override void RecoverTool() { }

    public override void OnMouseEnter() { }

    public override void OnMouseExit() { }

    private void RepairingTool(bool activating)
    {
        LaserPosition(activating);
        LaserFunction();
    }

    private void LaserPosition(bool activating)
    {
        laser.SetPosition(0, transform.position);

        if (laserPercent > 1)
            laserPercent = 1;

        int switcher;

        if (laserPercent < 0)
            switcher = 0;

        else
            switcher = 1;

        if (hitLaser)
            hitLaserPoint = hitLaser.point;

        if (activating)
        {
            distance = Vector2.Distance(transform.position, hitLaserPoint);
            laserPoint = transform.position + ((GetComponentInParent<Scr_IAMovement>().target.parent.transform.position - transform.position).normalized * distance * laserPercent * switcher);
        }

        else
            laserPoint = transform.position + ((hitLaserPoint - transform.position).normalized * distance * laserPercent);

        laser.SetPosition(1, laserPoint);
    }

    private void LaserFunction()
    {
        if (hitLaser)
        {
            if (hitLaser.collider.transform.CompareTag("PlayerShip"))
            {
                if (playerShip.GetComponent<Scr_PlayerShipStats>().currentShield < playerShip.GetComponent<Scr_PlayerShipStats>().maxShield && laserPercent == 1)
                    playerShip.GetComponent<Scr_PlayerShipStats>().currentShield += Time.deltaTime * repairingFactor;
            }
        }
    }
}