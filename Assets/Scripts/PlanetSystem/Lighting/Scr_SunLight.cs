using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_SunLight : MonoBehaviour
{
    private GameObject playerShip;

    private void Start()
    {
        playerShip = GameObject.Find("PlayerShip");
    }

    private void Update()
    {
        transform.LookAt(playerShip.transform);
    }
}