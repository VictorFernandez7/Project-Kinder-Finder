using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_MiniMapCamera : MonoBehaviour
{
    private GameObject playerShip;

    private void Start()
    {
        playerShip = GameObject.Find("PlayerShip");
    }

    private void Update()
    {
        FollowTarget();
    }

    private void FollowTarget()
    {
        transform.position = new Vector3(playerShip.transform.position.x, playerShip.transform.position.y, -10);
    }
}