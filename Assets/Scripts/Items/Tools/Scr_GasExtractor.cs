using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scr_GasExtractor : Scr_ToolBase {

    [SerializeField] private float extractorTime;
    [SerializeField] private LayerMask mask;
    [SerializeField] private GameObject resourceText;


    [HideInInspector] public Camera mainCamera;
    [HideInInspector] public bool recolectable;
    [HideInInspector] public int resourceLeft;

    private TextMeshProUGUI text;
    private Scr_ReferenceManager referenceManager;
    private float savedExtractorTime;
    private GameObject ghost;
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
        referenceManager = GameObject.Find("ReferenceManager").GetComponent<Scr_ReferenceManager>();
        text = resourceText.GetComponent<TextMeshProUGUI>();

        resourceText.SetActive(false);

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

        if(!onHands)
            text.text = resourceAmount.ToString();
    }

    public override void UseTool()
    {
        if (onHands)
        {
            placing = !placing;

            if (placing)
            {
                ghost = Instantiate(gameObject);
            }
            else if (!placing)
            {
                Destroy(ghost);
            }
        }
    }

    public override void Function()
    {
        savedExtractorTime -= Time.deltaTime;

        if (savedExtractorTime <= 0 && gasZone.GetComponent<Scr_GasZone>().amount > 0)
        {
            resourceAmount += 1;
            savedExtractorTime = extractorTime;
        }

        gasZone.GetComponent<Scr_GasZone>().amount -= Time.deltaTime / extractorTime;

        if(gasZone.GetComponent<Scr_GasZone>().amount <= 0 && resourceAmount != resourceLeft)
        {
            resourceAmount = resourceLeft;
        }  
    }

    private void PutOnPlace()
    {
        hit = Physics2D.Raycast(ghost.transform.position, (astronautMovement.currentPlanet.transform.position - transform.position).normalized, Mathf.Infinity, mask);
        hitR = Physics2D.Raycast(ghost.transform.position + transform.right, (astronautMovement.currentPlanet.transform.position - ghost.transform.position).normalized, Mathf.Infinity, mask);
        hitL = Physics2D.Raycast(ghost.transform.position - transform.right, (astronautMovement.currentPlanet.transform.position - ghost.transform.position).normalized, Mathf.Infinity, mask);

        float mouseposX = mainCamera.ScreenToWorldPoint(Input.mousePosition).x;
        float mouseposY = mainCamera.ScreenToWorldPoint(Input.mousePosition).y;
        mouseposX = Mathf.Clamp(mouseposX, astronaut.transform.position.x - 0.4f, astronaut.transform.position.x + 0.4f);
        mouseposY = Mathf.Clamp(mouseposY, astronaut.transform.position.y - 0.4f, astronaut.transform.position.y + 0.4f);
        Vector3 mousepos = new Vector3(mouseposX, mouseposY, 0f);
        ghost.transform.position = astronautMovement.currentPlanet.transform.position + ((mousepos - astronautMovement.currentPlanet.transform.position).normalized * (Vector3.Distance(hit.point, astronautMovement.currentPlanet.transform.position) + GetComponent<Renderer>().bounds.size.y / 2));
        ghost.transform.localRotation = Quaternion.LookRotation(ghost.transform.forward, Vector2.Perpendicular(hitL.point - hitR.point));

        if (ghost.GetComponent<Scr_GasExtractor>().recolectable)
        {
            Color color = ghost.GetComponent<Renderer>().material.color;
            color.g = 250;
            color.b = 0;
            color.r = 0;
            color.a = 0.6f;
            ghost.GetComponent<Renderer>().material.color = color;

            if (Input.GetMouseButtonDown(0))
            {
                resourceText.SetActive(true);
                transform.SetParent(null);
                transform.position = ghost.transform.position;
                transform.rotation = Quaternion.LookRotation(transform.forward, (transform.position - astronautMovement.currentPlanet.transform.position));
                transform.SetParent(astronautMovement.currentPlanet.transform);
                onHands = false;
                placing = false;
                astronaut.GetComponent<Scr_AstronautStats>().toolSlots[astronaut.GetComponent<Scr_AstronautsActions>().numberToolActive] = null;
                astronaut.GetComponent<Scr_AstronautStats>().physicToolSlots[astronaut.GetComponent<Scr_AstronautsActions>().numberToolActive] = null;
                astronaut.GetComponent<Scr_AstronautsActions>().BoolControl();
                gasZone = ghost.GetComponent<Scr_GasExtractor>().gasZone;
                resource = gasZone.GetComponent<Scr_GasZone>().currentResource;
                resourceLeft = (int)gasZone.GetComponent<Scr_GasZone>().amount;
                Destroy(ghost);
            }
        }

        else
        {
            Color color = ghost.GetComponent<Renderer>().material.color;
            color.g = 0;
            color.b = 0;
            color.r = 250;
            color.a = 0.6f;
            ghost.GetComponent<Renderer>().material.color = color;
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
                    resourceText.SetActive(false);
                    astronaut.GetComponent<Scr_AstronautStats>().toolSlots[i] = referenceManager.GasExtractor;
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
