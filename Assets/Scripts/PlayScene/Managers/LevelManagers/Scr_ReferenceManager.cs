using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scr_ReferenceManager : Scr_PersistentSingleton<Scr_ReferenceManager>
{
    [Header("Respawn Fuel Values")]
    [SerializeField] private float respawnTime;

    [Header("Resources")]
    [SerializeField] public GameObject[] Resources;
    [SerializeField] public GameObject[] Zones;
    [SerializeField] public List<GameObject> respawnFuelResources = new List<GameObject>();

    private float savedRespawnTime;

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

    private void Start()
    {
        savedRespawnTime = respawnTime;
    }

    private void Update()
    {
        if(respawnFuelResources.Count > 0)
        {
            savedRespawnTime -= Time.deltaTime;

            if (savedRespawnTime <= 0)
            {
                respawnFuelResources[0].SetActive(true);
                respawnFuelResources.RemoveAt(0);
                savedRespawnTime = respawnTime;
            }
        }
    }
}