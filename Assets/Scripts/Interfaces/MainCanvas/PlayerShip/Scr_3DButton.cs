using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_3DButton : MonoBehaviour
{
    [Header("Select Button Type")]
    [SerializeField] private ButtonType buttonType;

    [Header("Select System Number")]
    [Range(0, 7)] [SerializeField] private int systemNumber;

    [Header("Planet Panel")]
    [SerializeField] private float delay;

    [Header("References (All)")]
    [SerializeField] private Scr_SunButton sunButton;
    [SerializeField] private GameObject indicator;

    [Header("References (Planet)")]
    [SerializeField] private Transform cameraSpot;

    [Header("References (System)")]
    [SerializeField] private GameObject[] systems;

    private bool timerOn;
    private float savedDelay;

    private enum ButtonType
    {
        System,
        Planet
    }

    private void Start()
    {
        savedDelay = delay;
    }

    private void Update()
    {
        if (buttonType == ButtonType.Planet)
        {
            DelayTimer();
            CheckPanel();
        }
    }

    private void OnMouseOver()
    {
        indicator.SetActive(true);

        if (Input.GetMouseButtonDown(0))
        {
            if (buttonType == ButtonType.System)
            {
                sunButton.interfaceLevel = Scr_SunButton.InterfaceLevel.PlanetSelection;

                for (int i = 0; i < systems.Length; i++)
                {
                    if (i == systemNumber)
                        systems[i].SetActive(true);

                    else
                        systems[i].SetActive(false);
                }
            }

            else if (buttonType == ButtonType.Planet)
            {
                sunButton.interfaceLevel = Scr_SunButton.InterfaceLevel.PlanetInfo;

                sunButton.targetCameraPos = cameraSpot.position;
                timerOn = true;
            }
        }
    }

    private void OnMouseExit()
    {
        indicator.SetActive(false);
    }

    private void CheckPanel()
    {
        if (Input.GetMouseButtonDown(1) && sunButton.planetPanel.activeInHierarchy)
            sunButton.planetPanel.SetActive(false);
    }

    private void DelayTimer()
    {
        if (timerOn)
        {
            savedDelay -= Time.deltaTime;

            if (savedDelay <= 0)
            {
                sunButton.planetPanel.SetActive(true);
                savedDelay = delay;
                timerOn = false;
            }
        }
    }
}