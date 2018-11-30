using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_PlayerShipDeathCheck : MonoBehaviour
{
    [Header("Landing Parameters")]
    [SerializeField] private float deathAngle;
    [SerializeField] private float deathTime;

    [Header("Collision Parameters")]
    [SerializeField] private float deathVelocity;

    [HideInInspector] public Vector3 playerShipDirection;
    [HideInInspector] public Vector3 playerShipToPlanetDirection;

    private float landingTime;
    private float landingAngle;    
    private GameObject currentPlanet;
    private Scr_PlayerShipStats playerShipStats;
    private Scr_PlayerShipMovement playerShipMovement;

    private void Start()
    {
        playerShipStats = GetComponentInParent<Scr_PlayerShipStats>();
        playerShipMovement = GetComponentInParent<Scr_PlayerShipMovement>();
    }

    private void Update()
    {
        if (playerShipMovement.currentPlanet != null)
        {
            currentPlanet = playerShipMovement.currentPlanet;
            playerShipDirection = transform.up;
            playerShipToPlanetDirection = new Vector3(transform.position.x - currentPlanet.transform.position.x, transform.position.y - currentPlanet.transform.position.y, transform.position.z - currentPlanet.transform.position.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Planet") && playerShipMovement.landedOnce)
        {
            CheckAngle();
            LandingQuality();
            CheckLandingTime(true);
        }

        if (collision.gameObject.CompareTag("Asteroid"))
            CheckVelocity();
    }

    public void CheckLandingTime(bool reset)
    {
        if (reset)
            landingTime = 0;

        else
            landingTime += Time.deltaTime;
    }

    private void CheckAngle()
    {
        landingAngle = Vector3.Angle(playerShipDirection, playerShipToPlanetDirection);

        if (landingAngle >= deathAngle)
            playerShipStats.Death();
    }

    float maxHelath;

    private void CheckVelocity()
    {
        //float collisionVelocity = GetComponentInParent<Rigidbody2D>().velocity.magnitude * 10;
        //float collisionDamage = (100 / deathVelocity) * (deathVelocity - collisionVelocity);

        //TakeDamage(100 - collisionDamage);
    }

    private void LandingQuality()
    {
        float angleQuality = (int)((100 / deathAngle) * (deathAngle - landingAngle));
        float timeQuality = (int)((100 / deathTime) * (landingTime));
        float resultQuality = (angleQuality + timeQuality) / 2;

        Debug.Log("LANDING QUALITY " + resultQuality + " | " + "Angle Quality " + angleQuality + "/100 | " + "Time Quality " + timeQuality + "/100");

        TakeDamage(100 - resultQuality);
    }

    private void TakeDamage(float amount)
    {
        if (amount >= 0)
            playerShipStats.currentShield -= amount;
    }
}