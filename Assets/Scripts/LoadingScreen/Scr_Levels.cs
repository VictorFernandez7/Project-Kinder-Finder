using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Levels
{
    public static LevelToLoad levelToLoad;

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
}

public class LevelStorage
{
    public static string planetSystem1 = "PlanetSystem1";
    public static string planetSystem2 = "PlanetSystem2";
    public static string planetSystem3 = "PlanetSystem3";
    public static string planetSystem4 = "PlanetSystem4";
    public static string planetSystem5 = "PlanetSystem5";
    public static string planetSystem6 = "PlanetSystem6";
    public static string planetSystem7 = "PlanetSystem7";
}