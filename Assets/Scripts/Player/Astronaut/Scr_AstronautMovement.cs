using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Scr_AstronautMovement : MonoBehaviour
{
    [Header("Movement Properties")]
    [SerializeField] private float walkingSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private LayerMask collisionMask;

    [Header("Space Walk Parameters")]
    [Range(0.1f, 1f)] [SerializeField] private float spaceWalkSpeed;
    [SerializeField] private float rotationDelay;
    [SerializeField] private float attachDistance;
    [SerializeField] private float collisionKnockBack;
    [SerializeField] private float damageMultiplier;
    [SerializeField] private LayerMask asteroidMask;

    [Header("Height Properties")]
    [SerializeField] private float precisionHeight;
    [SerializeField] private float speedJump;
    [SerializeField] private float gravity;

    [Header("Collision Properties")]
    [SerializeField] private float distance;
    [SerializeField] private float astronautHeight;
    [SerializeField] private float astronautWidth;
    [SerializeField] private float maxMovementAngle;
    [SerializeField] private float minSlideAngle;

    [Header("References")]
    [SerializeField] private GameObject rayPointLeft;
    [SerializeField] private GameObject rayPointRight;
    [SerializeField] private ParticleSystem walkingParticles;
    [SerializeField] private Transform transforms;
    [SerializeField] private GameObject astronautVisuals;
    [SerializeField] private Animator astronautAnim;
    [SerializeField] private GameObject playerShip;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Scr_InterfaceManager interfaceManager;

    [HideInInspector] public bool onGround = true;
    [HideInInspector] public bool canMove;
    [HideInInspector] public bool canEnterShip;
    [HideInInspector] public bool keep;
    [HideInInspector] public bool faceRight;
    [HideInInspector] public bool breathable;
    [HideInInspector] public bool jumping;
    [HideInInspector] public bool walking;
    [HideInInspector] public float timeAtAir;
    [HideInInspector] public float velocity;
    [HideInInspector] public float exponentialMultiplier;
    [HideInInspector] public Vector3 planetPosition;
    [HideInInspector] public Vector3 vectorJump;
    [HideInInspector] public Quaternion planetRotation;
    [HideInInspector] public GameObject currentPlanet;

    private bool toJump;
    private bool lastRight;
    private bool attached;
    private bool dettaching;
    private float timeAfterJump = 0.5f;
    private float savedTimeAfterJump = 0.5f;
    private float timeDettaching = 1;
    private float baseDistance;
    private float currentAngle;
    private float currentDistance;
    private float inertialTime;
    private Vector2 pointLeft;
    private Vector2 pointRight;
    private Vector2 movementVector;
    private Vector2 lastVector;
    private Rigidbody2D astronautRb;
    private RaycastHit2D hitL;
    private RaycastHit2D hitR;
    private RaycastHit2D hitJL;
    private RaycastHit2D hitJR;
    private RaycastHit2D hitCentral;    
    private RaycastHit2D hitAngleUp;
    private RaycastHit2D hitAngleDown;
    private Scr_PlayerShipMovement playerShipMovement;
    private Scr_PlayerShipActions playerShipActions;
    private Scr_AstronautStats astronautStats;
    private Scr_AstronautEffects astronautEffects;

    public void Start()
    {
        playerShip = GameObject.Find("PlayerShip");
        mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();

        astronautRb = GetComponent<Rigidbody2D>();
        playerShipMovement = playerShip.GetComponent<Scr_PlayerShipMovement>();
        playerShipActions = playerShip.GetComponent<Scr_PlayerShipActions>();
        astronautStats = GetComponent<Scr_AstronautStats>();
        astronautEffects = GetComponent<Scr_AstronautEffects>();

        canMove = true;

        hitCentral = Physics2D.Raycast(transform.position, (playerShipMovement.currentPlanet.transform.position - transform.position).normalized, Mathf.Infinity, collisionMask);

        planetRotation = new Quaternion(0, 0, 0, 0);

        if (hitCentral)
            baseDistance = Vector3.Distance(transform.position, hitCentral.point);
    }

    private void Update()
    {
        if (playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.landed && !interfaceManager.gamePaused)
            Jumping();

        if (playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.inSpace)
            InSpaceMovement();
    }

    private void FixedUpdate()
    {
        if (playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.landed)
        {
            hitL = Physics2D.Raycast(rayPointLeft.transform.position, -rayPointLeft.transform.up, Mathf.Infinity, collisionMask);
            hitR = Physics2D.Raycast(rayPointRight.transform.position, -rayPointRight.transform.up, Mathf.Infinity, collisionMask);

            float anglex = Vector2.Angle(hitR.point - hitL.point, transform.right);

            if (anglex < minSlideAngle || jumping)
            {
                PlanetMovement();

                if (jumping)
                    MoveOnAir(lastRight);
            }
                
            else
                SlideDown();

            PlanetAttachment();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerShip")
            canEnterShip = true;

        if (collision.gameObject.tag == "Tool")
        {
            GetComponent<Scr_AstronautsActions>().toolOnFloor = collision.gameObject;
        }

        if(collision.gameObject.tag == "Asteroid" && !attached)
        {
            astronautRb.velocity = new Vector2(-astronautRb.velocity.x, -astronautRb.velocity.y) * collisionKnockBack;
            astronautStats.TakeDamaged(astronautRb.velocity.magnitude * damageMultiplier);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerShip")
            canEnterShip = false;

        if (collision.gameObject.tag == "Tool")
        {
            GetComponent<Scr_AstronautsActions>().toolOnFloor = null;
        }
    }

    private void SlideDown()
    {
        astronautAnim.SetBool("Moving", false);

        if (hitL)
            pointLeft = hitL.point;

        if (hitR)
            pointRight = hitR.point;

        if(Vector3.Project(pointLeft, transform.up).magnitude > Vector3.Project(pointRight, transform.up).magnitude)
        {
            movementVector = (pointLeft - pointRight).normalized;
        }

        else
        {
            movementVector = (pointRight - pointLeft).normalized;
        }

        velocity += Vector3.Project((-transform.up * gravity), movementVector).magnitude / 2;

        transform.Translate(movementVector * velocity, Space.World);
    }

    private void PlanetAttachment()
    {
        if (onGround)
        {
            transform.position += (currentPlanet.transform.position - planetPosition);
            planetPosition = currentPlanet.transform.position;
            transform.RotateAround(currentPlanet.transform.position, Vector3.forward, currentPlanet.transform.rotation.eulerAngles.z - planetRotation.eulerAngles.z);
            planetRotation = currentPlanet.transform.rotation;
        }

        transform.rotation = Quaternion.LookRotation(transform.forward, (transform.position - currentPlanet.transform.position));
    }

    private void PlanetMovement()
    {
        if (canMove == true)
        {
            currentAngle = Vector2.Angle((hitL.point - hitR.point), transform.right);
            currentAngle = 180 - currentAngle;

            SnapToFloor();

            if (Input.GetButton("Horizontal") && Input.GetAxis("Horizontal") < 0f)
            {
                walking = true;
                MoveLeft(false);
                astronautEffects.MovementParticles(true);

                if(!jumping)
                    lastRight = false;

                astronautAnim.SetBool("Moving", true);
            }

            else if (Input.GetButton("Horizontal") && Input.GetAxis("Horizontal") > 0f)
            {
                walking = true;
                MoveRight(false);
                astronautEffects.MovementParticles(true);

                if (!jumping)
                    lastRight = true;

                astronautAnim.SetBool("Moving", true);
            }

            else if (velocity > 0)
            {
                if (lastRight)
                    MoveRight(true);
                else
                    MoveLeft(true);
            }

            else
            {
                velocity = 0;
                exponentialMultiplier = 1;
                walking = false;
                astronautEffects.MovementParticles(false);

                astronautAnim.SetBool("Moving", false);
            }
        }

        if (jumping)
            astronautAnim.SetBool("Moving", false);
    }

    private void Jumping()
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
            if (Input.GetButtonDown("Jump") && currentAngle <= maxMovementAngle)
            {
                timeAtAir = 0;
                vectorJump = (transform.position - currentPlanet.transform.position).normalized * speedJump;
                jumping = true;
            }
        }

        else if (jumping)
        {
            timeAtAir += Time.deltaTime * 10;
            vectorJump -= (transform.position - currentPlanet.transform.position).normalized * gravity * timeAtAir * Time.deltaTime;
        }

        transform.Translate(vectorJump, Space.World);
    }

    private void MoveOnAir(bool right)
    {
        hitJL = Physics2D.Raycast(transform.position + (transform.up * astronautHeight) + (-transform.right * astronautWidth), -transform.up, distance, collisionMask);
        hitJR = Physics2D.Raycast(transform.position + (transform.up * astronautHeight) + (transform.right * astronautWidth), -transform.up, distance, collisionMask);

        if (hitJL || hitJR)
            velocity = 0;

        if (right)
            movementVector = Vector2.Perpendicular((currentPlanet.transform.position - transform.position).normalized);

        if (!right)
            movementVector = -Vector2.Perpendicular((currentPlanet.transform.position - transform.position).normalized);

        transform.Translate(movementVector * velocity, Space.World);
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

    private void MoveLeft(bool decelerating)
    {
        float angle = 0;
        
        if(jumping)
            hitJL = Physics2D.Raycast(transform.position + (transform.up * astronautHeight) + (-transform.right * astronautWidth), -transform.up, distance, collisionMask);
        
            hitAngleUp = Physics2D.Raycast(transform.position + (-transform.up * 0.06f), -transform.right, 0.08f, collisionMask);
            hitAngleDown = Physics2D.Raycast(transform.position + (-transform.up * 0.03f), -transform.right, 0.08f, collisionMask);

            Debug.DrawLine(transform.position + (transform.up * astronautHeight) + (-transform.right * astronautWidth), hitJL.point, Color.red);

            if (hitAngleUp && hitAngleDown)
            {
                Vector2 vectorAngle = (hitAngleUp.point - hitAngleDown.point);
                angle = Vector2.Angle(vectorAngle, transform.right);
            }
        

        if (!jumping)
        {
            if (hitL)
                pointLeft = hitL.point;

            if (hitR)
                pointRight = hitR.point;

            if (faceRight)
                Flip();
        }

        if ((!hitJL && jumping) || angle <= maxMovementAngle)
            Sprint(false, decelerating);
    }

    private void MoveRight(bool decelerating)
    {
        float angle = 0;

        if (jumping)
            hitJR = Physics2D.Raycast(transform.position + (transform.up * astronautHeight) + (transform.right * astronautWidth), -transform.up, distance, collisionMask);

            hitAngleUp = Physics2D.Raycast(transform.position + (-transform.up * 0.06f), transform.right, 0.08f, collisionMask);
            hitAngleDown = Physics2D.Raycast(transform.position + (-transform.up * 0.03f), transform.right, 0.08f, collisionMask);

            if (hitAngleUp && hitAngleDown)
            {
                Vector2 vectorAngle = (hitAngleUp.point - hitAngleDown.point);
                angle = Vector2.Angle(vectorAngle, -transform.right);
            }

        if (!jumping)
        {
            if (hitL)
                pointLeft = hitL.point;

            if (hitR)
                pointRight = hitR.point;

            Debug.DrawLine(rayPointLeft.transform.position, pointLeft, Color.yellow);
            Debug.DrawLine(rayPointRight.transform.position, pointRight, Color.yellow);

            if (!faceRight)
                Flip();
        }

        if ((!hitJR && jumping) || angle <= maxMovementAngle)
            Sprint(true, decelerating);
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

        Debug.DrawRay(transform.position, movementVector, Color.red);

        if (velocity < movement)
        {
            velocity += 0.0005f * exponentialMultiplier;
            exponentialMultiplier += 0.5f;

            if (velocity >= movement)
            {
                velocity = movement;
                exponentialMultiplier = 1;
            }
        }

        else if (velocity > movement)
        {
            velocity -= 0.001f * exponentialMultiplier;
            exponentialMultiplier += 1f;

            if(velocity <= movement)
            {
                velocity = movement;
                exponentialMultiplier = 1;
            }
        }

        transform.Translate(movementVector * velocity, Space.World);
    }

    private void Sprint(bool right, bool decelerating)
    {
        if (!decelerating)
        {
            if (Input.GetButton("Boost"))
            {
                Move(right, sprintSpeed);

                if (!breathable)
                    GetComponent<Scr_AstronautStats>().currentOxygen -= 0.05f;
            }

            else
                Move(right, walkingSpeed);
        }

        else
            Move(right, 0);
    }

    Vector3 playerShipPosition;

    private void InSpaceMovement()
    {
        if (playerShipActions.doingSpaceWalk)
        {
            if (!attached)
            {
                Vector3 difference = (mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position);
                difference.Normalize();
                float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0f, 0f, rotationZ - 90), rotationDelay);
            }

            if (Vector3.Distance(playerShip.transform.position, transform.position) < playerShipActions.maxDistanceOfShip)
            {
                if (Input.GetKeyDown(KeyCode.A) && faceRight)
                {
                    Flip();
                }

                else if (Input.GetKeyDown(KeyCode.D) && !faceRight)
                {
                    Flip();
                }

                if (Input.GetAxis("Vertical") > 0f)
                    astronautRb.AddForce(transform.up * spaceWalkSpeed);

                else if (Input.GetAxis("Vertical") < 0f)
                    astronautRb.AddForce(-transform.up * spaceWalkSpeed);

                if (Input.GetAxis("Horizontal") > 0f)
                    astronautRb.AddForce(transform.right * spaceWalkSpeed);

                else if (Input.GetAxis("Horizontal") < 0f)
                    astronautRb.AddForce(-transform.right * spaceWalkSpeed);

                RaycastHit2D attachToAsteroid = Physics2D.Raycast(transform.position, -transform.up, attachDistance, asteroidMask);
                hitL = Physics2D.Raycast(rayPointLeft.transform.position, -rayPointLeft.transform.up, Mathf.Infinity, collisionMask);
                hitR = Physics2D.Raycast(rayPointRight.transform.position, -rayPointRight.transform.up, Mathf.Infinity, collisionMask);

                Debug.DrawRay(transform.position, -transform.up * attachDistance, Color.red);

                if (attachToAsteroid && !attached)
                {
                    astronautRb.isKinematic = true;
                    astronautRb.velocity = Vector2.zero;
                    transform.rotation = Quaternion.LookRotation(transform.forward, -Vector2.Perpendicular(hitL.point - hitR.point));
                    transform.position = attachToAsteroid.point + (Vector2)transform.up * 0.05f;
                    transform.SetParent(attachToAsteroid.transform);
                    attached = true;
                }

                else if(!attached)
                {
                    if (mainCamera.GetComponent<Scr_MainCamera>().mining)
                    {
                        transform.position += (playerShip.transform.position - playerShipPosition);
                        playerShipPosition = playerShip.transform.position;
                    }
                }

                else if (attached)
                {
                    if (Input.GetButtonDown("Jump"))
                    {
                        astronautRb.isKinematic = false;
                        transform.SetParent(null);

                        if (mainCamera.GetComponent<Scr_MainCamera>().mining)
                        {
                            transform.position += (playerShip.transform.position - playerShipPosition);
                            playerShipPosition = playerShip.transform.position;
                        }
                        dettaching = true;
                    }

                    else if(!dettaching)
                        playerShipPosition = playerShip.transform.position;

                    if (dettaching)
                    {
                        astronautRb.AddForce(transform.up * spaceWalkSpeed * 2); 

                        if (mainCamera.GetComponent<Scr_MainCamera>().mining)
                        {
                            transform.position += (playerShip.transform.position - playerShipPosition);
                            playerShipPosition = playerShip.transform.position;
                        }

                        timeDettaching -= Time.deltaTime;

                        if(timeDettaching <= 0)
                        {
                            attached = false;
                            timeDettaching = 1;
                            dettaching = false;
                        }
                    }
                }
            }

            else
            {
                transform.position = new Vector3(playerShip.transform.position.x, playerShip.transform.position.y, transform.position.z);
                astronautRb.velocity = Vector2.zero;
            }
        }
    }

    private void Flip()
    {
        faceRight = !faceRight;
        astronautVisuals.transform.Rotate(new Vector3(0, 180, 0));
        transforms.transform.Rotate(new Vector3(0, 180, 0));
    }
}