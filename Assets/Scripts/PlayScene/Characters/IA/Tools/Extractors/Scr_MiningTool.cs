using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_MiningTool : Scr_ToolBase
{
    [Header("Repairing Tool Parameters")]
    [SerializeField] private float miningSpeed;
    [SerializeField] private float laserSpeed;
    [SerializeField] private LayerMask masker;

    [Header("References")]
    [SerializeField] private ParticleSystem startParticles;
    [SerializeField] private ParticleSystem hitParticles;

    [HideInInspector] public Camera mainCamera;

    private bool executingRepairingTool;
    private bool isMining;
    private float laserPercent = -2f;
    private float distance;
    private Vector3 laserPoint;
    private Vector3 hitLaserPoint;
    private LineRenderer laser;
    private RaycastHit2D hitLaser;

    void Start()
    {
        mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();

        laser = GetComponentInChildren<LineRenderer>();

        startParticles.Stop();
        hitParticles.Stop();
    }

    public override void Update()
    {
        if(GetComponentInParent<Scr_IAMovement>().target)
            hitLaser = Physics2D.Raycast(transform.position, GetComponentInParent<Scr_IAMovement>().target.parent.transform.position - transform.position, Mathf.Infinity, masker);

        if (Input.GetButton("Interact") && GetComponentInParent<Scr_IAMovement>().isMining && hitLaser)
        {
            executingRepairingTool = true;
            laser.enabled = executingRepairingTool;

            if (executingRepairingTool)
            {
                MiningTool(true);
                isMining = true;
                laserPercent += Time.deltaTime * laserSpeed;
            }
        }

        else if (executingRepairingTool && laserPercent > 0)
        {
            MiningTool(false);
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

        ParticleManagement();
    }

    public override void UseTool() { }

    public override void Function() { }

    public override void RecoverTool() { }

    public override void OnMouseEnter() { }

    public override void OnMouseExit() { }

    private void MiningTool(bool activating)
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
            if (hitLaser.collider.transform.CompareTag("Block") && laserPercent == 1)
                hitLaser.collider.transform.gameObject.GetComponent<Scr_Ore>().amount -= miningSpeed * Time.deltaTime;

            else if(hitLaser.collider.transform.CompareTag("Breakeable") && laserPercent == 1)
                hitLaser.collider.transform.gameObject.GetComponent<Scr_Breakeable>().amount -= miningSpeed * Time.deltaTime;
        }
    }

    private void ParticleManagement()
    {
        if (laserPercent >= 0)
        {
            if (!startParticles.isPlaying)
                startParticles.Play();

            Vector3 direction = GetComponentInParent<Scr_IAMovement>().target.parent.transform.position - transform.position;
            startParticles.transform.forward = direction;
        }

        else
            startParticles.Stop();

        if (laserPercent >= 1)
        {
            if (!hitParticles.isPlaying)
                hitParticles.Play();

            hitParticles.transform.position = hitLaser.point;
        }

        else
            hitParticles.Stop();
    }
}