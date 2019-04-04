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

    [HideInInspector] public static bool[] unlockedTools;

    [HideInInspector] public static bool[] galaxyInfo;
    [HideInInspector] public static bool[] system1Info;
    [HideInInspector] public static bool[] system2Info;

    private bool isActive;
    public static Scr_AstronautsActions astronautsActions;

    private void Start()
    {          
        unlockedTools = new bool[6];
        galaxyInfo = new bool[2];
        system1Info = new bool[5];
        system2Info = new bool[8];

        galaxyInfo[0] = true;
        system1Info[1] = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(commandPanel))
            ActivateCommandPanel();
    }

    public static void Finder()
    {
        astronautsActions = GameObject.Find("Astronaut").GetComponent<Scr_AstronautsActions>();
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
        Scr_Levels.currentLevel = desiredLevel;
        GalaxyCalculator(Scr_Levels.currentLevel);
        SceneManager.LoadScene(MenuStorage.loadingScreen);
    }

    public static void ReloadCurrentScene()
    {
        Scr_Levels.currentlyLoading = Scr_Levels.CurrentlyLoading.LoadingLevel;
        SceneManager.LoadScene(MenuStorage.loadingScreen);
    }

    private static void GalaxyCalculator(Scr_Levels.LevelToLoad currentLevel)
    {
        switch (currentLevel)
        {
            case Scr_Levels.LevelToLoad.PlanetSystem1:
                Scr_Levels.currentGalaxy = Scr_Levels.Galaxies.Galaxy1;
                break;

            case Scr_Levels.LevelToLoad.PlanetSystem2:
                Scr_Levels.currentGalaxy = Scr_Levels.Galaxies.Galaxy1;
                break;

            case Scr_Levels.LevelToLoad.PlanetSystem3:
                Scr_Levels.currentGalaxy = Scr_Levels.Galaxies.Galaxy2;
                break;

            case Scr_Levels.LevelToLoad.PlanetSystem4:
                Scr_Levels.currentGalaxy = Scr_Levels.Galaxies.Galaxy2;
                break;

            case Scr_Levels.LevelToLoad.PlanetSystem5:
                Scr_Levels.currentGalaxy = Scr_Levels.Galaxies.Galaxy2;
                break;

            case Scr_Levels.LevelToLoad.PlanetSystem6:
                Scr_Levels.currentGalaxy = Scr_Levels.Galaxies.Galaxy3;
                break;

            case Scr_Levels.LevelToLoad.PlanetSystem7:
                Scr_Levels.currentGalaxy = Scr_Levels.Galaxies.Galaxy3;
                break;
        }
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
            case "/ULT":
                UnlockTool(1);
                break;
            case "/UGT":
                UnlockTool(2);
                break;
            case "/URPT":
                UnlockTool(3);
                break;
            case "/URCT":
                UnlockTool(4);
                break;
            case "/LALL":
                LockTool();
                break;
        }

        commandInputField.text = "";
    }

    private void UnlockTool(int index)
    {
        astronautsActions.unlockedTools[index] = true;
    }

    private void LockTool()
    {
        for(int i = 1; i < astronautsActions.unlockedTools.Length; i++)
        {
            astronautsActions.unlockedTools[i] = false;
        }
    }
}