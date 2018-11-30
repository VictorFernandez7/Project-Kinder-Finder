﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Scr_AstronautsActions))]
[RequireComponent(typeof(Scr_AstronautStats))]

public class Scr_AstronautMovement : MonoBehaviour
{
    [Header("Movement Properties")]
    [SerializeField] private float walkingSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private LayerMask collisionMask;

    [Header("Height Properties")]
    [SerializeField] private float precisionHeight;
    [SerializeField] private float speedJump;
    [SerializeField] private float gravity;

    [Header("Collision Properties")]
    [SerializeField] private float distance;
    [SerializeField] private float astronautHeight;
    [SerializeField] private float astronautWidth;
    [SerializeField] private float maxAngle;

    [Header("References")]
    [SerializeField] private GameObject rayPointLeft;
    [SerializeField] private GameObject rayPointRight;
    [SerializeField] private ParticleSystem walkingParticles;
    [SerializeField] private Transform transforms;
    [SerializeField] private GameObject astronautVisuals;

    [HideInInspector] public bool onGround = true;
    [HideInInspector] public bool canMove;
    [HideInInspector] public bool canEnterShip;
    [HideInInspector] public bool closeToCollector;
    [HideInInspector] public bool keep;
    [HideInInspector] public bool faceRight;
    [HideInInspector] public bool breathable;
    [HideInInspector] public float velocity = 0;
    [HideInInspector] public Vector3 planetPosition;    
    [HideInInspector] public Quaternion planetRotation;
    [HideInInspector] public GameObject currentPlanet;
    [HideInInspector] public GameObject currentFuelCollector;
    [HideInInspector] public bool jumping;
    [HideInInspector] public Vector3 vectorJump;

    private bool toJump;
    private float timeAfterJump = 0.5f;
    private float savedTimeAfterJump = 0.5f;
    private float baseDistance;
    private float currentDistance;
    private float timeAtAir;
    private float inertialTime;
    private Vector2 pointLeft;
    private Vector2 pointRight;
    private Vector2 movementVector;
    private Vector2 lastVector;
    private RaycastHit2D hitL;
    private RaycastHit2D hitR;
    private RaycastHit2D hitJL;
    private RaycastHit2D hitJR;
    private RaycastHit2D hitCentral;    
    private RaycastHit2D hitAngleUp;
    private RaycastHit2D hitAngleDown;
    private Scr_PlayerShipMovement playerShipMovement;

    public void Start()
    {
        playerShipMovement = GameObject.Find("PlayerShip").GetComponent<Scr_PlayerShipMovement>();

        canMove = true;

        hitCentral = Physics2D.Raycast(transform.position, (currentPlanet.transform.position - transform.position).normalized, Mathf.Infinity, collisionMask);

        if (hitCentral)
            baseDistance = Vector3.Distance(transform.position, hitCentral.point);
    }

    private void Update()
    {
        if (jumping && !toJump)
        {
            if (timeAfterJump > 0f)
                timeAfterJump -= Time.deltaTime;

            else if (timeAfterJump <= 0f)
                toJump = true;
        }

        if ((currentDistance > (baseDistance - precisionHeight)) && (currentDistance < (baseDistance + precisionHeight)))
        {
            if (Input.GetButtonDown("Jump"))
            {
                timeAtAir = 0;
                vectorJump = (transform.position - currentPlanet.transform.position).normalized * speedJump;
                jumping = true;
            }
        }

        else if (jumping)
        {
            timeAtAir += Time.deltaTime * 5;
            vectorJump -= (transform.position - currentPlanet.transform.position).normalized * gravity * timeAtAir * Time.deltaTime;
        }

        transform.Translate(vectorJump, Space.World);
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.LookRotation(transform.forward, (transform.position - currentPlanet.transform.position));

        if (canMove == true)
        {
            SnapToFloor();

            if (Input.GetButton("Horizontal") && Input.GetAxis("Horizontal") < 0f)
            {
                MoveLeft();
            }

            else if (Input.GetButton("Horizontal") && Input.GetAxis("Horizontal") > 0f)
            {
                MoveRight();
            }

            else
            {
                velocity = 0;
            }
        }
        
        if (onGround)
        {
            transform.position += (currentPlanet.transform.position - planetPosition);
            planetPosition = currentPlanet.transform.position;
            transform.RotateAround(currentPlanet.transform.position, Vector3.forward, currentPlanet.transform.rotation.eulerAngles.z - planetRotation.eulerAngles.z);
            planetRotation = currentPlanet.transform.rotation;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerShip")
            canEnterShip = true;

        if (collision.gameObject.tag == "Tool")
        {
            currentFuelCollector = collision.gameObject;
            closeToCollector = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerShip")
            canEnterShip = false;

        if (collision.gameObject.tag == "Tool")
            closeToCollector = false;
    }

    private void SnapToFloor()
    {
        hitCentral = Physics2D.Raycast(transform.position, (currentPlanet.transform.position - transform.position).normalized, Mathf.Infinity, collisionMask);

        if (hitCentral)
            currentDistance = Vector3.Distance(transform.position, hitCentral.point);

        if (!jumping && !keep)
        {
            if (currentDistance > (baseDistance + gravity))
                transform.Translate(transform.up * -gravity, Space.World);

            else if (currentDistance < (baseDistance - gravity))
                transform.Translate(transform.up * gravity, Space.World);
        }

        else if ((currentDistance > (baseDistance - precisionHeight)) && (currentDistance < (baseDistance + precisionHeight)) && toJump)
        {
            vectorJump = new Vector2(0f, 0f);
            timeAfterJump = savedTimeAfterJump;
            toJump = false;
            jumping = false;
        }
    }

    private void MoveLeft()
    {
        float angle = 0;

        hitL = Physics2D.Raycast(rayPointLeft.transform.position, -rayPointLeft.transform.up, Mathf.Infinity, collisionMask);
        hitR = Physics2D.Raycast(rayPointRight.transform.position, -rayPointRight.transform.up, Mathf.Infinity, collisionMask);
        hitJL = Physics2D.Raycast(transform.position + (transform.up * astronautWidth) + (-transform.right * astronautWidth), -transform.up, distance, collisionMask);
        hitAngleUp = Physics2D.Raycast(transform.position + (-transform.up * 0.06f), -transform.right, 0.08f, collisionMask);
        hitAngleDown = Physics2D.Raycast(transform.position + (-transform.up * 0.03f), -transform.right, 0.08f, collisionMask);

        if (hitAngleUp && hitAngleDown)
        {
            Vector2 vectorAngle = (hitAngleUp.point - hitAngleDown.point);
            angle = Vector2.Angle(vectorAngle, transform.right);
        }

        if (hitL)
            pointLeft = hitL.point;

        if (hitR)
            pointRight = hitR.point;

        if (faceRight)
            Flip();

        if (!hitJL && angle <= maxAngle)
            Sprint(false);
    }

    private void MoveRight()
    {
        float angle = 0;

        hitL = Physics2D.Raycast(rayPointLeft.transform.position, -rayPointLeft.transform.up, Mathf.Infinity, collisionMask);
        hitR = Physics2D.Raycast(rayPointRight.transform.position, -rayPointRight.transform.up, Mathf.Infinity, collisionMask);
        hitJR = Physics2D.Raycast(transform.position + (transform.up * astronautWidth) + (transform.right * astronautWidth), -transform.up, distance, collisionMask);
        hitAngleUp = Physics2D.Raycast(transform.position + (-transform.up * 0.06f), transform.right, 0.08f, collisionMask);
        hitAngleDown = Physics2D.Raycast(transform.position + (-transform.up * 0.03f), transform.right, 0.08f, collisionMask);

        if (hitAngleUp && hitAngleDown)
        {
            Vector2 vectorAngle = (hitAngleUp.point - hitAngleDown.point);
            angle = Vector2.Angle(vectorAngle, transform.right);
        }

        if (hitL)
            pointLeft = hitL.point;

        if (hitR)
            pointRight = hitR.point;

        Debug.DrawLine(rayPointLeft.transform.position, pointLeft, Color.yellow);
        Debug.DrawLine(rayPointRight.transform.position, pointRight, Color.yellow);

        if (!faceRight)
            Flip();

        if (!hitJR && angle <= maxAngle)
            Sprint(true);
    }

    private void Move(bool right, float movement)
    {
        if (!jumping)
        {
            if (right)
                movementVector = (pointRight - pointLeft).normalized;

            if (!right)
                movementVector = (pointLeft - pointRight).normalized;
        }

        else
        {
            if (right)
                movementVector = Vector2.Perpendicular((currentPlanet.transform.position - transform.position).normalized);

            if (!right)
                movementVector = -Vector2.Perpendicular((currentPlanet.transform.position - transform.position).normalized);
        }

        Debug.DrawRay(transform.position, movementVector, Color.red);

        transform.Translate(movementVector * movement, Space.World);
    }

    private void Sprint(bool right)
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Move(right, sprintSpeed);

            if (!breathable)
                GetComponent<Scr_AstronautStats>().currentOxygen -= 0.05f;
        }

        else
            Move(right, walkingSpeed);
    }

    private void Flip()
    {
        faceRight = !faceRight;
        astronautVisuals.transform.Rotate(new Vector3(0, 180, 0));
        transforms.transform.Rotate(new Vector3(0, 180, 0));
    }
}