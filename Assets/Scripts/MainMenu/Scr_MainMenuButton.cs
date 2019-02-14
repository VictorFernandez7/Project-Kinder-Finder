using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class Scr_MainMenuButton : MonoBehaviour
{
    [Header("Select Button")]
    [SerializeField] private MainMenuButton mainMenuButton;

    [Header("Select Model")]
    [SerializeField] private Model model;

    [Header("Internal References")]
    [SerializeField] private Animator canvasAnim;
    [SerializeField] private Transform cameraSpot;
    [SerializeField] private GameObject visuals;
    [SerializeField] private GameObject indicator;
    [SerializeField] private TextMeshProUGUI buttonText;

    [Header("External References")]
    [SerializeField] private Scr_MainMenuManager mainMenuManager;

    [Header("Model References")]
    [SerializeField] private GameObject planet1;
    [SerializeField] private GameObject planet2;
    [SerializeField] private GameObject asteroid;

    private bool blocked;
    private Scr_ButtonVisuals buttonVisuals;

    private enum MainMenuButton
    {
        Play,
        ContinueGame,
        NewGame,
        LoadGame,
        Settings,
        AudioSettings,
        VideoSettings,
        GameSettings,
        AboutUs,
        Team,
        RRSS,
        Contact,
        Exit
    }

    private enum Model
    {
        Planet1,
        Planet2,
        Asteroid
    }

    private void Start()
    {
        SetModel();
        SetButtonName();

        buttonVisuals = GetComponentInChildren<Scr_ButtonVisuals>();
        GetComponent<SphereCollider>().radius = visuals.transform.localScale.x / 100;
        buttonText.enabled = false;
    }

    private void Update()
    {
        indicator.transform.LookAt(mainMenuManager.mainCamera.transform);

        if ((mainMenuButton == MainMenuButton.Play || mainMenuButton == MainMenuButton.Settings || mainMenuButton == MainMenuButton.AboutUs))
        {
            if (mainMenuManager.mainMenuLevel == Scr_MainMenuManager.MainMenuLevel.Main)
            {
                buttonVisuals.rotate = true;
                canvasAnim.SetBool("ShowText", false);
                blocked = true;
            }

            else
            {
                if (blocked)
                {
                    buttonVisuals.rotate = false;
                    blocked = false;
                }
            }
        }

        else
        {
            if (mainMenuManager.mainMenuLevel == Scr_MainMenuManager.MainMenuLevel.Secondary)
            {
                buttonVisuals.rotate = true;
                canvasAnim.SetBool("ShowText", false);
                blocked = true;
            }

            else
            {
                if (blocked)
                {
                    buttonVisuals.rotate = false;
                    blocked = false;
                }
            }
        }
    }

    private void OnMouseOver()
    {
        if (!blocked)
        {
            buttonText.enabled = true;
            buttonVisuals.rotate = true;
            canvasAnim.SetBool("ShowText", true);

            if (Input.GetMouseButtonDown(0))
            {
                mainMenuManager.currentCameraPos = cameraSpot.position;

                if (mainMenuButton == MainMenuButton.Play || mainMenuButton == MainMenuButton.Settings || mainMenuButton == MainMenuButton.AboutUs)
                {
                    mainMenuManager.mainMenuLevel = Scr_MainMenuManager.MainMenuLevel.Main;
                    mainMenuManager.savedMainSpot = cameraSpot.position;

                    switch (mainMenuButton)
                    {
                        case MainMenuButton.Play:
                            mainMenuManager.secondaryButtonsAnim.SetBool("Play", true);
                            break;
                        case MainMenuButton.Settings:
                            mainMenuManager.secondaryButtonsAnim.SetBool("Settings", true);
                            break;
                        case MainMenuButton.AboutUs:
                            mainMenuManager.secondaryButtonsAnim.SetBool("AboutUs", true);
                            break;
                    }
                }

                else
                {
                    mainMenuManager.mainMenuLevel = Scr_MainMenuManager.MainMenuLevel.Secondary;

                    switch (mainMenuButton)
                    {
                        case MainMenuButton.NewGame:
                            Scr_LevelManager.LoadPlanetSystem(Scr_Levels.LevelToLoad.PlanetSystem1);
                            break;
                        case MainMenuButton.AudioSettings:
                            mainMenuManager.settingsAnim.SetBool("Audio", true);
                            break;
                        case MainMenuButton.VideoSettings:
                            mainMenuManager.settingsAnim.SetBool("Video", true);
                            break;
                        case MainMenuButton.GameSettings:
                            mainMenuManager.settingsAnim.SetBool("Game", true);
                            break;
                        case MainMenuButton.RRSS:
                            mainMenuManager.aboutUsAnim.SetBool("RRSS", true);
                            break;
                        case MainMenuButton.Team:
                            mainMenuManager.aboutUsAnim.SetBool("Team", true);
                            break;
                        case MainMenuButton.Contact:
                            mainMenuManager.aboutUsAnim.SetBool("Contact", true);
                            break;
                    }
                }
            }
        }
    }

    private void OnMouseExit()
    {
        if (!blocked)
        {
            buttonVisuals.rotate = false;
            canvasAnim.SetBool("ShowText", false);
        }
    }

    private void SetButtonName()
    {
        switch (mainMenuButton)
        {
            case MainMenuButton.Play:
                buttonText.text = "PLAY";
                break;
            case MainMenuButton.ContinueGame:
                buttonText.text = "CONTINUE";
                break;
            case MainMenuButton.NewGame:
                buttonText.text = "NEW";
                break;
            case MainMenuButton.LoadGame:
                buttonText.text = "LOAD";
                break;
            case MainMenuButton.Settings:
                buttonText.text = "SETTINGS";
                break;
            case MainMenuButton.AudioSettings:
                buttonText.text = "AUDIO";
                break;
            case MainMenuButton.VideoSettings:
                buttonText.text = "VIDEO";
                break;
            case MainMenuButton.GameSettings:
                buttonText.text = "GAME";
                break;
            case MainMenuButton.AboutUs:
                buttonText.text = "ABOUT US";
                break;
            case MainMenuButton.Team:
                buttonText.text = "TEAM";
                break;
            case MainMenuButton.RRSS:
                buttonText.text = "RRSS";
                break;
            case MainMenuButton.Contact:
                buttonText.text = "CONTACT";
                break;
            case MainMenuButton.Exit:
                buttonText.text = "EXIT";
                break;
        }
    }

    private void SetModel()
    {
        switch (model)
        {
            case Model.Planet1:
                planet1.SetActive(true);
                planet2.SetActive(false);
                asteroid.SetActive(false);
                visuals = planet1;
                break;
            case Model.Planet2:
                planet1.SetActive(false);
                planet2.SetActive(true);
                asteroid.SetActive(false);
                visuals = planet2;
                break;
            case Model.Asteroid:
                planet1.SetActive(false);
                planet2.SetActive(false);
                asteroid.SetActive(true);
                visuals = asteroid;
                break;
        }
    }
}