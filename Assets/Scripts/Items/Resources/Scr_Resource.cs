using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Resource : MonoBehaviour
{
    [Header("Resource Properties")]
    [SerializeField] private ResourceType resourceType;
    [SerializeField] private int typeIndex;

    [Header("Physics")]
    [SerializeField] private float gravity;
    [SerializeField] private float spawnImpulse;
    [SerializeField] private float maxSpawnAngle;

    [HideInInspector] public GameObject resourceReference;

    private bool emulatePhysics;
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

        ResourceTypeChoice();
        InitialImpulse();
    }

    private void Update()
    {
        if (emulatePhysics)
        {
            rb.AddForce((playerShipMovement.currentPlanet.transform.position - gameObject.transform.position).normalized * gravity);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Planet"))
        {
            emulatePhysics = false;
            transform.SetParent(collision.transform);
        }
    }

    private void ResourceTypeChoice()
    {
        switch (resourceType)
        {
            case ResourceType.gas:
                resourceReference = GameObject.Find("ReferenceManager").GetComponent<Scr_ReferenceManager>().GasResources[typeIndex];
                break;

            case ResourceType.solid:
                resourceReference = GameObject.Find("ReferenceManager").GetComponent<Scr_ReferenceManager>().SolidResources[typeIndex];
                break;
        }
    }

    private void InitialImpulse()
    {
        spawnAngle = Random.Range(-maxSpawnAngle, maxSpawnAngle);

        rb.AddForce((transform.up + transform.right * (spawnAngle / 10)).normalized * spawnImpulse);
    }
}