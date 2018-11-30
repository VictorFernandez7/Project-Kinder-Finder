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
    [SerializeField] private GameObject landingInterfaceShip;

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

        anim_AstronautInterface.SetBool("Show", true);
        anim_FadeImage.SetBool("Fade", true);
    }

    private void Update()
    {
        CheckAstronautState();
        LandingInterface();

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

    public void ChangeTextColor(TextMeshProUGUI currentSelected)
    {
        if (currentSelected.color == Color.black)
            currentSelected.color = Color.white;

        else
            currentSelected.color = Color.black;
    }

    private void LandingInterface()
    {
        if (playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.landing)
        {
            landingInterface.SetActive(true);

            Vector3 direction = new Vector3(landingInterface.transform.position.x - playerShip.transform.position.x, landingInterface.transform.position.y - playerShip.transform.position.y, landingInterface.transform.position.z - playerShip.transform.position.z);

            landingInterfaceShip.transform.up = direction;
        }

        else
            landingInterface.SetActive(false);

    }
}