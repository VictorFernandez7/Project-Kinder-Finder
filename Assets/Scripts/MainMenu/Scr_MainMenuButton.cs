using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;

public class Scr_MainMenuButton : MonoBehaviour
{
    [Header("Select Button")]
    [SerializeField] private MainMenuButton mainMenuButton;

    [Header("Internal References")]
    [SerializeField] private Animator canvasAnim;
    [SerializeField] private Transform cameraSpot;
    [SerializeField] private GameObject visuals;
    [SerializeField] private GameObject indicator;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private Scr_MainMenuManager mainMenuManager;

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
        Exit
    }

    private void Start()
    {
        buttonVisuals = GetComponentInChildren<Scr_ButtonVisuals>();
        GetComponent<SphereCollider>().radius = visuals.transform.localScale.x / 100;
        buttonText.enabled = false;

        SetButtonName();
    }

    private void Update()
    {
        indicator.transform.LookAt(mainMenuManager.mainCamera.transform);
    }

    private void OnMouseOver()
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
            }

            else
                mainMenuManager.mainMenuLevel = Scr_MainMenuManager.MainMenuLevel.Secondary;
        }
    }

    private void OnMouseExit()
    {
        buttonVisuals.rotate = false;
        canvasAnim.SetBool("ShowText", false);
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
            case MainMenuButton.Exit:
                buttonText.text = "EXIT";
                break;
        }
    }
}