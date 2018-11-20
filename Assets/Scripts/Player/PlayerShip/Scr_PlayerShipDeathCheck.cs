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
        {
            landingTime += Time.deltaTime;

            //if (landingTime >= deathTime)
            //    playerShipStats.Death();
        }
    }

    private void CheckAngle()
    {
        landingAngle = Vector3.Angle(playerShipDirection, playerShipToPlanetDirection);

        if (landingAngle >= deathAngle)
            playerShipStats.Death();
    }

    private void LandingQuality()
    {
        float angleQuality = (int)((100 / deathAngle) * (deathAngle - landingAngle));
        float timeQuality = (int)((100 / deathTime) * (landingTime));
        float resultQuality = (angleQuality + timeQuality) / 2;

        //Debug.Log("Landing Quality " + resultQuality + "/100");
        Debug.Log("LANDING INFO | " + "Angle Quality " + angleQuality + "/100 | " + "Time Quality " + timeQuality + "/100");
    }
}