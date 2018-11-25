using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_MiningResource : MonoBehaviour
{
    [Header("Resource Properties")]
    [SerializeField] private float moveSpeed;

    private GameObject playerShip;

    private void Start()
    {
        playerShip = GameObject.Find("PlayerShip");
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, playerShip.transform.position, Time.deltaTime * moveSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
}