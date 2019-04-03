﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scr_Resource : MonoBehaviour
{
    [Header("Resource Properties")]
    [SerializeField] private ResourceType resourceType;
    [SerializeField] private int typeIndex;
    [SerializeField] public Sprite icon;

    [Header("Physics")]
    [SerializeField] private float gravity;
    [SerializeField] private float spawnImpulse;
    [SerializeField] private float maxSpawnAngle;

    [Header("Raycast")]
    [SerializeField] private float activationDelay;
    [SerializeField] private float rayLength;
    [SerializeField] private LayerMask collisionMask;

    [Header("References")]
    [SerializeField] private Transform rayOrigin1;
    [SerializeField] private Transform rayOrigin2;

    [HideInInspector] public GameObject resourceReference;
    [HideInInspector] public bool onHands;

    private bool emulatePhysics;
    private bool isGrounded;
    private float spawnAngle;
    private Rigidbody2D rb;
    private Scr_PlayerShipMovement playerShipMovement;

    public enum ResourceType
    {
        gas,
        solid
    }

    private void Start()
    {
        playerShipMovement = GameObject.Find("PlayerShip").GetComponent<Scr_PlayerShipMovement>();
        rb = GetComponent<Rigidbody2D>();

        emulatePhysics = true;
        onHands = false;

        ResourceTypeChoice();
        InitialImpulse();
    }

    private void FixedUpdate()
    {
        print(rb.velocity);
        
        if (!onHands)
        {
            activationDelay -= Time.deltaTime;

            if (activationDelay <= 0 && !isGrounded)
            {
                RaycastHit2D hit1 = Physics2D.Raycast(rayOrigin1.position, -transform.up, rayLength, collisionMask);
                RaycastHit2D hit2 = Physics2D.Raycast(rayOrigin1.position, -transform.up, rayLength, collisionMask);

                if (hit1 && hit2)
                {
                    rb.isKinematic = true;
                    isGrounded = true;
                }

                else
                {
                    transform.rotation = Quaternion.LookRotation(transform.forward, -(playerShipMovement.currentPlanet.transform.position - gameObject.transform.position));
                    rb.AddForce((playerShipMovement.currentPlanet.transform.position - gameObject.transform.position).normalized * gravity);
                    rb.isKinematic = false;
                }
            }

            else if (activationDelay <= 0)
                rb.velocity = Vector3.zero;
        }

        else
            rb.isKinematic = true;
    }

    private void ResourceTypeChoice()
    {
        switch (resourceType)
        {
            case ResourceType.gas:
                resourceReference = GameObject.Find("ReferenceManager").GetComponent<Scr_ReferenceManager>().Resources[typeIndex];
                break;

            case ResourceType.solid:
                resourceReference = GameObject.Find("ReferenceManager").GetComponent<Scr_ReferenceManager>().Resources[typeIndex];
                break;
        }
    }

    private void InitialImpulse()
    {
        spawnAngle = Random.Range(-maxSpawnAngle, maxSpawnAngle);

        rb.AddForce((transform.up + transform.right * (spawnAngle / 10)).normalized * spawnImpulse);
    }
}