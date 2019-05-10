using UnityEngine;
using TMPro;

public class Scr_InterfaceManager : MonoBehaviour
{
    [Header("Main Canvas Tooltips")]
    [SerializeField] public bool tooltipsOn;

    [Header("Interface Inputs")]
    [SerializeField] private KeyCode input_PauseMenu;

    [Header("References")]
    [SerializeField] private Animator anim_AstronautInterface;
    [SerializeField] private Animator anim_PlayerShipInterface;
    [SerializeField] private Animator anim_PlayerShipActions;
    [SerializeField] private Animator anim_PlayerShipWindow;
    [SerializeField] private Animator anim_FadeImage;
    [SerializeField] private Animator anim_MiniMapPanel;
    [SerializeField] private Animator anim_XPPanel;
    [SerializeField] private GameObject landingInterface;
    [SerializeField] private GameObject playerShipIcon;
    [SerializeField] private GameObject pauseCanvas;
    [SerializeField] private GameObject playerShip;
    [SerializeField] private GameObject angleIcon;
    [SerializeField] private GameObject astronaut;
    [SerializeField] private Scr_MainCamera mainCamera;
    [SerializeField] private TextMeshProUGUI planetName;

    [HideInInspector] public bool gamePaused;
    [HideInInspector] public bool interacting;

    private bool playerShipWindowActive;
    private Scr_PlayerShipMovement playerShipMovement;
    private Scr_PlayerShipStats playerShipStats;

    private void Start()
    {
        playerShipMovement = playerShip.GetComponent<Scr_PlayerShipMovement>();
        playerShipStats = playerShip.GetComponent<Scr_PlayerShipStats>();

        anim_AstronautInterface.SetBool("Show", true);
        anim_FadeImage.SetBool("Fade", true);
    }

    private void Update()
    {
        AstronautInterfaceInfoUpdate();
        PlayerShipWindowAvailable();
        LandingInterface();
        CheckInputs();

        if (!interacting)
        {
            if (playerShipWindowActive)
            {
                anim_MiniMapPanel.SetBool("Show", false);
                anim_XPPanel.SetBool("Show", false);
            }

            else
            {
                CheckAstronautState();
                anim_MiniMapPanel.SetBool("Show", true);
                anim_XPPanel.SetBool("Show", true);
            }
        }
    }

    public void PlayerShipWindow()
    {
        playerShipWindowActive = !playerShipWindowActive;
        anim_PlayerShipWindow.SetBool("Show", playerShipWindowActive);

        if (playerShipWindowActive)
            ClearInterface(true);

        else
            ClearInterface(false);
    }

    private void PlayerShipWindowAvailable()
    {
        if (playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.landed && playerShipMovement.astronautOnBoard)
            anim_PlayerShipWindow.SetBool("Available", true);

        else
            anim_PlayerShipWindow.SetBool("Available", false);
    }

    private void AstronautInterfaceInfoUpdate()
    {
        if (playerShipMovement.currentPlanet != null)
            planetName.text = playerShipMovement.currentPlanet.GetComponent<Scr_Planet>().planetName;
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
        if (Input.GetKeyDown(input_PauseMenu))
            PauseGame(true);
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

    public void ClearInterface(bool clear)
    {
        if (interacting)
        {
            anim_AstronautInterface.SetBool("Show", !clear);
            anim_MiniMapPanel.SetBool("Show", !clear);
            anim_XPPanel.SetBool("Show", !clear);
        }

        else
        {
            anim_AstronautInterface.SetBool("Show", !clear);
            anim_PlayerShipInterface.SetBool("Show", !clear);
            anim_PlayerShipActions.SetBool("InsideShip", !clear);
            anim_MiniMapPanel.SetBool("Show", !clear);
            anim_XPPanel.SetBool("Show", !clear);
        }
    }

    public void PauseGame(bool pause)
    {
        if (pause)
        {
            pauseCanvas.SetActive(true);
            Time.timeScale = 0;
            gamePaused = true;
        }

        else
        {
            pauseCanvas.SetActive(false);
            Time.timeScale = 1;
            gamePaused = false;
        }
    }

    public void Exit()
    {
        Scr_LevelManager.LoadMainMenu();
    }
}