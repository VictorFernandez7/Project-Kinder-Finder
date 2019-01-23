﻿using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Scr_PlayerShipProxCheck : MonoBehaviour
{
    [Header("Indicator Parameters")]
    [SerializeField] private float displayDistance;
    [SerializeField] private float minSize;
    [SerializeField] private float maxSize;

    [Header("References")]
    [SerializeField] private GameObject proximityIndicator;
    [SerializeField] private GameObject playerShip;
    [SerializeField] private GameObject playerShipCanvas;

    [HideInInspector] public List<Scr_AsteroidClass> asteroids;
    [HideInInspector] public List<Scr_PlanetClass> planets;

    private List<GameObject> indicators;
    private CircleCollider2D trigger;
    private Scr_PlayerShipMovement playerShipMovement;

    private void Awake()
    {
        playerShipMovement = playerShip.GetComponent<Scr_PlayerShipMovement>();
        trigger = GetComponent<CircleCollider2D>();

        asteroids = new List<Scr_AsteroidClass>();
        planets = new List<Scr_PlanetClass>();
        indicators = new List<GameObject>();

        trigger.enabled = false;
    }

    private void Update()
    {
        UpdateListStats();
        TriggerActivation();
        IndicatorUpdate();

        print(indicators.Count);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.inSpace)
        {
            if (collision.gameObject.CompareTag("Asteroid"))
            {
                asteroids.Add(new Scr_AsteroidClass(collision.name, collision.gameObject, Vector3.Distance(collision.transform.position, playerShip.transform.position), collision.transform.position));
                CreateIndicator(collision.transform.parent.name);
            }
            
            else if (collision.gameObject.CompareTag("Planet"))
                planets.Add(new Scr_PlanetClass(collision.name, collision.gameObject, Vector3.Distance(collision.transform.position, playerShip.transform.position), collision.transform.position));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.inSpace)
        {
            if (collision.gameObject.CompareTag("Asteroid"))
            {
                DestroyAsteroid(collision.transform.parent.name);
                DestroyIndicator(collision.transform.parent.name);
            }

            else if (collision.gameObject.CompareTag("Planet"))
                DestroyPlanet(collision.transform.parent.name);
        }
    }

    private void TriggerActivation()
    {
        if (playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.landed)
            trigger.enabled = false;

        if (playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.inSpace)
            trigger.enabled = true;
    }

    private void UpdateListStats()
    {
        foreach (Scr_AsteroidClass asteroid in asteroids)
        {
            asteroid.currentPos = asteroid.body.transform.position;
            asteroid.distanceToShip = Vector3.Distance(transform.position, asteroid.body.transform.position);
        }

        foreach (Scr_PlanetClass planet in planets)
        {
            planet.currentPos = planet.body.transform.position;
            planet.distanceToShip = Vector3.Distance(transform.position, planet.body.transform.position);
        }
    }

    private void CreateIndicator(string collisionName)
    {
        GameObject indicatorClone = Instantiate(proximityIndicator);
        indicatorClone.transform.SetParent(playerShipCanvas.transform);
        indicatorClone.name = collisionName;
        indicators.Add(indicatorClone);
    }

    private void DestroyIndicator(string collisionName)
    {
        List<GameObject> indicatorsToDelete = new List<GameObject>();

        foreach (GameObject indicator in indicators)
        {
            if (indicator.name == collisionName)
                indicatorsToDelete.Add(indicator);
        }

        foreach (GameObject indicator in indicatorsToDelete)
        {
            indicators.Remove(indicator);
            Destroy(indicator);
        }
    }

    private void DestroyAsteroid(string collisionName)
    {
        List<Scr_AsteroidClass> asteroidsToDelete = new List<Scr_AsteroidClass>();

        foreach (Scr_AsteroidClass asteroid in asteroids)
        {
            if (asteroid.name == collisionName)
                asteroidsToDelete.Add(asteroid);
        }

        foreach (Scr_AsteroidClass asteroid in asteroidsToDelete)
            asteroids.Remove(asteroid);
    }

    private void DestroyPlanet(string collisionName)
    {
        List<Scr_PlanetClass> planetsToDelete = new List<Scr_PlanetClass>();

        foreach (Scr_PlanetClass planet in planets)
        {
            if (planet.name == collisionName)
                planetsToDelete.Add(planet);
        }

        foreach (Scr_PlanetClass planet in planetsToDelete)
            planets.Remove(planet);
    }

    private void IndicatorUpdate()
    {
        if (indicators.Count != 0)
        {
            for (int i = 0; i < indicators.Count; i++)
            {
                foreach (Scr_AsteroidClass asteroid in asteroids)
                {
                    if (asteroid.name == indicators[i].name)
                    {
                        //indicators[i].
                    }
                }
            }

            foreach (GameObject indicator in indicators)
            {
                if (Vector3.Distance(this.transform.position, indicator.transform.position) >= displayDistance)
                    indicator.transform.position += (indicator.transform.position - this.transform.position).normalized;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, displayDistance);
    }
}