using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Scr_AstronautsActions))]
[RequireComponent(typeof(Scr_AstronautStats))]

public class Scr_AstronautMovement : MonoBehaviour
{
    [Header("Movement Properties")]
    [SerializeField] private float movementSpeed;

    [HideInInspector] public Vector3 planetPosition;
    [HideInInspector] public bool onGround = true;
    [HideInInspector] public GameObject currentPlanet;
    [HideInInspector] public bool canMove;
    [HideInInspector] public bool canEnterShip;

    void Update()
    {
        if (canMove == true)
        {
            if (Input.GetKey(KeyCode.A))
            {
                MoveLeft();
                GetComponent<SpriteRenderer>().flipX = true;
            }

            else if (Input.GetKey(KeyCode.D))
            {
                MoveRight();
                GetComponent<SpriteRenderer>().flipX = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (onGround)
        {
            transform.position += (currentPlanet.transform.position - planetPosition);
            planetPosition = currentPlanet.transform.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Planet")
            currentPlanet = collision.gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerShip")
            canEnterShip = true;
    }

    private void MoveRight()
    {
        transform.RotateAround(planetPosition, Vector3.forward, -movementSpeed / (currentPlanet.GetComponent<Renderer>().bounds.size.y / 2) * Time.fixedDeltaTime);
    }

    private void MoveLeft()
    {
        transform.RotateAround(planetPosition, Vector3.forward, movementSpeed / (currentPlanet.GetComponent<Renderer>().bounds.size.y / 2) * Time.fixedDeltaTime);
    }
}