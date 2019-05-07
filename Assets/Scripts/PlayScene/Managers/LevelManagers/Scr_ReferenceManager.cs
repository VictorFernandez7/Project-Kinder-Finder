using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scr_ReferenceManager : Scr_PersistentSingleton<Scr_ReferenceManager>
{
    [Header("Resources")]
    [SerializeField] public GameObject[] Resources;

    public enum ResourceName
    {
        Oxygen,
        Fuel,
        Carbon,
        Silicon,
        Helium,
        Aerogel,
        Iron,
        Aluminium,
        Magnetite,
        Mercury,
        Ceramic,
        Termatite,
        TechnologicalPart,
        EnergyCore
    }
}