using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_PlanetDiscovery : MonoBehaviour
{
    [Header("Select Planet Index")]
    [SerializeField] private int planetIndex;

    [Header("Start Conditions")]
    [SerializeField] private bool discovered;

    [Header("References")]
    [SerializeField] private Animator discoveryPanelAnim;
    [SerializeField] private GameObject planet;
    [SerializeField] private Scr_GameManager gameManager;
    [SerializeField] private Scr_PlayerShipStats playerShipStats;
    [SerializeField] private Scr_PlayerShipProxCheck playerShipProxCheck;
    [SerializeField] private Scr_PlayerShipHalo playerShipHalo;

    [HideInInspector] public bool sighted;
    [HideInInspector] public bool explored;

    private CircleCollider2D circleCollider;

    private void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();

        sighted = discovered;
        explored = discovered;
        planet.SetActive(discovered);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerShip"))
        {
            if (!discovered)
            {
                sighted = true;
                planet.SetActive(true);
                circleCollider.enabled = false;
                playerShipStats.GetExperience(gameManager.sightedXP);
                playerShipStats.GetComponent<Scr_PlayerShipEffects>().ConfettiEffect();
                discoveryPanelAnim.SetBool("Show", true);

                Invoke("HideDiscoveryPanel", 1.5f);
            }

            playerShipProxCheck.ClearInterface(false);
            playerShipHalo.disableHalo = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerShip"))
        {
            playerShipProxCheck.ClearInterface(true);
            playerShipProxCheck.trigger.enabled = true;
            playerShipProxCheck.PlanetDiscovered(transform.parent.name);
            playerShipHalo.disableHalo = false;
        }
    }

    public void PlanetExplored()
    {
        if (!explored)
        {
            explored = true;
            playerShipStats.GetExperience(gameManager.exploredXP);

            if (Scr_Levels.currentLevel == Scr_Levels.LevelToLoad.PlanetSystem1)
                Scr_LevelManager.system1Info[planetIndex] = true;

            else if (Scr_Levels.currentLevel == Scr_Levels.LevelToLoad.PlanetSystem2)
                Scr_LevelManager.system2Info[planetIndex] = true;
        }
    }

    private void HideDiscoveryPanel()
    {
        discoveryPanelAnim.SetBool("Show", false);
    }
}