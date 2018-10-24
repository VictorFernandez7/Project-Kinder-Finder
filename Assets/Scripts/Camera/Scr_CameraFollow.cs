using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_CameraFollow : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject playerShip;
    [SerializeField] private GameObject astronaut;

    [HideInInspector] public bool followAstronaut = true;

    private float rot;

    private void Start()
    {
        playerShip = GameObject.Find("PlayerShip");
        astronaut = GameObject.Find("Astronaut");
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
    }
}