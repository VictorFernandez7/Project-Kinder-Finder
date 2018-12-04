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
        UpdateAsteroid();
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

    private void UpdateAsteroid()
    {
        foreach (Scr_Asteroid asteroid in asteroids)
        {
            asteroid.currentPos = asteroid.body.transform.position;
            asteroid.distanceToShip = Vector3.Distance(transform.position, asteroid.body.transform.position);

            if (asteroid.distanceToShip <= asteroidCheckDistance)
                DrawProximityLine(new Vector3(transform.position.x - asteroid.currentPos.x, transform.position.x - asteroid.currentPos.x, transform.position.x - asteroid.currentPos.x), asteroid.distanceToShip);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, asteroidCheckDistance);
    }

    private void DrawProximityLine(Vector3 direction, float distance)
    {
        float lineWidth = distance / 2;
        Vector3 centralPoint = direction.normalized * asteroidCheckDistance;

        for (int i = 0; i < lineWidth; i++)
        {
            
        }
    }
}