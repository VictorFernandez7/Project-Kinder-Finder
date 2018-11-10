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
    [SerializeField] private float movementSpeed;
    [SerializeField] private LayerMask collisionMask;

    [Header("Hight Properties")]
    [SerializeField] private float precisionHight;
    [SerializeField] private float speedJump;
    [SerializeField] private float gravity;

    [Header("References")]
    [SerializeField] private GameObject rayPointLeft;
    [SerializeField] private GameObject rayPointRight;
    [SerializeField] private ParticleSystem walkingParticles;
    [SerializeField] private Transform transforms;
    [SerializeField] private GameObject astronautVisuals;

    [HideInInspector] public Vector3 planetPosition;
    [HideInInspector] public bool onGround = true;
    [HideInInspector] public bool canMove;
    [HideInInspector] public bool canEnterShip;
    [HideInInspector] public bool closeToCollector;
    [HideInInspector] public bool faceRight;
    [HideInInspector] public GameObject currentPlanet;
    [HideInInspector] public GameObject currentFuelCollector;

    private RaycastHit2D hitL;
    private float timeAfterJump = 1f;
    private float savedTimeAfterJump = 1f;
    private RaycastHit2D hitR;
    private RaycastHit2D hitCentral;
    private float baseDistance;
    private float currentDistance;
    private Vector2 pointLeft;
    private Vector2 pointRight;
    private Vector2 movementVector;
    private Vector2 lastVector;
    private Vector3 vectorJump;
    private bool facingRight;
    private bool jumping;
    private bool toJump;
    private GameObject miniPlayer;
    private GameObject miniPlanet;
    private Scr_PlayerShipMovement playerShipMovement;

    private void Start()
    {
        miniPlayer = GameObject.Find("MiniPlayer");
        miniPlanet = GameObject.Find("MiniPlanet");
        playerShipMovement = GameObject.Find("PlayerShip").GetComponent<Scr_PlayerShipMovement>();

        canMove = true;

        transform.up = - new Vector3(currentPlanet.transform.position.x - transform.position.x, currentPlanet.transform.position.y - transform.position.y, currentPlanet.transform.position.z - transform.position.z);

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

        if ((currentDistance > (baseDistance - precisionHight)) && (currentDistance < (baseDistance + precisionHight)))
        {
            if (Input.GetButtonDown("Jump"))
            {
                vectorJump = (transform.position - currentPlanet.transform.position).normalized * speedJump;
                jumping = true;
            }
        }

        else if (jumping)
        {
            vectorJump -= (transform.position - currentPlanet.transform.position).normalized * gravity * Time.deltaTime;
        }

        transform.Translate(vectorJump, Space.World);
    }

    private void FixedUpdate()
    {
        if (canMove == true)
        {
            hitCentral = Physics2D.Raycast(transform.position, (currentPlanet.transform.position - transform.position).normalized, Mathf.Infinity, collisionMask);
           
            if (hitCentral)
                currentDistance = Vector3.Distance(transform.position, hitCentral.point);

            if (!jumping)
            {
                if (currentDistance > baseDistance)
                    transform.Translate(transform.up * (baseDistance - currentDistance), Space.World);

                else if (currentDistance < baseDistance)
                    transform.Translate(transform.up * (baseDistance - currentDistance), Space.World);
            }

            else if((currentDistance > (baseDistance - precisionHight)) && (currentDistance < (baseDistance + precisionHight)) && toJump)
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

                Move(false, movementSpeed);
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

                Move(true, movementSpeed);
            }
        }
        
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
        if (right)
            movementVector = (pointRight - pointLeft).normalized;

        if (!right)
            movementVector = (pointLeft - pointRight).normalized;

        Debug.DrawRay(transform.position, movementVector, Color.red);

        transform.Translate(movementVector * movement, Space.World);

        float angle = Vector2.Angle(lastVector, (transform.position - currentPlanet.transform.position));

        lastVector = (transform.position - currentPlanet.transform.position);

        if (right)
            transform.Rotate(new Vector3(0f, 0f, -angle), Space.Self);

        else
            transform.Rotate(new Vector3(0f, 0f, angle), Space.Self);

        miniPlayer.transform.RotateAround(miniPlanet.transform.position, Vector3.forward, (movementSpeed / 4.5f) * Time.fixedDeltaTime);
    }

    private void Flip()
    {
        faceRight = !faceRight;
        astronautVisuals.transform.Rotate(new Vector3(0, 180, 0));
        transforms.transform.Rotate(new Vector3(0, 180, 0));
    }
}