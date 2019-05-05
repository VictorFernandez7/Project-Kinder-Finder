using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_IAMovement : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] private float rotationSpeed;
    [Tooltip("Distance to the ship from which the IA will follow the astronaut.")]
    [SerializeField] private float distanceFormShip;
    [Tooltip("Movement speed depends on the distance between GameObjects, this is a multiplicator.")]
    [Range(0, 3)] [SerializeField] private float astronautFollowMult;
    [Tooltip("Movement speed depends on the distance between GameObjects, this is a multiplicator.")]
    [Range(0, 3)] [SerializeField] private float playerShipFollowMult;

    [Header("Interaction Parameters")]
    [SerializeField] private float delay;

    private GameObject astronaut;
    private Transform astronautSpot;
    private GameObject playerShip;
    private Transform playerShipSpot;
    private Scr_PlayerShipMovement playerShipMovement;
    private float desiredSpeed;
    private float savedDelay;
    private Vector3 desiredRotation;
    private Animator anim;

    [HideInInspector] public Transform target;
    [HideInInspector] public bool isMining;

    private void Start()
    {
        astronaut = GameObject.Find("Astronaut");
        astronautSpot = GameObject.Find("AstronautSpot").GetComponent<Transform>();
        playerShip = GameObject.Find("PlayerShip");
        playerShipSpot = GameObject.Find("PlayerShipSpot").GetComponent<Transform>();

        anim = GetComponentInChildren<Animator>();
        playerShipMovement = playerShip.GetComponent<Scr_PlayerShipMovement>();

        savedDelay = delay;
        target = playerShipSpot;
    }

    private void Update()
    {
        CheckPlanet();
        CheckDistance();
        Interactions();
    }

    private void FixedUpdate()
    {
        FollowTarget();
    }

    private void CheckPlanet()
    {
        transform.SetParent(playerShipMovement.currentPlanet.transform);
    }

    private void CheckDistance()
    {
        if (!isMining)
        {
            if (Vector3.Distance(astronaut.transform.position, playerShip.transform.position) <= distanceFormShip)
                target = playerShipSpot;

            else
                target = astronautSpot;
        }
    }

    private void FollowTarget()
    {
        float astronautFollowSpeed = (Vector3.Distance(astronaut.transform.position, transform.position));
        float playerShipFollowSpeed = (Vector3.Distance(playerShip.transform.position, transform.position));
        Vector3 astronautUpVector = astronaut.transform.up;
        Vector3 playerShipVectorUp = playerShip.transform.up;

        if (target == astronautSpot)
            desiredSpeed = astronautFollowSpeed * astronautFollowMult;

        else
            desiredSpeed = playerShipFollowSpeed * playerShipFollowMult;

        if (target == astronautSpot)
            desiredRotation = Vector3.Lerp(desiredRotation, astronautUpVector, Time.deltaTime * rotationSpeed);

        else if (target == playerShipSpot)
            desiredRotation = Vector3.Lerp(desiredRotation, playerShipVectorUp, Time.deltaTime * rotationSpeed);

        else if (target)
            desiredRotation = Vector3.Lerp(desiredRotation, target.up, Time.deltaTime * rotationSpeed);

        if (target != null)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * desiredSpeed);
            transform.rotation = Quaternion.LookRotation(transform.forward, desiredRotation);
        }
    }

    private void Interactions()
    {
        anim.SetBool("OnBoard", playerShipMovement.astronautOnBoard);
        
        if (playerShipMovement.astronautOnBoard)
        {
            savedDelay -= Time.deltaTime;

            if (savedDelay <= 0)
                Destroy(gameObject);
        }
    }
}