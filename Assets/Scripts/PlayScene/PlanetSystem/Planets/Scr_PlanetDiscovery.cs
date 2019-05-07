﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_PlanetDiscovery : MonoBehaviour
{
    [Header("Start Conditions")]
    [SerializeField] private int planetIndex;

    [Header("Start Conditions")]
    [SerializeField] private bool discovered;

    [Header("References")]
    [SerializeField] private GameObject planet;
    [SerializeField] private Scr_GameManager gameManager;
    [SerializeField] private Scr_PlayerShipStats playerShipStats;

    [HideInInspector] public bool sighted;
    [HideInInspector] public bool explored;

    private CircleCollider2D circleCollider;

    private void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();

        sighted = discovered;
        explored = discovered;
        planet.SetActive(discovered);
        circleCollider.enabled = !discovered;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerShip"))
        {
            sighted = true;
            planet.SetActive(true);
            circleCollider.enabled = false;
            playerShipStats.GetExperience(gameManager.sightedXP);
        }
    }

    public void PlanetExplored()
    {
        explored = true;
        playerShipStats.GetExperience(gameManager.exploredXP);

        if (Scr_Levels.currentLevel == Scr_Levels.LevelToLoad.PlanetSystem1)
            Scr_LevelManager.system1Info[planetIndex] = true;

        else if (Scr_Levels.currentLevel == Scr_Levels.LevelToLoad.PlanetSystem2)
            Scr_LevelManager.system2Info[planetIndex] = true;
    }
}