using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scr_LevelManager : Scr_PersistentSingleton<Scr_LevelManager>
{
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
}