using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_CameraFollow : MonoBehaviour
{
    [Header("Camera Properties")]
    [SerializeField] private float zoomInSpace;
    [SerializeField] private float zoomInPlanet;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float zoomDistance;
    [SerializeField] private float rotationSpeed;

    [Header("References")]
    [SerializeField] private GameObject playerShip;
    [SerializeField] private GameObject astronaut;

    [HideInInspector] public bool followAstronaut = true;
    [HideInInspector] public bool smoothRotation;

    public GameObject currentPlanet;
    private Camera mainCamera;

    private Vector3 playerShipVectorUp;
    private Vector3 astronauUpVector;
    private float angleDifference;
    private float astronautRotation;
    private float currentRotation;
    private bool tookOff;

    private void Start()
    {
        playerShip = GameObject.Find("PlayerShip");
        astronaut = GameObject.Find("Astronaut");
        mainCamera = GetComponent<Camera>();
        //currentPlanet = astronaut.GetComponent<Scr_AstronautMovement>().currentPlanet;

        mainCamera.orthographicSize = zoomInPlanet;
    }

    private void Update()
    {
        angleDifference = Vector3.Angle(transform.up, playerShipVectorUp);

        if (followAstronaut == false)
        {
            transform.position = new Vector3(playerShip.transform.position.x, playerShip.transform.position.y, -10);

            if (smoothRotation)
            {
                playerShipVectorUp = playerShip.transform.up;

                transform.up = Vector3.Lerp(transform.up, playerShipVectorUp, Time.deltaTime * (angleDifference / 60));
            }
        }


        else
        {
            transform.position = new Vector3(astronaut.transform.position.x, astronaut.transform.position.y, -10);
            //smoothRotation = true;

            if (!smoothRotation)
            {
                astronautRotation = astronaut.transform.rotation.eulerAngles.z;

                transform.rotation = Quaternion.Euler(0f, 0f, astronautRotation);
            }

            else
            {
                if (smoothRotation)
                {
                    astronauUpVector = astronaut.transform.up;

                    transform.up = Vector3.Lerp(transform.up, astronauUpVector, Time.deltaTime * rotationSpeed);
                }
            }
        }

        if (Vector3.Distance(currentPlanet.transform.position, playerShip.transform.position) > zoomDistance && mainCamera.orthographicSize < zoomInSpace)
        {
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, zoomInSpace, Time.deltaTime * zoomSpeed);

            tookOff = true;
        }

        if (Vector3.Distance(currentPlanet.transform.position, playerShip.transform.position) < zoomDistance && mainCamera.orthographicSize > zoomInPlanet && tookOff)
        {
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, zoomInPlanet, Time.deltaTime * zoomSpeed);
        }
    }
}