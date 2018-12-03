using UnityEngine;
using System.Collections;

public class FixedScenes
{
    public static readonly string LVL01_SCENE = "Lvl_01";
    public static readonly string MAIN_SCENE = "Menu";
    public static readonly string LVL02_SCENE = "Lvl_02";
    public static readonly string LOADING_SCENE = "Lvl_02";
    public static bool onOptions;
    
}
public class FixedPlayerPrefKeys
{
    public static readonly string MUSIC_VOLUME = "MusicVolume";
    public static readonly string SFX_VOLUME = "SfxVolume";
}
public class FixedPaths
{
    public static readonly string PERSISTENT_DATA = Application.persistentDataPath;
    public static readonly string PATH_RESOURCE_SFX = "Sound/SFX/";
    public static readonly string PATH_RESOURCE_MUSIC = "Sound/Music/";
}
public class FixedSound
{
    //MUSIC
    public static readonly string TEMITA = "Meadow";
    public static readonly string Shoot = "Launch";
    public static readonly string MainMenu = "Sounds/Music/";
    public static readonly string InGamePause = "Sounds/Music/";
    public static readonly string Song1 = "Sounds/Music/";
    public static readonly string Song2 = "Sounds/Music/";
    public static readonly string Song3 = "Sounds/Music/";
    public static readonly string Song4 = "Sounds/Music/";
    //Ambience
    public static readonly string Discovery = "Discovery";
    //SFX
    //SHIP
    public static readonly string Launch = "Meadow";
    public static readonly string Thruster = "Launch";
    public static readonly string Charge = "";
    public static readonly string ShipGate = "ShipGate";
    public static readonly string HyperSpace = "hyperspace";
    public static readonly string Landing = "Sounds/Music/";
    //UI
    public static readonly string Navigate = "Navigate";
    public static readonly string Select = "Select";
    public static readonly string Error = "Error";
    public static readonly string MenuIn = "MenuIn";
    public static readonly string MenuOut = "MenuOut";
    //BLABLA
    public static readonly string HumanBla = "Sounds/Music/";
    public static readonly string AIBLA = "Sounds/Music/";
    public static readonly string AlienBLA = "Sounds/Music/";

}
