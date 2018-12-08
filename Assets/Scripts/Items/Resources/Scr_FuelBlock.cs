using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]

public class Scr_FuelBlock : MonoBehaviour
{
    [Header("Object Properties")]
    [SerializeField] public int fuelAmount;

    private GameObject astronautPickUp;

    private void Start()
    {
        astronautPickUp = GameObject.Find("PickUp");

        transform.SetParent(astronautPickUp.transform);
    }
}