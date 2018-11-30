using System.Collections;
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

    private float closestAsteroidDistance;
    private float[] distances;
    private GameObject closestAsteroid;
    private GameObject[] asteroids;

    private void Start()
    {
        asteroids = new GameObject[0];
        distances = new float[0];
    }

    private void Update()
    {
        //CheckAsteroids();

        print(closestAsteroidDistance);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            GameObject[] newAsteroids = new GameObject[asteroids.Length + 1];
            asteroids.CopyTo(newAsteroids, 0);

            asteroids[asteroids.Length + 1] = collision.gameObject;

        }
    }

    private void CheckAsteroids()
    {
        if (asteroids.Length > 0)
        {
            distances[asteroids.Length] = Vector3.Distance(transform.position, asteroids[asteroids.Length].transform.position);

            closestAsteroidDistance = Mathf.Min(distances);

            for (int i = 0; i <= asteroids.Length; i++)
            {
                if (Vector3.Distance(transform.position, asteroids[i].transform.position) == closestAsteroidDistance)
                    closestAsteroid = asteroids[i];
            }
        }
    }
}