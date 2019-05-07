﻿using UnityEngine;

public class Scr_3DButton : MonoBehaviour
{
    [Header("Select Button Type")]
    [SerializeField] private ButtonType buttonType;

    [Header("Set Blocked")]
    [SerializeField] private bool isBlocked;

    [Header("Select System Number")]
    [Range(0, 7)] [SerializeField] private int systemNumber;

    [Header("Select System Index")]
    [SerializeField] private int systemIndex;

    [Header("Select Planet Index")]
    [SerializeField] private int planetIndex;

    [Header("Planet Panel")]
    [SerializeField] private float delay;

    [Header("References (All)")]
    [SerializeField] private Scr_PlanetFilesPanel planetFilesPanel;
    [SerializeField] private GameObject indicator;
    [SerializeField] private GameObject notBlockedVisuals;
    [SerializeField] private GameObject blockedVisuals;
    [SerializeField] private GameObject blockedIndicator;

    [Header("References (Planet)")]
    [SerializeField] private Transform cameraSpot;
    [SerializeField] private Scr_PlanetPanel planetPanel;
    [SerializeField] private Scr_GameManager gameManager;

    [Header("References (System)")]
    [SerializeField] private Scr_FilesPanelAnimator filesPanelAnimator;
    [SerializeField] private GameObject[] systems;
    [SerializeField] private GameObject[] planets;

    private bool timerOn;
    private float savedDelay;
    private Scr_PlanetPanelInfo planetPanelInfo;

    private enum ButtonType
    {
        System,
        Planet
    }

    private void Start()
    {
        planetPanelInfo = GetComponent<Scr_PlanetPanelInfo>();

        savedDelay = delay;
    }

    private void Update()
    {
        UpdateVisuals();
        CheckIfBlocked();

        if (buttonType == ButtonType.Planet)
        {
            DelayTimer();
            CheckPanel();
        }

        if (buttonType == ButtonType.System)
            CheckAnimation();
    }

    private void OnMouseOver()
    {
        if (!isBlocked)
        {
            indicator.SetActive(true);

            if (Input.GetMouseButtonDown(0))
            {
                if (buttonType == ButtonType.System)
                {
                    planetFilesPanel.interfaceLevel = Scr_PlanetFilesPanel.InterfaceLevel.PlanetSelection;

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
                    planetFilesPanel.interfaceLevel = Scr_PlanetFilesPanel.InterfaceLevel.PlanetInfo;

                    planetPanel.UpdatePanelInfo(planetPanelInfo.planetName, planetPanelInfo.highTemp, planetPanelInfo.lowTemp, planetPanelInfo.toxic, planetPanelInfo.jetpack, planetPanelInfo.res1, planetPanelInfo.res2, planetPanelInfo.res3, planetPanelInfo.res4, planetPanelInfo.res5, planetPanelInfo.history);
                    planetFilesPanel.targetCameraPos = cameraSpot.position;
                    timerOn = true;
                }
            }
        }
    }

    private void OnMouseExit()
    {
        indicator.SetActive(false);
    }

    private void CheckPanel()
    {
        if (Input.GetMouseButtonDown(1) && planetFilesPanel.planetPanel.activeInHierarchy)
            planetFilesPanel.planetPanel.SetActive(false);
    }

    private void DelayTimer()
    {
        if (timerOn)
        {
            savedDelay -= Time.deltaTime;

            if (savedDelay <= 0)
            {
                planetFilesPanel.planetPanel.SetActive(true);
                savedDelay = delay;
                timerOn = false;
            }
        }
    }

    private void CheckAnimation()
    {
        if (filesPanelAnimator.animationPlaying)
        {
            for (int i = 0; i < planets.Length; i++)
            {
                planets[i].GetComponent<Scr_SimpleRotation>().enabled = false;
            }
        }

        else
        {
            for (int i = 0; i < planets.Length; i++)
            {
                planets[i].GetComponent<Scr_SimpleRotation>().enabled = true;
            }
        }
    }

    private void UpdateVisuals()
    {
        notBlockedVisuals.SetActive(!isBlocked);
        blockedVisuals.SetActive(isBlocked);
        blockedIndicator.SetActive(isBlocked);
    }

    private void CheckIfBlocked()
    {
        if (buttonType == ButtonType.Planet)
        {
            if (systemIndex == 0)
            {
                if (Scr_LevelManager.system1Info[planetIndex] == false)
                    isBlocked = true;

                else
                    isBlocked = false;
            }

            else
            {
                if (Scr_LevelManager.system2Info[planetIndex] == false)
                    isBlocked = true;

                else
                    isBlocked = false;
            }
        }

        else if (buttonType == ButtonType.System)
        {
            if (Scr_LevelManager.galaxyInfo[systemIndex] == false)
                isBlocked = true;

            else
                isBlocked = false;
        }
    }
}