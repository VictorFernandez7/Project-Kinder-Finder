using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Scr_OreExtractor : Scr_ToolBase {

    [SerializeField] private float extractorTime;
    [SerializeField] private LayerMask mask;
    [SerializeField] private GameObject resourceCanvas;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI remainingResources;
    [SerializeField] private TextMeshProUGUI harvestedResources;
    [SerializeField] private Slider harvestProcess;
    [SerializeField] private GameObject playerCheck;

    [HideInInspector] public Camera mainCamera;
    [HideInInspector] public bool recolectable;
    [HideInInspector] public bool playerNear;


    private Scr_ReferenceManager referenceManager;
    private float savedExtractorTime;
    private float process;
    private GameObject ghost;
    private GameObject oreZone;
    private GameObject astronaut;
    private RaycastHit2D hit;
    private RaycastHit2D hitR;
    private RaycastHit2D hitL;
    private Scr_AstronautMovement astronautMovement;
    private bool placing;
    private bool showInterface;

    void Start()
    {
        mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        astronautMovement = GameObject.Find("Astronaut").GetComponent<Scr_AstronautMovement>();
        astronaut = GameObject.Find("Astronaut");
        referenceManager = GameObject.Find("ReferenceManager").GetComponent<Scr_ReferenceManager>();

        resourceCanvas.SetActive(false);
        playerCheck.SetActive(false);

        savedExtractorTime = extractorTime;
        resourceAmount = 0;
        oreZone = null;
        onHands = true;
        recolectable = false;
    }

    public override void Update()
    {
        if (placing)
            PutOnPlace();

        if (!onHands && oreZone != null && oreZone.GetComponent<Scr_OreZone>().amount > 0)
            Function();

        if (oreZone == null)
        {
            recolectable = false;
        }

        if (!onHands && (showInterface || playerNear))
            Interface();

        else
            resourceCanvas.SetActive(false);

    }

    public override void UseTool()
    {
        if (onHands)
        {
            placing = !placing;

            if (placing)
            {
                ghost = Instantiate(gameObject, astronaut.transform.position, astronaut.transform.rotation);
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

        if (savedExtractorTime <= 0 && oreZone.GetComponent<Scr_OreZone>().amount > 0)
        {
            resourceAmount += 1;
            savedExtractorTime = extractorTime;
        }


        oreZone.GetComponent<Scr_OreZone>().amount -= Time.deltaTime / extractorTime;

        if (process == 3)
            process = 0;

        process += Time.deltaTime;
        process = Mathf.Clamp(process, 0, 3);


        if (oreZone.GetComponent<Scr_OreZone>().amount <= 0 && resourceAmount != resourceLeft)
        {
            resourceAmount = resourceLeft;
        }
    }

    private void PutOnPlace()
    {
        hit = Physics2D.Raycast(ghost.transform.position, (astronautMovement.currentPlanet.transform.position - transform.position).normalized, Mathf.Infinity, mask);
        hitR = Physics2D.Raycast(ghost.transform.position + transform.right * 0.05f, (astronautMovement.currentPlanet.transform.position - ghost.transform.position).normalized, Mathf.Infinity, mask);
        hitL = Physics2D.Raycast(ghost.transform.position - transform.right * 0.05f, (astronautMovement.currentPlanet.transform.position - ghost.transform.position).normalized, Mathf.Infinity, mask);

        float mouseposX = mainCamera.ScreenToWorldPoint(Input.mousePosition).x;
        float mouseposY = mainCamera.ScreenToWorldPoint(Input.mousePosition).y;
        mouseposX = Mathf.Clamp(mouseposX, astronaut.transform.position.x - 0.4f, astronaut.transform.position.x + 0.4f);
        mouseposY = Mathf.Clamp(mouseposY, astronaut.transform.position.y - 0.4f, astronaut.transform.position.y + 0.4f);
        Vector3 mousepos = new Vector3(mouseposX, mouseposY, 0f);
        ghost.transform.position = astronautMovement.currentPlanet.transform.position + ((mousepos - astronautMovement.currentPlanet.transform.position).normalized * (Vector3.Distance(hit.point, astronautMovement.currentPlanet.transform.position) + GetComponentInChildren<Renderer>().bounds.size.y / 2));
        ghost.transform.localRotation = Quaternion.LookRotation(ghost.transform.forward, Vector2.Perpendicular(hitL.point - hitR.point));

        if (ghost.GetComponent<Scr_OreExtractor>().recolectable)
        {
            Color color = ghost.GetComponentInChildren<Renderer>().material.color;
            color.g = 250;
            color.b = 0;
            color.r = 0;
            color.a = 0.6f;
            ghost.GetComponentInChildren<Renderer>().material.color = color;

            if (Input.GetMouseButtonDown(0))
            {
                transform.SetParent(null);
                playerCheck.SetActive(true);
                transform.position = ghost.transform.position;
                transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, transform.rotation.z));
                transform.rotation = Quaternion.LookRotation(transform.forward, (transform.position - astronautMovement.currentPlanet.transform.position));
                transform.SetParent(astronautMovement.currentPlanet.transform);
                onHands = false;
                placing = false;
                astronaut.GetComponent<Scr_AstronautStats>().toolSlots[astronaut.GetComponent<Scr_AstronautsActions>().numberToolActive] = null;
                astronaut.GetComponent<Scr_AstronautStats>().physicToolSlots[astronaut.GetComponent<Scr_AstronautsActions>().numberToolActive] = null;
                astronaut.GetComponent<Scr_AstronautsActions>().BoolControl();
                oreZone = ghost.GetComponent<Scr_OreExtractor>().oreZone;
                resource = oreZone.GetComponent<Scr_OreZone>().currentResource;
                resourceLeft = (int)oreZone.GetComponent<Scr_OreZone>().amount;
                Destroy(ghost);
            }
        }

        else
        {
            Color color = ghost.GetComponentInChildren<Renderer>().material.color;
            color.g = 0;
            color.b = 0;
            color.r = 250;
            color.a = 0.6f;
            ghost.GetComponentInChildren<Renderer>().material.color = color;
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

    public override void OnMouseEnter()
    {
        showInterface = true;
    }

    public override void OnMouseExit()
    {
        showInterface = false;
    }

    private void Interface()
    {
        resourceCanvas.SetActive(true);
        nameText.text = resource.name;

        if (oreZone != null)
        {
            if (oreZone.GetComponent<Scr_OreZone>().amount > 0)
                remainingResources.text = "Remaining   " + ((int)oreZone.GetComponent<Scr_OreZone>().amount + 1);

            else
                remainingResources.text = "Remaining   " + (int)oreZone.GetComponent<Scr_OreZone>().amount;
        }

        harvestedResources.text = "Harvested    " + resourceAmount;
        harvestProcess.value = process / 3 * 100;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "OreZone")
        {
            recolectable = true;
            oreZone = collision.gameObject;
            resource = oreZone.GetComponent<Scr_OreZone>().currentResource;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "OreZone")
        {
            recolectable = false;
            oreZone = null;
        }
    }
}
