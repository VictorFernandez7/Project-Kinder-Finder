﻿using UnityEngine;

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
    [SerializeField] private GameObject panels;
    [SerializeField] private GameObject discovered;
    [SerializeField] private GameObject discoveredPanel;
    [SerializeField] private GameObject notDiscovered;
    [SerializeField] private GameObject notDiscoveredPanel;

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

        // Provisional:

        if (this.gameObject.name == "Galaxy1")
            ClickEvent();
    }

    private void Update()
    {
        UpdateComponents();

        if (buttonType == ButtonType.System)
        {
            discovered.SetActive(beenDiscovered);
            discoveredPanel.SetActive(beenDiscovered);
            notDiscovered.SetActive(!beenDiscovered);
            notDiscoveredPanel.SetActive(!beenDiscovered);
        }
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
                panels.SetActive(true);
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
                panels.SetActive(false);

            if (panels.activeInHierarchy)
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