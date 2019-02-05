using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_SystemSelectionManager : MonoBehaviour
{
    [Header("Input Settings")]
    [SerializeField] private KeyCode backCode;

    [Header("References")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject titleText;

    [HideInInspector] public float currentZoom;
    [HideInInspector] public float savedZoom;
    [HideInInspector] public float currentZoomSpeed;
    [HideInInspector] public float currentMovementSpeed;
    [HideInInspector] public Vector3 currentPos;
    [HideInInspector] public Vector3 savedPos;
    [HideInInspector] public InterfaceLevel interfaceLevel;

    private float initialZoom;
    private Vector3 initialPos;

    public enum InterfaceLevel
    {
        Galaxy,
        Group,
        System
    }

    private void Start()
    {
        initialPos = mainCamera.transform.position;
        initialZoom = mainCamera.orthographicSize;
        currentZoom = initialZoom;
        savedZoom = initialZoom;

        currentPos = initialPos;
    }

    private void Update()
    {
        UpdateCamera();
        CheckInput();

        if (interfaceLevel == InterfaceLevel.Galaxy)
            titleText.SetActive(true);

        else
            titleText.SetActive(false);
    }

    private void UpdateCamera()
    {
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, currentZoom, Time.deltaTime * currentZoomSpeed);
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, currentPos, Time.deltaTime * currentMovementSpeed);
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(backCode))
        {
            if (interfaceLevel == Scr_SystemSelectionManager.InterfaceLevel.System)
            {
                interfaceLevel = Scr_SystemSelectionManager.InterfaceLevel.Group;
                currentZoom = savedZoom;
                currentPos = savedPos;
            }

            else if (interfaceLevel == Scr_SystemSelectionManager.InterfaceLevel.Group)
            {
                interfaceLevel = Scr_SystemSelectionManager.InterfaceLevel.Galaxy;
                currentZoom = initialZoom;
                currentPos = initialPos;
            }
        }
    }
}