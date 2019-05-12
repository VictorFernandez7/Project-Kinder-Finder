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
    [SerializeField] private Transform rayOrigin3;
    [SerializeField] private GameObject solidVisuals;
    [SerializeField] private GameObject gasVisuals;
    [SerializeField] private GameObject liquidVisuals;
    [SerializeField] public Sprite icon;

    [HideInInspector] public int iD;
    [HideInInspector] public bool onHands;
    [HideInInspector] public bool lerping;
    [HideInInspector] public bool entering;
    [HideInInspector] public GameObject targetPosition;
    [HideInInspector] public GameObject resourceReference;
    [HideInInspector] public ParticleSystem lootParticles;

    private bool emulatePhysics;
    private bool isGrounded;
    private bool lerpingToSurface;
    private float spawnAngle;
    private float speed;
    private Vector3 direction;
    private Vector3 finalHitPoint = Vector3.zero;
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
            transform.position = Vector3.Lerp(transform.position, targetPosition.transform.position, Time.deltaTime * flyingSpeed);

        if (entering)
            IntroduceInTheShip();

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
                    direction = rb.velocity.normalized;
                    speed = rb.velocity.magnitude;
                    rb.isKinematic = false;
                }
            }

            else if (isGrounded && !lerpingToSurface)
            {
                RaycastHit2D hit3 = Physics2D.Raycast(rayOrigin3.position,direction, rayLength + 0.1f, collisionMask);

                if(hit3)
                    finalHitPoint = hit3.point;

                if(finalHitPoint != Vector3.zero && Vector2.Distance(transform.position, finalHitPoint + transform.up * 0.03f) > 0.01f)
                {
                    transform.position = Vector2.Lerp(transform.position, finalHitPoint + transform.up * 0.03f, Time.deltaTime * speed * 10);
                }

                else
                    lerpingToSurface = true;
            }
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

    public void IntroduceInTheShip()
    {
        lerping = false;
        Vector2 position = playerShipMovement.gameObject.transform.position + (playerShipMovement.gameObject.transform.up * 0.2f);
        transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * flyingSpeed);

        if (transform.position.x <= position.x + 0.1f && transform.position.x >= position.x - 0.1f && transform.position.y <= position.y + 0.1f && transform.position.y >= position.y - 0.1f)
            Destroy(this.gameObject);
    }
}