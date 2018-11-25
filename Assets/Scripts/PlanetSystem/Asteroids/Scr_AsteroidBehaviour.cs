﻿using System.Collections;
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

    [Header("References")]
    [SerializeField] public Animator messageTextAnim;
    [SerializeField] public Animator asteroidAnim;

    [HideInInspector] public bool attached;
    [HideInInspector] public bool closeToShip;
    [HideInInspector] public bool move;

    private Vector3 canvasPos;
    private GameObject playerShip;
    private TextMeshProUGUI messageText;
    private Scr_PlayerShipActions playerShipActions;
    private Scr_AsteroidStats asteroidStats;

    private enum AsteroidType
    {
        planetOrbit
    }

    private void Start()
    {
        playerShip = GameObject.Find("PlayerShip");

        playerShipActions = playerShip.GetComponent<Scr_PlayerShipActions>();
        messageText = GetComponentInChildren<TextMeshProUGUI>();
        asteroidStats = GetComponent<Scr_AsteroidStats>();

        move = true;

        messageText.text = message;
    }

    private void Update()
    {
        if (!asteroidStats.dead)
        {
            if (asteroidType == AsteroidType.planetOrbit && move)
                Orbit();

            ShipAttach();
            SetCanvasPositionAndRotation();
        }
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
        messageTextAnim.SetBool("CanAttach", true);
        playerShipActions.closeToAsteroid = true;
        playerShipActions.currentAsteroid = gameObject;
    }

    public void NotColoseToShip()
    {
        messageTextAnim.SetBool("CanAttach", false);
        playerShipActions.closeToAsteroid = true;
        playerShipActions.currentAsteroid = null;
    }

    private void ShipAttach()
    {
        if (attached)
        {
            float currentDistance = Vector3.Distance(transform.position, playerShip.transform.position);
            float attachingSpeed = currentDistance / 4;

            messageTextAnim.SetBool("CanAttach", false);
            asteroidAnim.SetBool("Sliders", true);

            if (currentDistance > attachingDistance)
                playerShip.transform.position = Vector3.Lerp(playerShip.transform.position, transform.position, Time.deltaTime * attachingSpeed);
        }

        else
            asteroidAnim.SetBool("Sliders", false);
    }

    private void SetCanvasPositionAndRotation()
    {
        GetComponentInChildren<Canvas>().gameObject.transform.position = transform.position + Vector3.up * 1.35f;
        GetComponentInChildren<Canvas>().gameObject.transform.up = Vector3.up;
    }
}