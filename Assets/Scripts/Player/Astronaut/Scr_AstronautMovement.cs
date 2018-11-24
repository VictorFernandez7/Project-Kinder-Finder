using System.Collections;
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
    [HideInInspector] public Vector3 planetPosition;    
    [HideInInspector] public Quaternion planetRotation;
    [HideInInspector] public GameObject currentPlanet;
    [HideInInspector] public GameObject currentFuelCollector;

    private bool facingRight;
    private bool jumping;
    private bool toJump;
    private float timeAfterJump = 1f;
    private float savedTimeAfterJump = 1f;
    private float baseDistance;
    private float currentDistance;
    private Vector2 pointLeft;
    private Vector2 pointRight;
    private Vector2 movementVector;
    private Vector2 lastVector;
    private Vector3 vectorJump;
    private RaycastHit2D hitL;
    private RaycastHit2D hitR;
    private RaycastHit2D hitCentral;    
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
                vectorJump = (transform.position - currentPlanet.transform.position).normalized * speedJump;
                jumping = true;
            }
        }

        else if (jumping)
            vectorJump -= (transform.position - currentPlanet.transform.position).normalized * gravity * Time.deltaTime;

        transform.Translate(vectorJump, Space.World);
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.LookRotation(transform.forward, (transform.position - currentPlanet.transform.position));

        if (canMove == true)
        {
            hitCentral = Physics2D.Raycast(transform.position, (currentPlanet.transform.position - transform.position).normalized, Mathf.Infinity, collisionMask);
           
            if (hitCentral)
                currentDistance = Vector3.Distance(transform.position, hitCentral.point);

            if (!jumping && !keep)
            {
                if (currentDistance > baseDistance)
                    transform.Translate(transform.up * (baseDistance - currentDistance), Space.World);

                else if (currentDistance < baseDistance)
                    transform.Translate(transform.up * (baseDistance - currentDistance), Space.World);
            }

            else if((currentDistance > (baseDistance - precisionHeight)) && (currentDistance < (baseDistance + precisionHeight)) && toJump)
            {
                vectorJump = new Vector2(0f, 0f);
                timeAfterJump = savedTimeAfterJump;
                toJump = false;
                jumping = false;
            }

            if (Input.GetButton("Horizontal") && Input.GetAxis("Horizontal") < 0f)
            {
                hitL = Physics2D.Raycast(rayPointLeft.transform.position, -rayPointLeft.transform.up, Mathf.Infinity, collisionMask);
                hitR = Physics2D.Raycast(rayPointRight.transform.position, -rayPointRight.transform.up, Mathf.Infinity, collisionMask);

                if (hitL)
                    pointLeft = hitL.point;

                if (hitR)
                    pointRight = hitR.point;

                Debug.DrawLine(rayPointLeft.transform.position, pointLeft, Color.yellow);
                Debug.DrawLine(rayPointRight.transform.position, pointRight, Color.yellow);

                if (faceRight)
                    Flip();

                Move(false, walkingSpeed);
                Sprint(false);
            }

            else if (Input.GetButton("Horizontal") && Input.GetAxis("Horizontal") > 0f)
            {
                hitL = Physics2D.Raycast(rayPointLeft.transform.position, -rayPointLeft.transform.up, Mathf.Infinity, collisionMask);
                hitR = Physics2D.Raycast(rayPointRight.transform.position, -rayPointRight.transform.up, Mathf.Infinity, collisionMask);

                if (hitL)
                    pointLeft = hitL.point;

                if (hitR)
                    pointRight = hitR.point;

                Debug.DrawLine(rayPointLeft.transform.position, pointLeft, Color.yellow);
                Debug.DrawLine(rayPointRight.transform.position, pointRight, Color.yellow);

                if (!faceRight)
                    Flip();

                Move(true, walkingSpeed);
                Sprint(true);
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

        if (collision.gameObject.tag == "FuelCollector")
        {
            currentFuelCollector = collision.gameObject;
            closeToCollector = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerShip")
            canEnterShip = false;

        if (collision.gameObject.tag == "FuelCollector")
            closeToCollector = true;
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