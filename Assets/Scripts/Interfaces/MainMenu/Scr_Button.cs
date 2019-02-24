using UnityEngine;

public class Scr_Button : MonoBehaviour
{
    [Header("Blocked Status")]
    [SerializeField] public bool beenDiscovered;

    [Header("Button Type")]
    [SerializeField] private ButtonType buttonType;

    [Header("Camera Parameters")]
    [SerializeField] public float xPos;
    [SerializeField] public float yPos;
    [SerializeField] public float zoom;
    [SerializeField] public float zoomSpeed;
    [SerializeField] public float movementSpeed;

    [Header("Planet List")]
    [SerializeField] private GameObject[] planets;

    [Header("References")]
    [SerializeField] private Scr_SystemSelectionManager systemSelectionManager;
    [SerializeField] private GameObject systemInfoPanel;
    [SerializeField] private GameObject systemIndicator;
    [SerializeField] private GameObject groupIndicator;
    [SerializeField] private GameObject discovered;
    [SerializeField] private GameObject notDiscovered;

    private CircleCollider2D circleCollider;

    private enum ButtonType
    {
        System,
        Group
    }

    private void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
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
        if (beenDiscovered)
        {
            if (buttonType == ButtonType.Group)
            {
                PlanetActivation(true);
                groupIndicator.SetActive(true);
            }

            else if (buttonType == ButtonType.System)
            {
                PlanetActivation(true);
                systemIndicator.SetActive(true);
            }

            if (Input.GetMouseButtonDown(0))
                ClickEvent();
        }
    }

    private void OnMouseExit()
    {
        if (beenDiscovered)
        {
            if (buttonType == ButtonType.Group)
            {
                PlanetActivation(false);
                groupIndicator.SetActive(false);
            }

            else if (buttonType == ButtonType.System)
            {
                PlanetActivation(false);
                systemIndicator.SetActive(false);
            }
        }
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
            case ButtonType.Group:
                systemSelectionManager.interfaceLevel = Scr_SystemSelectionManager.InterfaceLevel.Group;
                systemSelectionManager.savedZoom = zoom;
                systemSelectionManager.savedPos = new Vector3(transform.position.x + xPos, transform.position.y + yPos, -100); ;
                break;
        }
    }

    private void UpdateComponents()
    {
        if (buttonType == ButtonType.Group)
        {
            if (systemSelectionManager.interfaceLevel == Scr_SystemSelectionManager.InterfaceLevel.Galaxy)
                circleCollider.enabled = true;

            else
                circleCollider.enabled = false;
        }

        else if (buttonType == ButtonType.System)
        {
            if (systemSelectionManager.interfaceLevel == Scr_SystemSelectionManager.InterfaceLevel.Group)
                circleCollider.enabled = true;

            else
                circleCollider.enabled = false;

            if (systemSelectionManager.interfaceLevel == Scr_SystemSelectionManager.InterfaceLevel.Group)
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