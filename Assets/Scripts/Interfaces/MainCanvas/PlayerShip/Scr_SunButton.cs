using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class Scr_SunButton : MonoBehaviour
{
    [Header("Current Interface Level")]
    [SerializeField] public InterfaceLevel interfaceLevel;

    [Header("Camera Settings")]
    [SerializeField] private float cameraSpeed;

    [Header("References")]
    [SerializeField] private GameObject planets;
    [SerializeField] private GameObject systems;
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject planetFilesCamera;

    [HideInInspector] public Vector3 targetCameraPos;

    private Vector3 initialCameraPos;

    public enum InterfaceLevel
    {
        SystemSelection,
        PlanetSelection,
        PlanetInfo
    }

    private void Start()
    {
        initialCameraPos = planetFilesCamera.transform.position;
        targetCameraPos = initialCameraPos;
    }

    private void Update()
    {
        InputCheck();
        CameraPosUpdate();
        InterfaceLevelCheck();
    }

    private void InterfaceLevelCheck()
    {
        switch (interfaceLevel)
        {
            case InterfaceLevel.SystemSelection:
                anim.SetInteger("InterfaceLevel", 0);
                break;
            case InterfaceLevel.PlanetSelection:
                anim.SetInteger("InterfaceLevel", 1);
                targetCameraPos = initialCameraPos;
                break;
            case InterfaceLevel.PlanetInfo:
                // panel show
                break;
        }
    }

    private void InputCheck()
    {
        if (Input.GetMouseButtonDown(1))
        {
            switch (interfaceLevel)
            {
                case InterfaceLevel.PlanetSelection:
                    interfaceLevel = InterfaceLevel.SystemSelection;
                    break;
                case InterfaceLevel.PlanetInfo:
                    interfaceLevel = InterfaceLevel.PlanetSelection;
                    break;
            }
        }
    }

    private void CameraPosUpdate()
    {
        planetFilesCamera.transform.position = Vector3.Lerp(planetFilesCamera.transform.position, targetCameraPos, Time.deltaTime * cameraSpeed);
    }
}