﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Scr_InterfaceManager : MonoBehaviour
{
    [Header("Interface Inputs")]
    [SerializeField] private KeyCode input_QuestPanel;
    [SerializeField] private KeyCode input_PlayerShipWindow;

    private bool questPanelActive;
    private bool playerShipWindowActive;

    private Animator anim_AstronautInterface;
    private Animator anim_PlayerShipInterface;
    private Animator anim_PlayerShipActions;
    private Animator anim_QuestPanel;
    private Animator anim_PlayerShipWindow;
    private Animator anim_FadeImage;

    private Scr_MainCamera mainCamera;
    private Scr_PlayerShipMovement playerShipMovement;

    private void Start()
    {
        anim_AstronautInterface = GameObject.Find("AstronautInterface").GetComponent<Animator>();
        anim_PlayerShipInterface = GameObject.Find("PlayerShipInterface").GetComponent<Animator>();
        anim_PlayerShipActions = GameObject.Find("PlayerShipActions").GetComponent<Animator>();
        anim_QuestPanel = GameObject.Find("QuestPanel").GetComponent<Animator>();
        anim_PlayerShipWindow = GameObject.Find("PlayerShipWindow").GetComponent<Animator>();
        anim_FadeImage = GameObject.Find("FadeImage").GetComponent<Animator>();

        mainCamera = GameObject.Find("MainCamera").GetComponent<Scr_MainCamera>();
        playerShipMovement = GameObject.Find("PlayerShip").GetComponent<Scr_PlayerShipMovement>();

        anim_AstronautInterface.SetBool("Show", true);
        anim_FadeImage.SetBool("Fade", true);
    }

    private void Update()
    {
        CheckPlayerState();

        if (playerShipMovement.astronautOnBoard)
            CheckInputs();
    }

    private void CheckPlayerState()
    {
        anim_PlayerShipActions.SetBool("Mining", mainCamera.mining);

        if (playerShipMovement.astronautOnBoard)
        {
            anim_AstronautInterface.SetBool("Show", false);
            anim_PlayerShipInterface.SetBool("Show", true);
            anim_PlayerShipActions.SetBool("InsideShip", true);
        }

        else
        {
            anim_AstronautInterface.SetBool("Show", true);
            anim_PlayerShipInterface.SetBool("Show", false);
            anim_PlayerShipActions.SetBool("InsideShip", false);
        }
    }

    private void CheckInputs()
    {
        if (Input.GetKeyDown(input_QuestPanel))
        {
            questPanelActive = !questPanelActive;
            anim_QuestPanel.SetBool("Show", questPanelActive);
        }

        if (Input.GetKeyDown(input_PlayerShipWindow))
        {
            playerShipWindowActive = !playerShipWindowActive;
            anim_PlayerShipWindow.SetBool("Show", playerShipWindowActive);
        }
    }
}