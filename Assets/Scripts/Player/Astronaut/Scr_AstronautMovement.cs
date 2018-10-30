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

    [Header("Height System Values")]
    [SerializeField] private float precisionValue;

    [Header("References")]
    [SerializeField] private GameObject rayPointLeft;
    [SerializeField] private GameObject rayPointRight;
    [SerializeField] private ParticleSystem walkingParticles;
    [SerializeField] private Transform transforms;

    [HideInInspector] public Vector3 planetPosition;
    [HideInInspector] public bool onGround = true;
    [HideInInspector] public bool canMove = true;
    [HideInInspector] public bool canEnterShip;
    [HideInInspector] public bool closeToCollector;
    [HideInInspector] public bool faceRight;
    [HideInInspector] public GameObject currentPlanet;
    [HideInInspector] public GameObject currentFuelCollector;

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
    private GameObject miniPlayer;
    private GameObject miniPlanet;

    private void Start()
    {
        miniPlayer = GameObject.Find("MiniPlayer");
        miniPlanet = GameObject.Find("MiniPlanet");

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

    private void FixedUpdate()
    {
        Debug.DrawLine(rayPointLeft.transform.position, hitL.point, Color.yellow);
        Debug.DrawLine(rayPointRight.transform.position, hitR.point, Color.yellow);

        if (canMove == true)
        {
            hitL = Physics2D.Raycast(rayPointLeft.transform.position, -rayPointLeft.transform.up, checkDistance, collisionMask);
            hitR = Physics2D.Raycast(rayPointRight.transform.position, -rayPointRight.transform.up, checkDistance, collisionMask);

            if (hitL)
                currentDistanceLeft = Vector2.Distance(rayPointLeft.transform.position, hitL.point);

            if (hitR)
                currentDistanceRight = Vector2.Distance(rayPointRight.transform.position, hitR.point);

            if (Input.GetKey(KeyCode.A))
            {
                MoveLeft();

                if (faceRight)
                    Flip(true);

                if (currentDistanceLeft < (baseDistanceLeft - precisionValue))
                    transform.position += new Vector3(0f, (baseDistanceLeft - currentDistanceLeft), 0f);

                else if (currentDistanceLeft > (baseDistanceLeft + precisionValue))
                    transform.position += new Vector3(0f, (baseDistanceRight - currentDistanceRight), 0f);
            }

            else if (Input.GetKey(KeyCode.D))
            {
                MoveRight();

                if (!faceRight)
                    Flip(false);

                if (currentDistanceRight < (baseDistanceRight - precisionValue))
                    transform.position += new Vector3(0f, (baseDistanceRight - currentDistanceRight), 0f);

                else if (currentDistanceRight > (baseDistanceRight + precisionValue))
                    transform.position += new Vector3(0f, (baseDistanceLeft - currentDistanceLeft), 0f);
            }

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
                walkingParticles.Play();
            else
                walkingParticles.Stop();
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

    private void MoveRight()
    {
        transform.RotateAround(planetPosition, Vector3.forward, -movementSpeed / (currentPlanet.transform.GetChild(0).GetComponent<Renderer>().bounds.size.y / 2) * Time.fixedDeltaTime);
        miniPlayer.transform.RotateAround(miniPlanet.transform.position, Vector3.forward, (-movementSpeed / 4.5f) * Time.fixedDeltaTime);
    }

    private void MoveLeft()
    {
        transform.RotateAround(planetPosition, Vector3.forward, movementSpeed / (currentPlanet.transform.GetChild(0).GetComponent<Renderer>().bounds.size.y / 2) * Time.fixedDeltaTime);
        miniPlayer.transform.RotateAround(miniPlanet.transform.position, Vector3.forward, (movementSpeed / 4.5f) * Time.fixedDeltaTime);
    }

    private void Flip(bool orientation)
    {
        faceRight = !faceRight;

        //astronautVisuals.flipX = !astronautVisuals.flipX; PENDIENTE DE CAMBIAR POR EL 3D
        transforms.transform.Rotate(new Vector3(0, 0, 180));
    }
}