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
        public static readonly string MainMenu = "***";
        public static readonly string InGamePause = "***";
        public static readonly string Song1 = "***";
        public static readonly string Song2 = "***";
        public static readonly string Song3 = "***";
        public static readonly string Song4 = "***";
        //Ambience
        public static readonly string Discovery = "Discovery";
        //SFX
        //SHIP
        public static readonly string Ship_Launch = "Meadow";
        public static readonly string Ship_Thruster = "Snd_Thruster";
        public static readonly string Ship_StartEngines = "Snd_StartEngines";
        public static readonly string Ship_StartEnginesLong = "Snd_StartEngines_Long";
        public static readonly string Ship_ShipGate = "Snd_Gates";
        public static readonly string Ship_HyperSpace = "Snd_Hyperspace";
        public static readonly string Ship_Landing = "***";
        public static readonly string Ship_PreTakeOff = "***";
        public static readonly string Ship_PostTakeOff = "Snd_PostTakeOff";
        public static readonly string Ship_TakeOff = "***";
        public static readonly string Ship_Explode1 = "Snd_Explosion_01";
        public static readonly string Ship_Explode2 = "Snd_Explosion_02";
        public static readonly string Ship_Explode3 = "Snd_Explosion_03";
        public static readonly string Ship_Impact1 = "Snd_Impact";
        public static readonly string Ship_Impact2 = "***";
        public static readonly string Ship_Impact3 = "***";
        public static readonly string Ship_Laser = "Snd_Laser";
        public static readonly string Ship_Shutdown = "Snd_Shutdown";
        public static readonly string Ship_ShutdownLong = "Snd_Shutdown_Long";
        public static readonly string Ship_ThrusterStop = "Snd_Thrustop";
        public static readonly string Ship_DarkHole = "Snd_IntoDarkHole";
        public static readonly string Ship_LeaveAtmos = "Snd_Leave_Atmos";
    //UI
    public static readonly string UI_Navigate = "Snd_ButtonLight_01";
        public static readonly string UI_Select = "Snd_ButtonLight_03";
        public static readonly string UI_Error = "Snd_Button_04";
        public static readonly string UI_MenuIn = "Snd_AnimateIn";
        public static readonly string UI_MenuOut = "Snd_AnimateOut";
        public static readonly string UI_ExitGame = "Snd_ExitGame";
        public static readonly string UI_AddTool = "Snd_Addtool";
        public static readonly string UI_LoadGame1 = "Snd_LoadGame_01";
        public static readonly string UI_LoadGame2 = "Snd_LoadGame_02";
        public static readonly string UI_NewGame = "Snd_NewGame";
        public static readonly string UI_Pause = "Snd_Pause";
        public static readonly string UI_Resume = "Snd_Resume";
        public static readonly string UI_ShipUpgrade = "Snd_ShipUpgrade";
        public static readonly string UI_AstroUpgrade = "***";


        //BLABLA
        public static readonly string HumanBla = "Snd_Sounds/Music/";
        public static readonly string AIBLA = "Snd_Sounds/Music/";
        public static readonly string AlienBLA = "Snd_Sounds/Music/";
        //Astronaut
        public static readonly string Astro_Footsteps = "Snd_Footstep";
        public static readonly string Astro_Jump = "Snd_Jump_Subtle";
        public static readonly string Astro_Build = "Snd_Build";
        public static readonly string Astro_Jetpack = "Snd_Jetpack";
        public static readonly string Astro_Repair1 = "Snd_Repair_01";
        public static readonly string Astro_Repair2 = "Snd_Repair_02";
        public static readonly string Astro_Repair3 = "Snd_Repair_03";
        public static readonly string Astro_Mine = "Snd_ManualMining";
        public static readonly string Astro_StepOutShip = "Snd_OutOfShip(SubirPitch)";
    
        //misc
        public static readonly string misc_Analize = "Snd_Analize";
        public static readonly string misc_AnalizeAI = "Snd_Analize-UndistinctAI";
    
        public static readonly string misc_Beep_01 = "Snd_Beep_01";
        public static readonly string misc_Beep_02 = "Snd_Beep_02";
        
    public static readonly string misc_ChangeScene = "Snd_ChangeScene";
    public static readonly string misc_DataProcess = "Snd_DataProcess";
    public static readonly string misc_DramaticFade = "Snd_DramaticFade";
    public static readonly string misc_EndAlienTrade = "Snd_EndAlienTrade";
    public static readonly string misc_ExtractorRoarCloseSmall = "Snd_ExtractorRoar";
    public static readonly string misc_ExtractorRoarFarBig = "Snd_Snd_ExtractorRoar_02";
    public static readonly string misc_Danger = "Snd_CloseDanger";
    public static readonly string misc_Alarm = "Snd_IncomingMessageAlarm";
    public static readonly string misc_DistantMachinary = "Snd_Machine_Deep";
    public static readonly string misc_OtherShipHyperspace = "Snd_OtherShipHyperspace";
    public static readonly string misc_OtherShipLeaves = "Snd_OtherShipLeaves";
    public static readonly string misc_RoboticAlarm = "Snd_RoboticAlarm";
    public static readonly string misc_SendMessage = "Snd_SendMessage";
    


    //TD
    public static readonly string TD_BuildMulti = "Snd_Build_Multi";
    //Shot
    public static readonly string Shot_Deep = "Snd_DeepShot";
    public static readonly string Shot_Standard = "Snd_StandardShot";


}
