using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Tool : MonoBehaviour {

    [Header("Tool Info")]
    [SerializeField] public string toolName;

    [HideInInspector] public bool onHands;
    [HideInInspector] public Camera mainCamera;

    private bool placing;
    private bool onRange;
    private GameObject gosht;
    private Scr_AstronautMovement astronautMovement;
    private GameObject astronaut;

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
    }

    public void UseTool()
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
        float mouseposX = mainCamera.ScreenToWorldPoint(Input.mousePosition).x;
        float mouseposY = mainCamera.ScreenToWorldPoint(Input.mousePosition).y;
        mouseposX = Mathf.Clamp(mouseposX, astronaut.transform.position.x - 0.5f, astronaut.transform.position.x + 0.5f);
        mouseposY = Mathf.Clamp(mouseposY, astronaut.transform.position.y - 0.1f, astronaut.transform.position.y + 0.1f);
        Vector3 mousepos = new Vector3(mouseposX, mouseposY, 0f);
        gosht.transform.position = astronautMovement.currentPlanet.transform.position + ((mousepos - astronautMovement.currentPlanet.transform.position).normalized * (astronautMovement.currentPlanet.transform.position - astronaut.transform.position).magnitude);
        gosht.transform.rotation = Quaternion.LookRotation(transform.forward, (gosht.transform.position - astronautMovement.currentPlanet.transform.position));
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
