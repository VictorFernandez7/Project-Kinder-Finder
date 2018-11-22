using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scr_AsteroidBehaviour : MonoBehaviour
{
    [Header("Type")]
    [SerializeField] private AsteroidType asteroidType;

    [Header("Planet Orbit")]
    [SerializeField] private GameObject planet;

    [Header("Movement Parameters")]
    [SerializeField] float moveSpeed;

    [Header("Mining Parameters")]
    [SerializeField] float attachingDistance;

    [Header("Control Assistance")]
    [SerializeField] private string message;

    [HideInInspector] public bool attached;
    [HideInInspector] public bool closeToShip;

    private Vector3 canvasPos;
    private GameObject playerShip;
    private TextMeshProUGUI messageText;
    private Scr_PlayerShipActions playerShipActions;

    private enum AsteroidType
    {
        planetOrbit
    }

    private void Start()
    {
        playerShip = GameObject.Find("PlayerShip");

        playerShipActions = playerShip.GetComponent<Scr_PlayerShipActions>();
        messageText = GetComponentInChildren<TextMeshProUGUI>();

        messageText.text = "";
    }

    private void Update()
    {
        if (asteroidType == AsteroidType.planetOrbit)
            Orbit();

        ShipAttach();

        GetComponentInChildren<Canvas>().gameObject.transform.up = Vector3.up;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attachingDistance);
    }

    private void Orbit()
    {
        transform.RotateAround(planet.transform.position, transform.right, Time.deltaTime * moveSpeed);
    }

    public void CloseToShip()
    {
        messageText.text = message;
        playerShipActions.closeToAsteroid = true;
        playerShipActions.currentAsteroid = gameObject;
    }

    public void NotColoseToShip()
    {
        messageText.text = "";
        playerShipActions.closeToAsteroid = true;
        playerShipActions.currentAsteroid = null;
    }

    private void ShipAttach()
    {
        if (attached)
        {
            float currentDistance = Vector3.Distance(transform.position, playerShip.transform.position);
            float attachingSpeed = currentDistance / 4;

            messageText.text = "";

            if (currentDistance > attachingDistance)
                playerShip.transform.position = Vector3.Lerp(playerShip.transform.position, transform.position, Time.deltaTime * attachingSpeed);
        }
    }
}