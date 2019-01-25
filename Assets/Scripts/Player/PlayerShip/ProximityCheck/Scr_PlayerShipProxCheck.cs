using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Scr_PlayerShipProxCheck : MonoBehaviour
{
    [Header("Indicator Parameters")]
    [SerializeField] private float displayDistance;
    [SerializeField] private float asteroidDistanceDetection;
    [SerializeField] private float planetDistanceDetection;
    [SerializeField] private float asteroidSizeDivider;
    [SerializeField] private float planetSizeDivider;

    [Header("References")]
    [SerializeField] private GameObject asteroidIndicator;
    [SerializeField] private GameObject planetIndicator;
    [SerializeField] private GameObject playerShip;
    [SerializeField] private GameObject worldCanvas;

    [HideInInspector] public List<Scr_AsteroidClass> asteroids;
    [HideInInspector] public List<Scr_PlanetClass> planets;

    private List<GameObject> asteroidIndicators;
    private List<GameObject> planetIndicators;
    private CircleCollider2D trigger;
    private Scr_PlayerShipMovement playerShipMovement;

    private void Awake()
    {
        playerShipMovement = playerShip.GetComponent<Scr_PlayerShipMovement>();
        trigger = GetComponent<CircleCollider2D>();

        asteroids = new List<Scr_AsteroidClass>();
        planets = new List<Scr_PlanetClass>();
        asteroidIndicators = new List<GameObject>();
        planetIndicators = new List<GameObject>();

        trigger.enabled = false;
    }

    private void Update()
    {
        UpdateListStats();
        TriggerActivation();
    }

    private void FixedUpdate()
    {
        AsteroidIndicatorUpdate();
        PlanetIndicatorUpdate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.inSpace)
        {
            if (collision.gameObject.CompareTag("Asteroid"))
            {
                asteroids.Add(new Scr_AsteroidClass(collision.transform.parent.name, collision.gameObject, Vector3.Distance(collision.transform.position, playerShip.transform.position), collision.transform.position));
                CreateAsteroidIndicator(collision.transform.parent.name, collision.transform.position);
            }
            
            else if (collision.gameObject.CompareTag("Planet"))
            {
                planets.Add(new Scr_PlanetClass(collision.name, collision.gameObject, Vector3.Distance(collision.transform.position, playerShip.transform.position), collision.transform.position));
                CreatePlanetIndicator(collision.name, collision.transform.position);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.inSpace)
        {
            if (collision.gameObject.CompareTag("Asteroid"))
            {
                DestroyAsteroid(collision.transform.parent.name);
                DestroyAsteroidIndicator(collision.transform.parent.name);
            }

            else if (collision.gameObject.CompareTag("Planet"))
            {
                DestroyPlanet(collision.transform.parent.name);
                DestroyPlanetIndicator(collision.name);
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

    private void CreateAsteroidIndicator(string collisionName, Vector3 collisionPosition)
    {
        GameObject indicatorClone = Instantiate(asteroidIndicator, collisionPosition, gameObject.transform.rotation);
        indicatorClone.transform.SetParent(worldCanvas.transform);
        indicatorClone.name = collisionName;
        asteroidIndicators.Add(indicatorClone);
    }

    private void CreatePlanetIndicator(string collisionName, Vector3 collisionPosition)
    {
        GameObject indicatorClone = Instantiate(planetIndicator, collisionPosition, gameObject.transform.rotation);
        indicatorClone.transform.SetParent(worldCanvas.transform);
        indicatorClone.name = collisionName;
        planetIndicators.Add(indicatorClone);
    }

    private void DestroyAsteroidIndicator(string collisionName)
    {
        List<GameObject> indicatorsToDelete = new List<GameObject>();

        foreach (GameObject indicator in asteroidIndicators)
        {
            if (indicator.name == collisionName)
                indicatorsToDelete.Add(indicator);
        }

        foreach (GameObject indicator in indicatorsToDelete)
        {
            asteroidIndicators.Remove(indicator);
            Destroy(indicator);
        }
    }

    private void DestroyPlanetIndicator(string collisionName)
    {
        List<GameObject> planetToDelete = new List<GameObject>();

        foreach (GameObject indicator in planetIndicators)
        {
            if (indicator.name == collisionName)
                planetToDelete.Add(indicator);
        }

        foreach (GameObject indicator in planetToDelete)
        {
            planetIndicators.Remove(indicator);
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

    private void AsteroidIndicatorUpdate()
    {
        if (asteroidIndicators.Count != 0)
        {
            foreach (GameObject indicator in asteroidIndicators)
            {
                foreach (Scr_AsteroidClass asteroid in asteroids)
                {
                    if (asteroid.name == indicator.name)
                    {
                        indicator.transform.position = ((asteroid.currentPos - this.transform.position).normalized) * displayDistance + this.transform.position;

                        if (asteroid.distanceToShip <= asteroidDistanceDetection)
                            indicator.transform.localScale = ((asteroidDistanceDetection - asteroid.distanceToShip) / asteroidSizeDivider) * Vector3.one;

                        else
                            indicator.transform.localScale = Vector3.zero;
                    }
                }
            }
        }
    }

    private void PlanetIndicatorUpdate()
    {
        if (planetIndicators.Count != 0)
        {
            foreach (GameObject indicator in planetIndicators)
            {
                foreach (Scr_PlanetClass planet in planets)
                {
                    if (planet.name == indicator.name)
                    {
                        indicator.transform.position = ((planet.currentPos - this.transform.position).normalized) * displayDistance + this.transform.position;

                        if (planet.distanceToShip <= planetDistanceDetection)
                            indicator.transform.localScale = ((planetDistanceDetection - planet.distanceToShip) / planetSizeDivider) * Vector3.one;

                        else
                            indicator.transform.localScale = Vector3.zero;
                    }
                }
            }
        }
    }

    public void ClearInterface(bool active)
    {
        foreach (GameObject indicator in planetIndicators)
        {
            indicator.gameObject.SetActive(active);
        }

        foreach (GameObject indicator in asteroidIndicators)
        {
            indicator.gameObject.SetActive(active);
        }
    }
}