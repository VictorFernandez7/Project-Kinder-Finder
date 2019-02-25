using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Levels
{
    public static CurrentlyLoading currentlyLoading;

    public static MenuToLoad menuToLoad;
    public static LevelToLoad levelToLoad;

    public static LevelToLoad currentLevel;
    public static Galaxies currentGalaxy;

    public enum CurrentlyLoading
    {
        LoadingMenu,
        LoadingLevel
    }

    public enum MenuToLoad
    {
        MainMenu,
        SystemSelection
    }

    public enum LevelToLoad
    {
        PlanetSystem1,
        PlanetSystem2,
        PlanetSystem3,
        PlanetSystem4,
        PlanetSystem5,
        PlanetSystem6,
        PlanetSystem7
    }

    public enum Galaxies
    {
        Galaxy1,
        Galaxy2,
        Galaxy3
    }
}

public class MenuStorage
{
    public static string mainMenu = "Scn_MainMenu";
    public static string systemSelection = "Scn_SystemSelection";
    public static string loadingScreen = "Scn_LoadingScreen";
}

public class LevelStorage
{
    public static string planetSystem1 = "Scn_PlanetSystem1";
    public static string planetSystem2 = "Scn_PlanetSystem2";
    public static string planetSystem3 = "Scn_PlanetSystem3";
    public static string planetSystem4 = "Scn_PlanetSystem4";
    public static string planetSystem5 = "Scn_PlanetSystem5";
    public static string planetSystem6 = "Scn_PlanetSystem6";
    public static string planetSystem7 = "Scn_PlanetSystem7";
}