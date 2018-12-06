using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_GasExtractor : Scr_ToolBase {

    [SerializeField] private float extractorTime;
    [SerializeField] private LayerMask mask;

    [HideInInspector] public Camera mainCamera;
    [HideInInspector] public bool recolectable;

    private float savedExtractorTime;
    private GameObject gosht;
    private GameObject gasZone;
    private GameObject astronaut;
    private RaycastHit2D hit;
    private RaycastHit2D hitR;
    private RaycastHit2D hitL;
    private Scr_AstronautMovement astronautMovement;
    private bool placing;

    // Use this for initialization
    void Start ()
    {
        mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        astronautMovement = GameObject.Find("Astronaut").GetComponent<Scr_AstronautMovement>();
        astronaut = GameObject.Find("Astronaut");

        savedExtractorTime = extractorTime;
        resourceAmount = 0;
        gasZone = null;
        onHands = true;
        recolectable = false;
    }
	
	// Update is called once per frame
	public override void Update ()
    {
        if(placing)
            PutOnPlace();

        if (!onHands && gasZone != null)
            Function();

        if (gasZone == null)
        {
            recolectable = false;
        }
    }

    public override void UseTool()
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

    public override void RecoverTool()
    {
        if (astronaut.GetComponent<Scr_AstronautsActions>().emptyHands && !astronaut.GetComponent<Scr_AstronautsActions>().toolOnHands && !onHands)
        {
            for (int i = 0; i < astronaut.GetComponent<Scr_AstronautStats>().toolSlots.Length; i++)
            {
                if (astronaut.GetComponent<Scr_AstronautStats>().toolSlots[i] == null)
                {
                    astronaut.GetComponent<Scr_AstronautStats>().toolSlots[i] = astronaut.GetComponent<Scr_AstronautsActions>().prefab;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "GasZone")
        {
            print("pedo");
            recolectable = true;
            gasZone = collision.gameObject;
            resource = gasZone.GetComponent<Scr_GasZone>().currentResource;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "GasZone")
        {
            recolectable = false;
            gasZone = null;
        }
    }
}
