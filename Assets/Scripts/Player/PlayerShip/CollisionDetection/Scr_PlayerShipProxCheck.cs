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

    private List<Scr_Asteroid> asteroids;
    private GameObject playerShip;

    private void Start()
    {
        playerShip = GameObject.Find("PlayerShip");

        asteroids = new List<Scr_Asteroid>();
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            asteroids.Add(new Scr_Asteroid(collision.name, Vector3.Distance(collision.transform.position, playerShip.transform.position),collision.transform.position));
        }
    }
}