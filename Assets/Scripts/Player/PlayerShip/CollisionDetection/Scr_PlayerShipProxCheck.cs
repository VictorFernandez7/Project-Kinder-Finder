using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Scr_PlayerShipProxCheck : MonoBehaviour
{
    [Header("Proximity Parameters")]
    [SerializeField] private float planetCheckDistance;
    [SerializeField] private float asteroidCheckDistance;
    [SerializeField] private float alertDistance;
    [SerializeField] private string alertText;
    [SerializeField] private Color normalColor;
    [SerializeField] private Color alertColor;

    [Header("References")]
    [SerializeField] private GameObject proximityIndicator;
    [SerializeField] private Sprite planetIcon;
    [SerializeField] private Sprite asteroidIcon;

    [HideInInspector] public List<Scr_Asteroid> asteroids;

    private GameObject playerShip;

    private void Start()
    {
        playerShip = GameObject.Find("PlayerShip");

        asteroids = new List<Scr_Asteroid>();
    }

    private void Update()
    {
        UpdateAsteroidDistance();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
            asteroids.Add(new Scr_Asteroid(collision.name, collision.gameObject, Vector3.Distance(collision.transform.position, playerShip.transform.position), collision.transform.position));
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            List<Scr_Asteroid> asteroidsToDelete = new List<Scr_Asteroid>();

            foreach (Scr_Asteroid asteroid in asteroids)
            {
                if (asteroid.name == collision.name)
                    asteroidsToDelete.Add(asteroid);
            }

            foreach (Scr_Asteroid asteroid in asteroidsToDelete)
                asteroids.Remove(asteroid);
        }
    }

    private void UpdateAsteroidDistance()
    {
        foreach (Scr_Asteroid asteroid in asteroids)
        {
            asteroid.distanceToShip = Vector3.Distance(transform.position, asteroid.body.transform.position);

            if (asteroid.distanceToShip <= asteroidCheckDistance)
                ShowAsteroidInHUD(asteroid.body);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, asteroidCheckDistance);
    }

    private void ShowAsteroidInHUD(GameObject targetAsteroid)
    {

    }
}