using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_TravelGate : MonoBehaviour
{
    [Header("Select Galaxy")]
    [SerializeField] private Scr_Levels.LevelToLoad targetGalaxy;

    [Header("References")]
    [SerializeField] private GameObject travelGateCanvas;
    [SerializeField] private Scr_SystemSelectionManager systemSelectionManager;

    private CircleCollider2D circleCollider;

    private void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        UpdateCollider();
    }

    private void OnMouseOver()
    {
        if (systemSelectionManager.interfaceLevel == Scr_SystemSelectionManager.InterfaceLevel.System)
        {
            travelGateCanvas.SetActive(true);

            if (Input.GetMouseButtonDown(0))
                Scr_LevelManager.LoadPlanetSystem(targetGalaxy);
        }
    }

    private void OnMouseExit()
    {
        travelGateCanvas.SetActive(false);
    }

    private void UpdateCollider()
    {
        if (systemSelectionManager.interfaceLevel == Scr_SystemSelectionManager.InterfaceLevel.System)
            circleCollider.enabled = true;

        else
            circleCollider.enabled = false;
    }
}