using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_SystemSelectionManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera mainCamera;

    [HideInInspector] public float currentZoom;
    [HideInInspector] public Vector3 currentPos;
    [HideInInspector] public InterfaceLevel interfaceLevel;

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
        currentZoom = mainCamera.orthographicSize;

        currentPos = initialPos;
    }

    private void Update()
    {
        mainCamera.orthographicSize = currentZoom;
        mainCamera.transform.position = currentPos;
    }
}