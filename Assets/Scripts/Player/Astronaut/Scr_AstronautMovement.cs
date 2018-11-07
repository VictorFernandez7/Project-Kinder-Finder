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
    [SerializeField] private float checkDistance;
    [SerializeField] private LayerMask collisionMask;

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
    private RaycastHit2D hitR;
    private Vector2 pointLeft;
    private Vector2 pointRight;
    private bool facingRight;
    private GameObject miniPlayer;
    private GameObject miniPlanet;
    private Scr_GameManager gameManager;

    private void Start()
    {
        miniPlayer = GameObject.Find("MiniPlayer");
        miniPlanet = GameObject.Find("MiniPlanet");
        gameManager = GameObject.Find("GameManager").GetComponent<Scr_GameManager>();

        canMove = true;

        currentPlanet = gameManager.initialPlanet;
        transform.up = - new Vector3(currentPlanet.transform.position.x - transform.position.x, currentPlanet.transform.position.y - transform.position.y, currentPlanet.transform.position.z - transform.position.z);
    }

    private void FixedUpdate()
    {
        if (canMove == true)
        {
            hitL = Physics2D.Raycast(rayPointLeft.transform.position, -rayPointLeft.transform.up, checkDistance, collisionMask);
            hitR = Physics2D.Raycast(rayPointRight.transform.position, -rayPointRight.transform.up, checkDistance, collisionMask);

            if (hitL)
                pointLeft = hitL.point;

            if (hitR)
                pointRight = hitR.point;


            Debug.DrawLine(rayPointLeft.transform.position, pointLeft, Color.yellow);
            Debug.DrawLine(rayPointRight.transform.position, pointRight, Color.yellow);

            if (Input.GetAxis("Horizontal") <= -0.5f)
            {
                Move(false, movementSpeed);

                if (faceRight)
                    Flip();
            }

            else if (Input.GetAxis("Horizontal") >= 0.5f)
            {
                Move(true, movementSpeed);

                if (!faceRight)
                    Flip();
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
        Vector2 movementVector = new Vector2(0f, 0f);
        Vector3 initialPosition = (currentPlanet.transform.position - transform.position);

        if (right)
            movementVector = (pointRight - pointLeft).normalized;

        if (!right)
            movementVector = (pointLeft - pointRight).normalized;

        Debug.DrawRay(transform.position, movementVector, Color.red);

        transform.Translate(movementVector * movement, Space.World);

        float angle = Vector3.Angle(initialPosition, (currentPlanet.transform.position - transform.position));

        if(right)
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