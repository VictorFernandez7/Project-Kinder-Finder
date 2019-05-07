using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_PlayerData : Scr_PersistentSingleton<Scr_PlayerData>
{
    public static float checkpointFuel;
    public static float checkpointShield;
    public static Vector3 checkpointPlayershipPosition;
    public static Quaternion checkpointPlayershipRotation;
    public static Transform checkpointPlanet;
    public static bool dead;
}
