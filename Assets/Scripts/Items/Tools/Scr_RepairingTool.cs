using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_RepairingTool : Scr_ToolBase
{
    [Header("Repairing Tool Parameters")]
    [SerializeField] private float distance;
    [SerializeField] private float angleLimit;
    [SerializeField] private float repairingFactor;
    [SerializeField] private LayerMask masker;
    [SerializeField] private Color miningColor;
    [SerializeField] private Color repairingColor;

    [HideInInspector] public Camera mainCamera;

    private bool executingRepairingTool;
    private bool miningMode;
    private GameObject playerShip;
    private LineRenderer laser;
    private RaycastHit2D hitLaser;
    private Vector3 lastDirection;

    void Start ()
    {
        mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        playerShip = GameObject.Find("PlayerShip");

        laser = GetComponent<LineRenderer>();

        miningMode = true;
    }

    public override void Update () {

        if (Input.GetMouseButton(0))
        {
            executingRepairingTool = true;
            laser.enabled = executingRepairingTool;

            if (executingRepairingTool)
                Multitool(miningMode);
        }

        else
        {
            executingRepairingTool = false;
            laser.enabled = executingRepairingTool;
        }

        if (Input.GetMouseButtonDown(2))
            miningMode = !miningMode;
    }

    public override void UseTool() { }

    public override void Function() { }

    public override void RecoverTool() { }

    public override void OnMouseEnter() { }

    public override void OnMouseExit() { }

    private void Multitool(bool miningMode)
    {
        if(miningMode == true)
            laser.material.color = miningColor;

        else
            laser.material.color = repairingColor;

        LaserPosition();
        LaserFunction(miningMode);
    }

    private void LaserPosition()
    {
        Vector3 direction = (mainCamera.ScreenToWorldPoint(Input.mousePosition) - (transform.position + (transform.up * 0.01f)));
        direction = new Vector3(direction.x, direction.y, 0).normalized;
        hitLaser = Physics2D.Raycast(transform.position + (transform.up * 0.01f), direction, distance, masker);

        laser.SetPosition(0, transform.position + (transform.up * 0.01f));

        if (Vector3.Angle(transform.right, direction) < angleLimit)
        {
            if (hitLaser)
                laser.SetPosition(1, hitLaser.point);

            else
                laser.SetPosition(1, (transform.position + (transform.up * 0.01f)) + (direction * distance));

            lastDirection = direction;
        }

        else if (Vector3.Angle(transform.right, lastDirection) < 90)
            laser.SetPosition(1, (transform.position + (transform.up * 0.01f)) + (lastDirection * distance));

        else
            laser.SetPosition(1, transform.position);
    }

    private void LaserFunction(bool mode)
    {
        if (mode)
        {
            if (hitLaser)
            {
                if (hitLaser.collider.transform.CompareTag("Block"))
                {
                    hitLaser.collider.transform.gameObject.GetComponent<Scr_Ore>().resistanceTime -= Time.deltaTime;
                }
            }
        }

        else
        {
            if (hitLaser)
            {
                if (hitLaser.collider.transform.CompareTag("PlayerShip"))
                {
                    if (playerShip.GetComponent<Scr_PlayerShipStats>().currentShield < playerShip.GetComponent<Scr_PlayerShipStats>().maxShield)
                        playerShip.GetComponent<Scr_PlayerShipStats>().currentShield += Time.deltaTime * repairingFactor;
                }
            }
        }
    }
}