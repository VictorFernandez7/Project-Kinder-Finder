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
    [SerializeField] private float checkDistance;
    [SerializeField] private LayerMask collisionMask;

    [Header("References")]
    [SerializeField] private GameObject rayPointLeft;
    [SerializeField] private GameObject rayPointRight;

    [HideInInspector] public Vector3 planetPosition;
    [HideInInspector] public bool onGround = true;
    [HideInInspector] public bool canMove = true;
    [HideInInspector] public bool canEnterShip;
    [HideInInspector] public GameObject currentPlanet;

    private Rigidbody2D astronautRB;
    private Vector3 VectorTangente;
    private RaycastHit2D hitL;
    private RaycastHit2D hitR;
    private float currentDistance;

    private void Start()
    {
        canMove = true;
        astronautRB = GetComponent<Rigidbody2D>();
        hitL = Physics2D.Raycast(rayPointLeft.transform.position, -rayPointLeft.transform.up, checkDistance, collisionMask);
        if (hitL)
            currentDistance = Vector2.Distance(rayPointLeft.transform.position, hitL.transform.position);
    }

    private void Update()
    {
          if (canMove == true)
          {
              if (Input.GetKey(KeyCode.A))
              {
                  MoveLeft();
                  Flip();
              }

              else if (Input.GetKey(KeyCode.D))
              {
                  MoveRight();
                  Flip();
              }
          }

        hitL = Physics2D.Raycast(rayPointLeft.transform.position, -rayPointLeft.transform.up, checkDistance, collisionMask);
        hitR = Physics2D.Raycast(rayPointRight.transform.position, -rayPointLeft.transform.up, checkDistance, collisionMask);
    }

    private void FixedUpdate()
    {

        if (onGround)
        {
            transform.position += (currentPlanet.transform.position - planetPosition);
            planetPosition = currentPlanet.transform.position;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerShip")
            canEnterShip = true;
    }

    private void MoveRight()
    {
        transform.RotateAround(planetPosition, Vector3.forward, -movementSpeed / (currentPlanet.transform.GetChild(0).GetComponent<Renderer>().bounds.size.y / 2) * Time.fixedDeltaTime);
    }

    private void MoveLeft()
    {
        transform.RotateAround(planetPosition, Vector3.forward, movementSpeed / (currentPlanet.transform.GetChild(0).GetComponent<Renderer>().bounds.size.y / 2) * Time.fixedDeltaTime);
    }

    private void Flip()
    {

    }
}