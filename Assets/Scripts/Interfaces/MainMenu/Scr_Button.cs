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

    [Header("References (System)")]
    [SerializeField] private GameObject systemInfoPanel;
    [SerializeField] private GameObject discovered;
    [SerializeField] private GameObject notDiscovered;

    [Header("References (Galaxy)")]
    [SerializeField] private Animator indicatorsAnim;

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
    }

    private void Update()
    {
        UpdateComponents();

        if (buttonType == ButtonType.System)
        {
            discovered.SetActive(beenDiscovered);
            notDiscovered.SetActive(!beenDiscovered);
        }
    }

    private void OnMouseOver()
    {
        if (buttonType == ButtonType.Galaxy)
        {
            anim.SetBool("Zoom", true);
            PlanetActivation(true);

            if (indicatorsAnim != null)
                indicatorsAnim.SetBool("Show", true);
        }

        else if (buttonType == ButtonType.System)
            PlanetActivation(true);

        if (Input.GetMouseButtonDown(0))
            ClickEvent();
    }

    private void OnMouseExit()
    {
        if (buttonType == ButtonType.Galaxy)
        {
            anim.SetBool("Zoom", false);
            PlanetActivation(false);

            if (indicatorsAnim != null)
                indicatorsAnim.SetBool("Show", false);
        }

        else if (buttonType == ButtonType.System)
            PlanetActivation(false);
    }

    private void ClickEvent()
    {
        systemSelectionManager.currentPos = new Vector3(transform.position.x + xPos, transform.position.y + yPos, -100);
        systemSelectionManager.currentZoom = zoom;
        systemSelectionManager.currentZoomSpeed = zoomSpeed;
        systemSelectionManager.currentMovementSpeed = movementSpeed;

        switch (buttonType)
        {
            case ButtonType.System:
                systemSelectionManager.interfaceLevel = Scr_SystemSelectionManager.InterfaceLevel.System;
                systemInfoPanel.SetActive(true);
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
                systemInfoPanel.SetActive(false);

            if (systemInfoPanel.activeInHierarchy)
                PlanetActivation(true);
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