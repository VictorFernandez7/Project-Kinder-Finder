using System.Collections.Generic;
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

        print(indicators.Count);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.inSpace)
        {
            if (collision.gameObject.CompareTag("Asteroid"))
            {
                asteroids.Add(new Scr_AsteroidClass(collision.name, collision.gameObject, Vector3.Distance(collision.transform.position, playerShip.transform.position), collision.transform.position));
                CreateIndicator(collision);
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
                DestroyAsteroid(collision);
                DestroyIndicator(collision);
            }

            else if (collision.gameObject.CompareTag("Planet"))
            {
                DestroyPlanet(collision);
            }
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

    private void CreateIndicator(Collider2D collision)
    {
        GameObject indicatorClone = Instantiate(proximityIndicator);
        indicatorClone.transform.SetParent(playerShipCanvas.transform);
        indicatorClone.name = collision.name;
        indicators.Add(indicatorClone);
    }

    private void DestroyIndicator(Collider2D collision)
    {
        List<GameObject> indicatorsToDelete = new List<GameObject>();

        foreach (GameObject indicator in indicators)
        {
            if (indicator.name == collision.name)
                indicatorsToDelete.Add(indicator);
        }

        foreach (GameObject indicator in indicatorsToDelete)
            indicators.Remove(indicator);
    }

    private void DestroyAsteroid(Collider2D collision)
    {
        List<Scr_AsteroidClass> asteroidsToDelete = new List<Scr_AsteroidClass>();

        foreach (Scr_AsteroidClass asteroid in asteroids)
        {
            if (asteroid.name == collision.name)
                asteroidsToDelete.Add(asteroid);
        }

        foreach (Scr_AsteroidClass asteroid in asteroidsToDelete)
            asteroids.Remove(asteroid);
    }

    private void DestroyPlanet(Collider2D collision)
    {
        List<Scr_PlanetClass> planetsToDelete = new List<Scr_PlanetClass>();

        foreach (Scr_PlanetClass planet in planets)
        {
            if (planet.name == collision.name)
                planetsToDelete.Add(planet);
        }

        foreach (Scr_PlanetClass planet in planetsToDelete)
            planets.Remove(planet);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, displayDistance);
    }
}