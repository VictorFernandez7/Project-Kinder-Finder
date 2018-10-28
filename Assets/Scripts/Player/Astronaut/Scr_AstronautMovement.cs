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

    [Header("Height System Values")]
    [SerializeField] private float heightVariation;
    [SerializeField] private float precisionValue;

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
    private float baseDistanceRight;
    private float baseDistanceLeft;
    private float currentDistanceLeft;
    private float currentDistanceRight;
    private bool facingRight;
    private SpriteRenderer astronautVisuals;

    private void Start()
    {
        astronautVisuals = GetComponentInChildren<SpriteRenderer>();
        astronautRB = GetComponent<Rigidbody2D>();

        canMove = true;

        hitL = Physics2D.Raycast(rayPointLeft.transform.position, -rayPointLeft.transform.up, checkDistance, collisionMask);

        if (hitL)
            baseDistanceLeft = Vector2.Distance(rayPointLeft.transform.position, hitL.point);

        hitR = Physics2D.Raycast(rayPointRight.transform.position, -rayPointRight.transform.up, checkDistance, collisionMask);

        if (hitR)
            baseDistanceRight = Vector2.Distance(rayPointRight.transform.position, hitR.point);
    }

    private void Update()
    {
        if (canMove == true)
        {
            if (Input.GetKey(KeyCode.A))
            {
                hitL = Physics2D.Raycast(rayPointLeft.transform.position, -rayPointLeft.transform.up, checkDistance, collisionMask);
                hitR = Physics2D.Raycast(rayPointRight.transform.position, -rayPointRight.transform.up, checkDistance, collisionMask);

                MoveLeft();
                Flip(true);

                if (hitL)
                    currentDistanceLeft = Vector2.Distance(rayPointLeft.transform.position, hitL.point);

                if (hitR)
                    currentDistanceRight = Vector2.Distance(rayPointRight.transform.position, hitR.point);

                if (currentDistanceLeft < (baseDistanceLeft - precisionValue))
                    transform.position += new Vector3(0f, (currentDistanceLeft - baseDistanceLeft) * heightVariation, 0f);

                else if (currentDistanceLeft > (baseDistanceLeft + precisionValue))
                    transform.position += new Vector3(0f, (currentDistanceRight - baseDistanceRight) * heightVariation, 0f);
            }

            else if (Input.GetKey(KeyCode.D))
            {
                hitL = Physics2D.Raycast(rayPointLeft.transform.position, -rayPointLeft.transform.up, checkDistance, collisionMask);
                hitR = Physics2D.Raycast(rayPointRight.transform.position, -rayPointRight.transform.up, checkDistance, collisionMask);

                MoveRight();
                Flip(false);

                if (hitL)
                    currentDistanceLeft = Vector2.Distance(rayPointLeft.transform.position, hitL.point);

                if (hitR)
                    currentDistanceRight = Vector2.Distance(rayPointRight.transform.position, hitR.point);

                if (currentDistanceRight < (baseDistanceRight - precisionValue))
                    transform.position += new Vector3(0f, (currentDistanceRight - baseDistanceRight) * heightVariation, 0f);

                else if (currentDistanceRight > (baseDistanceRight + precisionValue))
                    transform.position += new Vector3(0f, (currentDistanceLeft - baseDistanceLeft) * heightVariation, 0f);
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

    private void Flip( bool orientation)
    {
        facingRight = !facingRight;
        astronautVisuals.flipX = orientation;
    }
}