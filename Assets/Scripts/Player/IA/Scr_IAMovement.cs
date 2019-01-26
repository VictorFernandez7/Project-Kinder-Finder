using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_IAMovement : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] private float rotationSpeed;
    [Tooltip("Distance from the ship from which the IA will follow the astronaut.")]
    [SerializeField] private float distanceFormShip;
    [Tooltip("Movement speed depends on the distance between GameObjects, this is a multiplicator.")]
    [Range(0, 3)] [SerializeField] private float astronautFollowMult;
    [Tooltip("Movement speed depends on the distance between GameObjects, this is a multiplicator.")]
    [Range(0, 3)] [SerializeField] private float playerShipFollowMult;

    [Header("References")]
    [SerializeField] private GameObject astronaut;
    [SerializeField] private Transform astronautSpot;
    [SerializeField] private GameObject playerShip;
    [SerializeField] private Transform playerShipSpot;

    private float desiredSpeed;
    private Vector3 desiredRotation;
    private Transform target;

    private void Update()
    {
        CheckPlanet();
        CheckDistance();        
    }

    private void FixedUpdate()
    {
        FollowTarget();
    }

    private void CheckPlanet()
    {
        transform.SetParent(playerShip.GetComponent<Scr_PlayerShipMovement>().currentPlanet.transform);
    }

    private void CheckDistance()
    {
        if (Vector3.Distance(astronaut.transform.position, playerShip.transform.position) <= distanceFormShip)
            target = playerShipSpot;

        else
            target = astronautSpot;
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

        else
            desiredRotation = Vector3.Lerp(desiredRotation, playerShipVectorUp, Time.deltaTime * rotationSpeed);

        if (target != null)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * desiredSpeed);
            transform.rotation = Quaternion.LookRotation(transform.forward, desiredRotation);
        }
    }
}