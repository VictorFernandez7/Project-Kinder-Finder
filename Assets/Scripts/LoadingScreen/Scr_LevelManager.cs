using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class Scr_LevelManager : Scr_PersistentSingleton<Scr_LevelManager>
{
    [Header("Travel Values")]
    [SerializeField] public static int travelCost0to1;
    [SerializeField] public static int travelCost1to2;
    [SerializeField] public static int travelCost0to2;

    [Header("Select Key")]
    [SerializeField] private KeyCode commandPanel;

    [Header("References")]
    [SerializeField] private GameObject commandCanvas;
    [SerializeField] private TMP_InputField commandInputField;

    private bool isActive;

    private void Update()
    {
        if (Input.GetKeyDown(commandPanel))
            ActivateCommandPanel();
    }

    public static void LoadMainMenu()
    {
        Scr_Levels.currentlyLoading = Scr_Levels.CurrentlyLoading.LoadingMenu;
        Scr_Levels.menuToLoad = Scr_Levels.MenuToLoad.MainMenu;
        SceneManager.LoadScene(MenuStorage.loadingScreen);
    }

    public static void LoadSystemSelection()
    {
        Scr_Levels.currentlyLoading = Scr_Levels.CurrentlyLoading.LoadingMenu;
        Scr_Levels.menuToLoad = Scr_Levels.MenuToLoad.SystemSelection;
        SceneManager.LoadScene(MenuStorage.loadingScreen);
    }

    public static void LoadPlanetSystem(Scr_Levels.LevelToLoad desiredLevel)
    {
        Scr_Levels.currentlyLoading = Scr_Levels.CurrentlyLoading.LoadingLevel;
        Scr_Levels.levelToLoad = desiredLevel;
        SceneManager.LoadScene(MenuStorage.loadingScreen);
    }

    public static void ReloadCurrentScene()
    {
        Scr_Levels.currentlyLoading = Scr_Levels.CurrentlyLoading.LoadingLevel;
        SceneManager.LoadScene(MenuStorage.loadingScreen);
    }

    public static void QuitGame()
    {
        Application.Quit();
    }

    private void ActivateCommandPanel()
    {
        isActive = !isActive;
        commandCanvas.SetActive(isActive);

        if (isActive)
            commandInputField.Select();
    }

    public void CheckCommands()
    {
        switch (commandInputField.text)
        {
            case "/MM":
                LoadMainMenu();
                break;
            case "/SS":
                LoadSystemSelection();
                break;
            case "/PS1":
                LoadPlanetSystem(Scr_Levels.LevelToLoad.PlanetSystem1);
                break;
            case "/PS2":
                LoadPlanetSystem(Scr_Levels.LevelToLoad.PlanetSystem2);
                break;
            case "/PS3":
                LoadPlanetSystem(Scr_Levels.LevelToLoad.PlanetSystem3);
                break;
            case "/PS4":
                LoadPlanetSystem(Scr_Levels.LevelToLoad.PlanetSystem4);
                break;
            case "/PS5":
                LoadPlanetSystem(Scr_Levels.LevelToLoad.PlanetSystem5);
                break;
            case "/PS6":
                LoadPlanetSystem(Scr_Levels.LevelToLoad.PlanetSystem6);
                break;
            case "/PS7":
                LoadPlanetSystem(Scr_Levels.LevelToLoad.PlanetSystem7);
                break;
        }

        commandInputField.text = "";
    }
}