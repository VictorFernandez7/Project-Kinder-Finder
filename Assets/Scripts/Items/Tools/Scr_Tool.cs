﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Tool : MonoBehaviour {

    [Header("Tool Info")]
    [SerializeField] public string toolName;
    [SerializeField] private LayerMask mask;
    [SerializeField] private LayerMask masker;
    [SerializeField] private bool placeable;
    [SerializeField] private bool multitool;
    [SerializeField] private float distance;
    [SerializeField] private LineRenderer laser;

    [HideInInspector] public bool onHands;
    [HideInInspector] public Camera mainCamera;

    private bool placing;
    private bool onRange;
    private bool executingMultitool;
    private GameObject gosht;
    private Scr_AstronautMovement astronautMovement;
    private GameObject astronaut;
    private RaycastHit2D hit;
    private RaycastHit2D hitR;
    private RaycastHit2D hitL;
    private RaycastHit2D hitLaser;

    void Start () {
        mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        astronautMovement = GameObject.Find("Astronaut").GetComponent<Scr_AstronautMovement>();
        astronaut = GameObject.Find("Astronaut");
        onHands = true;
    }

	void Update () {
        if (placing)
        {
            PutOnPlace();
        }

        if (executingMultitool)
        {
            Multitool();
        }
    }

    public void UseTool()
    {
        if (placeable)
        {
            if (onHands)
            {
                placing = !placing;

                if (placing)
                {
                    gosht = Instantiate(gameObject);
                }
                else if (!placing)
                {
                    Destroy(gosht);
                }
            }
        }

        if (multitool)
        {
            executingMultitool = !executingMultitool;
            laser.enabled = executingMultitool;
        }
    }

    public void RecoverTool()
    {
        if (astronaut.GetComponent<Scr_AstronautsActions>().emptyHands && !astronaut.GetComponent<Scr_AstronautsActions>().toolOnHands && !onHands)
        {
            for (int i = 0; i < astronaut.GetComponent<Scr_AstronautStats>().toolSlots.Length; i++)
            {
                if (astronaut.GetComponent<Scr_AstronautStats>().toolSlots[i] == null)
                {
                    astronaut.GetComponent<Scr_AstronautStats>().toolSlots[i] = gameObject;
                    astronaut.GetComponent<Scr_AstronautStats>().physicToolSlots[i] = gameObject;
                    transform.SetParent(null);
                    transform.position = astronaut.GetComponent<Scr_AstronautsActions>().pickPoint.position;
                    transform.SetParent(astronaut.GetComponent<Scr_AstronautsActions>().pickPoint);
                    onHands = true;
                    astronaut.GetComponent<Scr_AstronautsActions>().BoolControl();
                    break;
                }
            }
        }
    }

    private void PutOnPlace()
    {
        hit = Physics2D.Raycast(gosht.transform.position, (astronautMovement.currentPlanet.transform.position - transform.position).normalized, Mathf.Infinity, mask);
        hitR = Physics2D.Raycast(gosht.transform.position + transform.right, (astronautMovement.currentPlanet.transform.position - gosht.transform.position).normalized, Mathf.Infinity, mask);
        hitL = Physics2D.Raycast(gosht.transform.position - transform.right, (astronautMovement.currentPlanet.transform.position - gosht.transform.position).normalized, Mathf.Infinity, mask);

        float mouseposX = mainCamera.ScreenToWorldPoint(Input.mousePosition).x;
        float mouseposY = mainCamera.ScreenToWorldPoint(Input.mousePosition).y;
        mouseposX = Mathf.Clamp(mouseposX, astronaut.transform.position.x - 0.4f, astronaut.transform.position.x + 0.4f);
        mouseposY = Mathf.Clamp(mouseposY, astronaut.transform.position.y - 0.4f, astronaut.transform.position.y + 0.4f);
        Vector3 mousepos = new Vector3(mouseposX, mouseposY, 0f);
        gosht.transform.position = astronautMovement.currentPlanet.transform.position + ((mousepos - astronautMovement.currentPlanet.transform.position).normalized * (Vector3.Distance(hit.point, astronautMovement.currentPlanet.transform.position) + GetComponent<Renderer>().bounds.size.y / 2));
        gosht.transform.localRotation = Quaternion.LookRotation(gosht.transform.forward, Vector2.Perpendicular(hitL.point - hitR.point));
        Color color = gosht.GetComponent<Renderer>().material.color;
        color.g = 250;
        color.b = 0;
        color.r = 0;
        color.a = 0.6f;
        gosht.GetComponent<Renderer>().material.color = color;

        if (Input.GetMouseButtonDown(0))
        {
            transform.SetParent(null);
            transform.position = gosht.transform.position;
            transform.rotation = Quaternion.LookRotation(transform.forward, (transform.position - astronautMovement.currentPlanet.transform.position));
            Destroy(gosht);
            transform.SetParent(astronautMovement.currentPlanet.transform);
            onHands = false;
            placing = false;
            astronaut.GetComponent<Scr_AstronautStats>().toolSlots[astronaut.GetComponent<Scr_AstronautsActions>().numberToolActive] = null;
            astronaut.GetComponent<Scr_AstronautStats>().physicToolSlots[astronaut.GetComponent<Scr_AstronautsActions>().numberToolActive] = null;
            astronaut.GetComponent<Scr_AstronautsActions>().BoolControl();
        }
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