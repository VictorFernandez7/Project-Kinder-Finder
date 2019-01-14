﻿using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Scr_InterfaceManager : MonoBehaviour
{
    [Header("Interface Inputs")]
    [SerializeField] private KeyCode input_QuestPanel;

    [Header("PlayerShip Window")]
    [SerializeField] private Color active;
    [SerializeField] private Color notActive;

    [Header("Buttons")]
    [SerializeField] private Color interactable;
    [SerializeField] private Color notInteractable;

    [Header("References")]
    [SerializeField] private GameObject landingInterface;
    [SerializeField] private GameObject playerShipIcon;
    [SerializeField] private GameObject angleIcon;
    [SerializeField] private Animator anim_TakingOffIcon;
    [SerializeField] private Animator anim_LandingIcon;
    [SerializeField] private Animator anim_FuelIcon;
    [SerializeField] private Animator anim_MiningIcon;
    [SerializeField] private Animator anim_DangerIcon;
    [SerializeField] private Animator anim_PlanetIcon;
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private TextMeshProUGUI upgradePanelText;
    [SerializeField] private GameObject warehousePanel;
    [SerializeField] private TextMeshProUGUI warehousePanelText;
    [SerializeField] private GameObject craftingPanel;
    [SerializeField] private TextMeshProUGUI craftingPanelText;
    [SerializeField] private GameObject ToolWarehousePanel;
    [SerializeField] private Image ToolWarehousePanelIcon;
    [SerializeField] private GameObject resourceWarehousePanel;
    [SerializeField] private Image resourceWarehousePanelIcon;
    [SerializeField] private Animator anim_AstronautInterface;
    [SerializeField] private Animator anim_PlayerShipInterface;
    [SerializeField] private Animator anim_PlayerShipActions;
    [SerializeField] private Animator anim_QuestPanel;
    [SerializeField] private Animator anim_PlayerShipWindow;
    [SerializeField] private Animator anim_FadeImage;
    [SerializeField] private Animator anim_MiniMapPanel;
    [SerializeField] private GameObject playerShip;
    [SerializeField] private Scr_MainCamera mainCamera;    
    [SerializeField] private Button purchaseButton;
    [SerializeField] private Image purchaseButtonImage;
    [SerializeField] private Button craftButton;
    [SerializeField] private Image craftButtonImage;

    private bool questPanelActive;
    private bool playerShipWindowActive;
    
    private Scr_PlayerShipMovement playerShipMovement;
    private Scr_PlayerShipStats playerShipStats;

    private void Start()
    {
        playerShipMovement = playerShip.GetComponent<Scr_PlayerShipMovement>();
        playerShipStats = playerShip.GetComponent<Scr_PlayerShipStats>();

        anim_AstronautInterface.SetBool("Show", true);
        anim_FadeImage.SetBool("Fade", true);
        anim_MiniMapPanel.SetBool("Show", true);
    }

    private void Update()
    {
        LandingInterface();
        IndicatorManagement();
        ChangeButtonColor();

        if (playerShipMovement.astronautOnBoard)
            CheckInputs();

        if (!playerShipWindowActive)
            CheckAstronautState();
    }

    public void PlayerShipWindow()
    {
        playerShipWindowActive = !playerShipWindowActive;
        anim_PlayerShipWindow.SetBool("Show", playerShipWindowActive);

        if (playerShipWindowActive)
        {
            anim_AstronautInterface.SetBool("Show", false);
            anim_MiniMapPanel.SetBool("Show", false);
        }

        else
        {
            anim_AstronautInterface.SetBool("Show", true);      
            anim_MiniMapPanel.SetBool("Show", true);
        }
    }

    private void CheckAstronautState()
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
    }

    private void ChangeButtonColor()
    {
        if (upgradePanel.activeInHierarchy)
            upgradePanelText.color = active;

        else
            upgradePanelText.color = notActive;

        if (warehousePanel.activeInHierarchy)
            warehousePanelText.color = active;

        else
            warehousePanelText.color = notActive;

        if (craftingPanel.activeInHierarchy)
            craftingPanelText.color = active;

        else
            craftingPanelText.color = notActive;

        if (ToolWarehousePanel.activeInHierarchy)
            ToolWarehousePanelIcon.color = active;

        else
            ToolWarehousePanelIcon.color = notActive;

        if (resourceWarehousePanel.activeInHierarchy)
            resourceWarehousePanelIcon.color = active;

        else
            resourceWarehousePanelIcon.color = notActive;

        if (purchaseButton.interactable)
            purchaseButtonImage.color = interactable;

        else
            purchaseButtonImage.color = notInteractable;

        if (craftButton.interactable)
            craftButtonImage.color = interactable;

        else
            craftButtonImage.color = notInteractable;
    }

    private void LandingInterface()
    {
        if (playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.landing)
        {
            landingInterface.SetActive(true);

            float angle = Vector2.SignedAngle((playerShipMovement.currentPlanet.transform.position - playerShip.transform.position), playerShip.transform.up);
            playerShipIcon.transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        else
            landingInterface.SetActive(false);
    }

    private void IndicatorManagement()
    {
        if (playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.takingOff)
            anim_TakingOffIcon.SetBool("TurnOn", true);

        else
            anim_TakingOffIcon.SetBool("TurnOn", false);

        if (playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.landing)
            anim_LandingIcon.SetBool("TurnOn", true);

        else
            anim_LandingIcon.SetBool("TurnOn", false);

        if (playerShipStats.currentFuel <= (0.25f * playerShipStats.maxFuel))
            anim_FuelIcon.SetBool("TurnOn", true);

        else
            anim_FuelIcon.SetBool("TurnOn", false);

        anim_DangerIcon.SetBool("TurnOn", playerShipStats.inDanger);

        if (playerShipMovement.currentPlanet != null)
            anim_PlanetIcon.SetBool("TurnOn", true);
        
        else
            anim_PlanetIcon.SetBool("TurnOn", false);

        if (mainCamera.mining)
            anim_MiningIcon.SetBool("TurnOn", true);
        
        else
            anim_MiningIcon.SetBool("TurnOn", false);

        if (playerShipStats.currentShield <= (0.25f * playerShipStats.maxShield) && playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.landed)
            playerShipStats.inDanger = true;

        else if (playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.landed)
            playerShipStats.inDanger = false;
    }
}