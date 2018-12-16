using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Scr_InterfaceManager : MonoBehaviour
{
    [Header("Interface Inputs")]
    [SerializeField] private KeyCode input_QuestPanel;
    [SerializeField] private KeyCode input_PlayerShipWindow;

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

    private bool questPanelActive;
    private bool playerShipWindowActive;

    private Animator anim_AstronautInterface;
    private Animator anim_PlayerShipInterface;
    private Animator anim_PlayerShipActions;
    private Animator anim_QuestPanel;
    private Animator anim_PlayerShipWindow;
    private Animator anim_FadeImage;

    private GameObject playerShip;
    private Scr_MainCamera mainCamera;
    private Scr_PlayerShipMovement playerShipMovement;
    private Scr_PlayerShipStats playerShipStats;

    private void Start()
    {
        playerShip = GameObject.Find("PlayerShip");
        anim_AstronautInterface = GameObject.Find("AstronautInterface").GetComponent<Animator>();
        anim_PlayerShipInterface = GameObject.Find("PlayerShipInterface").GetComponent<Animator>();
        anim_PlayerShipActions = GameObject.Find("PlayerShipActions").GetComponent<Animator>();
        anim_QuestPanel = GameObject.Find("QuestPanel").GetComponent<Animator>();
        anim_PlayerShipWindow = GameObject.Find("PlayerShipWindow").GetComponent<Animator>();
        anim_FadeImage = GameObject.Find("FadeImage").GetComponent<Animator>();
        mainCamera = GameObject.Find("MainCamera").GetComponent<Scr_MainCamera>();

        playerShipMovement = playerShip.GetComponent<Scr_PlayerShipMovement>();
        playerShipStats = playerShip.GetComponent<Scr_PlayerShipStats>();

        anim_AstronautInterface.SetBool("Show", true);
        anim_FadeImage.SetBool("Fade", true);
    }

    private void Update()
    {
        CheckAstronautState();
        LandingInterface();
        IndicatorManagement();
        ChangeTopButtonColor();

        if (playerShipMovement.astronautOnBoard)
            CheckInputs();
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

        if (Input.GetKeyDown(input_PlayerShipWindow))
        {
            playerShipWindowActive = !playerShipWindowActive;
            anim_PlayerShipWindow.SetBool("Show", playerShipWindowActive);
        }
    }

    private void ChangeTopButtonColor()
    {
        if (upgradePanel.activeInHierarchy)
            upgradePanelText.color = Color.white;

        else
            upgradePanelText.color = Color.black;

        if (warehousePanel.activeInHierarchy)
            warehousePanelText.color = Color.white;

        else
            warehousePanelText.color = Color.black;

        if (craftingPanel.activeInHierarchy)
            craftingPanelText.color = Color.white;

        else
            craftingPanelText.color = Color.black;
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
    }
}