using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Button : MonoBehaviour
{
    [Header("Button Type")]
    [SerializeField] private ButtonType buttonType;

    [Header("Camera Parameters")]
    [SerializeField] public float xPos;
    [SerializeField] public float yPos;
    [SerializeField] public float zoom;

    [Header("References")]
    [SerializeField] private Scr_SystemSelectionManager systemSelectionManager;

    private BoxCollider boxCollider;

    private enum ButtonType
    {
        System,
        Group,
        Back
    }

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        UpdateCollider();
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            systemSelectionManager.currentPos = new Vector3(transform.position.x + xPos, transform.position.y + yPos, -100);
            systemSelectionManager.currentZoom = zoom;

            switch (buttonType)
            {
                case ButtonType.System:
                    systemSelectionManager.interfaceLevel = Scr_SystemSelectionManager.InterfaceLevel.System;
                    break;
                case ButtonType.Group:
                    systemSelectionManager.interfaceLevel = Scr_SystemSelectionManager.InterfaceLevel.Group;
                    break;
                case ButtonType.Back:
                    if (systemSelectionManager.interfaceLevel == Scr_SystemSelectionManager.InterfaceLevel.System)
                        systemSelectionManager.interfaceLevel = Scr_SystemSelectionManager.InterfaceLevel.Group;
                    else if (systemSelectionManager.interfaceLevel == Scr_SystemSelectionManager.InterfaceLevel.Group)
                        systemSelectionManager.interfaceLevel = Scr_SystemSelectionManager.InterfaceLevel.Galaxy;
                    break;
            }
        }
    }

    private void UpdateCollider()
    {
        switch (buttonType)
        {
            case ButtonType.System:
                if (systemSelectionManager.interfaceLevel == Scr_SystemSelectionManager.InterfaceLevel.Group)
                    boxCollider.enabled = true;
                else
                    boxCollider.enabled = false;
                    break;
            case ButtonType.Group:
                if (systemSelectionManager.interfaceLevel == Scr_SystemSelectionManager.InterfaceLevel.Galaxy)
                    boxCollider.enabled = true;
                else
                    boxCollider.enabled = false;
                break;
        }
    }
}