using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_IAMovement : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] private float followDistance;

    [Header("References")]
    [SerializeField] private GameObject player;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        transform.position = (transform.position - player.transform.position).normalized * followDistance + player.transform.position;
        transform.rotation = player.transform.rotation;

        rb.AddForce(player.transform.up * 20);
    }
}