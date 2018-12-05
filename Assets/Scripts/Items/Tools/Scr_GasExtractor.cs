using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_GasExtractor : Scr_ToolBase {

    [SerializeField] private float extractorTime;

    [HideInInspector] public int resourceAmount;

    private float savedExtractorTime;
    private GameObject gasZone;

    // Use this for initialization
    void Start ()
    {
        savedExtractorTime = extractorTime;
        resourceAmount = 0;
    }
	
	// Update is called once per frame
	public override void Update ()
    {
        PutOnPlace();
    }

    public override void UseTool()
    {
        
    }

    public override void Function()
    {
        savedExtractorTime -= Time.deltaTime;
        if (savedExtractorTime <= 0)
        {
            resourceAmount += 1;
            gasZone.GetComponent<Scr_GasZone>().amount -= 1;
            savedExtractorTime = extractorTime;
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
}
