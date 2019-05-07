using UnityEngine;

public class Scr_Button : MonoBehaviour
{
    [Header("Blocked Status")]
    [SerializeField] public bool beenDiscovered;

    [Header("Button Type")]
    [SerializeField] private ButtonType buttonType;
    [SerializeField] private Scr_Levels.LevelToLoad targetSystem;

    [Header("Camera Parameters")]
    [SerializeField] public float xPos;
    [SerializeField] public float yPos;
    [SerializeField] public float zoom;
    [SerializeField] public float zoomSpeed;
    [SerializeField] public float movementSpeed;

    [Header("Planet List")]
    [SerializeField] private GameObject[] planets;

    [Header("References (All)")]
    [SerializeField] private Scr_SystemSelectionManager systemSelectionManager;
    [SerializeField] private Animator indicatorsAnim;

    [Header("References (System)")]
    [SerializeField] private Animator panels;
    [SerializeField] private GameObject discovered;
    [SerializeField] private GameObject notDiscovered;

    private bool delayDone;
    private float delay = 1;
    private Animator anim;
    private CircleCollider2D circleCollider;

    private enum ButtonType
    {
        System,
        Galaxy
    }

    private void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();

        if (buttonType == ButtonType.Galaxy)
            anim = GetComponent<Animator>();

        else if (buttonType == ButtonType.System)
            anim = GetComponentInParent<Animator>();
    }

    private void Update()
    {
        UpdateComponents();
        SystemButton();
    }

    private void OnMouseOver()
    {
        if (buttonType == ButtonType.Galaxy)
        {
            anim.SetBool("ZoomGalaxy", true);
            PlanetActivation(true);

            if (indicatorsAnim != null)
                indicatorsAnim.SetBool("ShowAll", true);
        }

        else if (buttonType == ButtonType.System)
        {
            anim.SetBool(targetSystem.ToString(), true);
            PlanetActivation(true);

            if (indicatorsAnim != null)
                indicatorsAnim.SetBool(targetSystem.ToString(), true);
        }

        if (Input.GetMouseButtonDown(0))
            ClickEvent();
    }

    private void OnMouseExit()
    {
        if (buttonType == ButtonType.Galaxy)
        {
            anim.SetBool("ZoomGalaxy", false);
            PlanetActivation(false);

            if (indicatorsAnim != null)
                indicatorsAnim.SetBool("ShowAll", false);
        }

        else if (buttonType == ButtonType.System)
        {
            anim.SetBool(targetSystem.ToString(), false);
            PlanetActivation(false);

            if (indicatorsAnim != null)
                indicatorsAnim.SetBool(targetSystem.ToString(), false);
        }
    }

    private void SystemButton()
    {
        if (buttonType == ButtonType.System)
        {
            panels.SetBool("Discovered", beenDiscovered);
            discovered.SetActive(beenDiscovered);
            notDiscovered.SetActive(!beenDiscovered);

            if (systemSelectionManager.interfaceLevel == Scr_SystemSelectionManager.InterfaceLevel.Galaxy && !delayDone)
            {
                circleCollider.enabled = false;

                delay -= Time.deltaTime;

                if (delay <= 0)
                {
                    circleCollider.enabled = true;
                    delayDone = true;
                    delay = 1;
                }
            }

            if (systemSelectionManager.interfaceLevel == Scr_SystemSelectionManager.InterfaceLevel.System && Input.GetMouseButtonDown(1))
                delayDone = false;
        }
    }

    private void ClickEvent()
    {
        systemSelectionManager.currentPos = new Vector3(transform.position.x + xPos, transform.position.y + yPos, -100);
        systemSelectionManager.currentZoom = zoom;
        systemSelectionManager.currentZoomSpeed = zoomSpeed;
        systemSelectionManager.currentMovementSpeed = movementSpeed;

        anim.SetBool("ZoomGalaxy", false);
        anim.SetBool("ZoomSystem1", false);

        switch (buttonType)
        {
            case ButtonType.System:
                systemSelectionManager.interfaceLevel = Scr_SystemSelectionManager.InterfaceLevel.System;
                panels.SetBool("Show", true);
                break;
            case ButtonType.Galaxy:
                systemSelectionManager.interfaceLevel = Scr_SystemSelectionManager.InterfaceLevel.Galaxy;
                systemSelectionManager.savedZoom = zoom;
                systemSelectionManager.savedPos = new Vector3(transform.position.x + xPos, transform.position.y + yPos, -100); ;
                break;
        }
    }

    private void UpdateComponents()
    {
        if (buttonType == ButtonType.Galaxy)
        {
            if (systemSelectionManager.interfaceLevel == Scr_SystemSelectionManager.InterfaceLevel.Initial)
                circleCollider.enabled = true;

            else
                circleCollider.enabled = false;
        }

        else if (buttonType == ButtonType.System)
        {
            if (systemSelectionManager.interfaceLevel == Scr_SystemSelectionManager.InterfaceLevel.Galaxy)
                circleCollider.enabled = true;

            else
                circleCollider.enabled = false;

            if (systemSelectionManager.interfaceLevel == Scr_SystemSelectionManager.InterfaceLevel.Galaxy)
                panels.SetBool("Show", false);

            if (panels.GetBool("Show"))
                PlanetActivation(true);

            else if (systemSelectionManager.interfaceLevel == Scr_SystemSelectionManager.InterfaceLevel.System)
                PlanetActivation(false);
        }
    }

    private void PlanetActivation(bool activate)
    {
        foreach (GameObject planet in planets)
        {
            planet.GetComponent<Scr_SimpleRotation>().enabled = activate;
        }
    }
}