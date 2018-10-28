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


    [Header("References")]
    [SerializeField] private GameObject playerShip;
    [SerializeField] private GameObject astronaut;

    [HideInInspector] public bool followAstronaut = true;

    public GameObject currentPlanet;
    private Camera mainCamera;

    private float rot;

    private void Start()
    {
        playerShip = GameObject.Find("PlayerShip");
        astronaut = GameObject.Find("Astronaut");

        //currentPlanet = astronaut.GetComponent<Scr_AstronautMovement>().currentPlanet;
        mainCamera = GetComponent<Camera>();

        mainCamera.orthographicSize = zoomInPlanet;
    }

    private void Update()
    {
        if (followAstronaut == false)
            transform.position = new Vector3(playerShip.transform.position.x, playerShip.transform.position.y, -10);

        else
        {
            transform.position = new Vector3(astronaut.transform.position.x, astronaut.transform.position.y, -10);
            rot = astronaut.transform.rotation.eulerAngles.z;
            transform.rotation = Quaternion.Euler(0f, 0f, rot);
        }

        /*if (Vector3.Distance(currentPlanet.transform.position, playerShip.transform.position) > zoomDistance)
        {
            mainCamera.orthographicSize = Mathf.Lerp(zoomInPlanet, zoomInSpace, Time.deltaTime);
        }*/
    }
}