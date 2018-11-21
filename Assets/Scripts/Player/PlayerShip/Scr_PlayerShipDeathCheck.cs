using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_PlayerShipDeathCheck : MonoBehaviour
{
    [Header("Death Parameters")]
    [SerializeField] private float deathAngle;
    [SerializeField] private float deathTime;

    private float landingTime;
    private float landingAngle;
    float angleQuality;
    float timeQuality;
    float resultQuality;
    private Vector3 playerShipDirection;
    private Vector3 playerShipToPlanetDirection;
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

    private void LandingQuality()
    {
        angleQuality = (int)((100 / deathAngle) * (deathAngle - landingAngle));
        timeQuality = (int)((100 / deathTime) * (landingTime));
        resultQuality = (angleQuality + timeQuality) / 2;

        Debug.Log("LANDING QUALITY " + resultQuality + " | " + "Angle Quality " + angleQuality + "/100 | " + "Time Quality " + timeQuality + "/100");

        TakeDamage(100 - resultQuality);
    }

    private void TakeDamage(float amount)
    {
        if (amount >= 0)
            playerShipStats.currentShield -= amount;
    }
}