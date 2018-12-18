using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_SunLight : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject playerShip;

    private void Update()
    {
        transform.LookAt(playerShip.transform);
    }
}