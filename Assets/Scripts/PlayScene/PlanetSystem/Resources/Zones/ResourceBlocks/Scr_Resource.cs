using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scr_Resource : MonoBehaviour
{
    [Header("Resource Properties")]
    [SerializeField] private int typeIndex;
    [SerializeField] private ResourceType resourceType;

    [Header("Physics")]
    [SerializeField] private float gravity;
    [SerializeField] private float spawnImpulse;
    [SerializeField] private float maxSpawnAngle;
    [SerializeField] private float flyingSpeed;

    [Header("Raycast")]
    [SerializeField] private float activationDelay;
    [SerializeField] private float rayLength;
    [SerializeField] private LayerMask collisionMask;

    [Header("References")]
    [SerializeField] private Transform rayOrigin1;
    [SerializeField] private Transform rayOrigin2;
    [SerializeField] private GameObject solidVisuals;
    [SerializeField] private GameObject gasVisuals;
    [SerializeField] private GameObject liquidVisuals;
    [SerializeField] public Sprite icon;

    [HideInInspector] public int iD;
    [HideInInspector] public bool onHands;
    [HideInInspector] public bool lerping;
    [HideInInspector] public Vector3 targetPosition;
    [HideInInspector] public GameObject resourceReference;
    [HideInInspector] public ParticleSystem lootParticles;

    private bool emulatePhysics;
    private bool isGrounded;
    private float spawnAngle;
    private Rigidbody2D rb;
    private Scr_PlayerShipMovement playerShipMovement;

    public enum ResourceType
    {
        Gas,
        Solid,
        Liquid
    }

    private void Start()
    {
        playerShipMovement = GameObject.Find("PlayerShip").GetComponent<Scr_PlayerShipMovement>();
        rb = GetComponent<Rigidbody2D>();
        lootParticles = GetComponentInChildren<ParticleSystem>();

        emulatePhysics = true;
        onHands = false;

        ResourceTypeChoice();
        InitialImpulse();
    }

    private void FixedUpdate()
    {
        if (lerping)
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * flyingSpeed);

        if (!onHands)
        {
            activationDelay -= Time.deltaTime;

            if (activationDelay <= 0 && !isGrounded)
            {
                RaycastHit2D hit1 = Physics2D.Raycast(rayOrigin1.position, -transform.up, rayLength, collisionMask);
                RaycastHit2D hit2 = Physics2D.Raycast(rayOrigin1.position, -transform.up, rayLength, collisionMask);

                if (hit1 && hit2)
                {
                    rb.velocity = Vector3.zero;
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
            case ResourceType.Gas:
                resourceReference = GameObject.Find("ReferenceManager").GetComponent<Scr_ReferenceManager>().Resources[typeIndex];
                break;

            case ResourceType.Solid:
                resourceReference = GameObject.Find("ReferenceManager").GetComponent<Scr_ReferenceManager>().Resources[typeIndex];
                break;
        }
    }

    private void InitialImpulse()
    {
        spawnAngle = Random.Range(-maxSpawnAngle, maxSpawnAngle);

        rb.AddForce((transform.up + transform.right * (spawnAngle / 10)).normalized * spawnImpulse);
    }

    public void ChangeVisuals(ResourceType targetType)
    {
        switch (targetType)
        {
            case ResourceType.Gas:
                solidVisuals.SetActive(false);
                gasVisuals.SetActive(true);
                liquidVisuals.SetActive(false);
                break;
            case ResourceType.Solid:
                solidVisuals.SetActive(true);
                gasVisuals.SetActive(false);
                liquidVisuals.SetActive(false);
                break;
            case ResourceType.Liquid:
                solidVisuals.SetActive(false);
                gasVisuals.SetActive(false);
                liquidVisuals.SetActive(true);
                break;
        }
    }
}