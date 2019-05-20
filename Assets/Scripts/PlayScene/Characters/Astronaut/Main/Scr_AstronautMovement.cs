using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Scr_AstronautMovement : MonoBehaviour
{
    [Header("Movement Properties")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float slideSpeed;
    [SerializeField] private LayerMask collisionMask;

    [Header("Space Walk Parameters")]
    [Range(0.1f, 1f)] [SerializeField] private float spaceWalkSpeed;
    [SerializeField] private float rotationDelay;
    [SerializeField] private float attachDistance;
    [SerializeField] private float collisionKnockBack;
    [SerializeField] private float damageMultiplier;
    [SerializeField] private LayerMask asteroidMask;

    [Header("Jump Properties")]
    [SerializeField] private float precisionHeight;
    [SerializeField] private float speedJump;
    [SerializeField] private float stopMovement;
    [Tooltip("Smaller value drag less")]
    [SerializeField] private float airDragModifier;
    [SerializeField] private float gravity;

    [Header("Jetpack Properties")]
    [SerializeField] private float speedJetpack;
    [SerializeField] private float timeCharge;

    [Header("Collision Properties")]
    [SerializeField] private float distance;
    [SerializeField] private float astronautHeight;
    [SerializeField] private float astronautWidth;
    [SerializeField] private float minSlideAngle;

    [Header("Animation Properties")]
    [SerializeField] private float walkAnimationSpeed;
    [SerializeField] private float runAnimationSpeed;

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

    [HideInInspector] public bool canEnterShip;
    [HideInInspector] public bool breathable;
    [HideInInspector] public bool unlockedJetpack;
    [HideInInspector] public Vector3 planetPosition;
    [HideInInspector] public Quaternion planetRotation;
    [HideInInspector] public GameObject currentPlanet;
    [HideInInspector] public bool onDialogue;
    [HideInInspector] public bool jumping;
    [HideInInspector] public bool canMove;

    private bool canMoveRight = true;
    private bool canMoveLeft = true;
    private bool faceRight = true;
    private bool toJump;
    private bool canJump = true;
    private bool lastRight = true;
    private bool attached;
    private bool dettaching; 
    private bool charge;
    private bool movementCapacityOnAir = true;
    private float exponentialMultiplier;
    private float currentVelocity;
    private float timeAtAir;
    private float savedTimeCharge;
    private float timeAfterJump = 0.5f;
    private float savedTimeAfterJump = 0.5f;
    private float timeDettaching = 1;
    private float baseDistanceFromCenterToGround;
    private float currentAngle;
    private float currentDistanceFromCenterToGround;
    private float surfaceAngle;
    private float nextStepAngleLeft;
    private float nextStepAngleRight;
    private Rigidbody2D astronautRb;
    private Scr_PlayerShipMovement playerShipMovement;
    private Scr_PlayerShipActions playerShipActions;
    private Scr_AstronautStats astronautStats;
    private Scr_AstronautEffects astronautEffects;
    private Vector2 movementVector;
    private Vector2 hitLeftGroundPoint;
    private Vector2 hitRightGroundPoint;
    private Vector2 vectorJump;

    public void Start()
    {
        astronautRb = GetComponent<Rigidbody2D>();
        playerShipMovement = playerShip.GetComponent<Scr_PlayerShipMovement>();
        playerShipActions = playerShip.GetComponent<Scr_PlayerShipActions>();
        astronautStats = GetComponent<Scr_AstronautStats>();
        astronautEffects = GetComponent<Scr_AstronautEffects>();

        savedTimeCharge = timeCharge;

        charge = true;
        unlockedJetpack = false;
        planetRotation = new Quaternion(0, 0, 0, 0);

        Calculations();
        baseDistanceFromCenterToGround = currentDistanceFromCenterToGround;
        canMove = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Stop(true, true);
        }

        else if (Input.GetMouseButtonUp(1) && !onDialogue)
        {
            MoveAgain();
        }

        if (onDialogue && !jumping)
            Stop(true, true);

        if (playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.landed && !interfaceManager.gamePaused)
        {
            if (Input.GetButtonDown("Jump") && (surfaceAngle < minSlideAngle && surfaceAngle > -minSlideAngle) && !jumping && canJump)
            {
                timeAtAir = 0;
                speedInJump = speedJump;
                jumping = true;
                astronautAnim.SetTrigger("JumpStart");
                astronautEffects.JumpParticles();
            }

            Jetpacking();
        }

        if (playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.inSpace)
            InSpaceMovement();
    }

    private void FixedUpdate()
    {
        Calculations();
        SnapToFloor();

        if (playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.landed)
        {
            if (surfaceAngle < minSlideAngle && !jumping)
            {
                if (canMove)
                    PlanetMovement();
            }

            else if (jumping)
            {
                Jumping();
                MoveOnAir(lastRight);
            }

            else
                SlideDown();

            PlanetAttachment();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerShip"))
            canEnterShip = true;

        if (collision.CompareTag("Tool"))
            GetComponent<Scr_AstronautsActions>().toolOnFloor = collision.gameObject;

        if (collision.CompareTag("Asteroid") && !attached)
        {
            astronautRb.velocity = new Vector2(-astronautRb.velocity.x, -astronautRb.velocity.y) * collisionKnockBack;
            astronautStats.TakeDamaged(astronautRb.velocity.magnitude * damageMultiplier);
        }

        if (collision.CompareTag("Obstacle"))
        {
            if (jumping)
            {
                movementVector = Vector2.zero;
                timeAtAir = 0;
                speedInJump = collision.GetComponent<Scr_Obstacle>().impulseForce;
                astronautEffects.JumpParticles();
            }

            else
            {
                timeAtAir = 0;
                speedInJump = collision.GetComponent<Scr_Obstacle>().impulseForce * 0.5f;
                lastRight = !lastRight;
                currentVelocity = currentVelocity * 0.35f;
                jumping = true;
                astronautAnim.SetTrigger("JumpStart");
                astronautEffects.JumpParticles();
            }

            astronautStats.currentHealth -= collision.GetComponent<Scr_Obstacle>().damage;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerShip")
            canEnterShip = false;

        if (collision.gameObject.tag == "Tool")
            GetComponent<Scr_AstronautsActions>().toolOnFloor = null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Stop(false, lastRight);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        MoveAgain();
    }

    private void Calculations()
    {
        //WITH TWO RAYCAST READ TWO POINTS UNDER THE ASTRONAUT AND CALCULATE THE ANGLE OF THE TERRAIN

        RaycastHit2D hitLeftGround = Physics2D.Raycast(rayPointLeft.transform.position, -rayPointLeft.transform.up, Mathf.Infinity, collisionMask);
        RaycastHit2D hitRightGround = Physics2D.Raycast(rayPointRight.transform.position, -rayPointRight.transform.up, Mathf.Infinity, collisionMask);

        hitLeftGroundPoint = hitLeftGround.point;
        hitRightGroundPoint = hitRightGround.point;

        surfaceAngle = Vector2.Angle(hitRightGroundPoint - hitLeftGroundPoint, transform.right);

        //WITH ANOTHER RAYCAST MEASURE THE SPACE BETWEEN THE CENTER OF THE ASTRONAUT AND THE POINT UNDER IT

        RaycastHit2D hitMiddleGroundPoint = Physics2D.Raycast(transform.position, (currentPlanet.transform.position - transform.position).normalized, Mathf.Infinity, collisionMask);

        if (hitMiddleGroundPoint)
            currentDistanceFromCenterToGround = Vector3.Distance(transform.position, hitMiddleGroundPoint.point);

        //WITH TWO RAYCAST TAKE TO POINTS PER SIDE TO CHECK THE ANGLE OF THE NEXT STEP OF THE ASTRONAUT

        RaycastHit2D hitUpLeft = Physics2D.Raycast(transform.position + (-transform.up * 0.03f), -transform.right, 0.09f, collisionMask);
        RaycastHit2D hitDownLeft = Physics2D.Raycast(transform.position + (-transform.up * 0.05f), -transform.right, 0.09f, collisionMask);
        RaycastHit2D hitUpRight = Physics2D.Raycast(transform.position + (-transform.up * 0.03f), transform.right, 0.09f, collisionMask);
        RaycastHit2D hitDownRight = Physics2D.Raycast(transform.position + (-transform.up * 0.05f), transform.right, 0.09f, collisionMask);

        if (hitUpLeft && hitDownLeft)
            nextStepAngleLeft = Vector2.Angle(hitUpLeft.point - hitDownLeft.point, -transform.right);

        else
            nextStepAngleLeft = 0;

        if (hitUpRight && hitDownRight)
            nextStepAngleRight = Vector2.Angle(hitUpRight.point - hitDownRight.point, transform.right);

        else
            nextStepAngleRight = 0;
    }

    private void SlideDown()
    {/*
        bool right;

        if (Vector3.Project(hitLeftGroundPoint, transform.up).magnitude > Vector3.Project(hitRightGroundPoint, transform.up).magnitude)
        {
            movementVector = (hitLeftGroundPoint - hitRightGroundPoint).normalized;
            right = false;
        }

        else
        {
            movementVector = (hitRightGroundPoint - hitLeftGroundPoint).normalized;
            right = true;
        }

        if ((faceRight && !right) || (!faceRight && right))
            Flip();

        currentVelocity += Vector3.Project((-transform.up * gravity), movementVector).magnitude * slideSpeed;
        transform.Translate(movementVector * currentVelocity, Space.World);*/

        timeAtAir = 0;
        speedInJump = 0.015f;
        jumping = true;
        astronautAnim.SetTrigger("JumpStart");
        astronautEffects.JumpParticles();
    }

    public void Stop(bool complete, bool right)
    {
        currentVelocity = 0;
        exponentialMultiplier = 1;

        astronautAnim.SetBool("Moving", false);

        if (complete)
        {
            canMoveLeft = false;
            canMoveRight = false;
            canJump = false;
        }

        else if(right)
            canMoveRight = false;

        else
            canMoveLeft = false;
    }

    public void MoveAgain()
    {
        canMoveLeft = true;
        canMoveRight = true;
        canJump = true;
    }

    private void PlanetAttachment()
    {
        transform.position += (currentPlanet.transform.position - planetPosition);
        planetPosition = currentPlanet.transform.position;
        transform.RotateAround(currentPlanet.transform.position, Vector3.forward, currentPlanet.transform.rotation.eulerAngles.z - planetRotation.eulerAngles.z);
        planetRotation = currentPlanet.transform.rotation;

        transform.rotation = Quaternion.LookRotation(transform.forward, (transform.position - currentPlanet.transform.position));
    }

    private void SnapToFloor()
    {
        if (!jumping)
        {
            float positionCorrection = currentDistanceFromCenterToGround - baseDistanceFromCenterToGround;

            if (positionCorrection != 0)
                transform.Translate(transform.up * -positionCorrection, Space.World);
        }

        else if ((currentDistanceFromCenterToGround > (baseDistanceFromCenterToGround - precisionHeight)) && (currentDistanceFromCenterToGround < (baseDistanceFromCenterToGround + precisionHeight)) && toJump)
        {
            vectorJump = Vector2.zero;
            speedInJump = 0;
            timeAfterJump = savedTimeAfterJump;
            astronautAnim.SetTrigger("JumpEnd");
            astronautEffects.FallParticles();
            toJump = false;
            jumping = false;
        }
    }

    private void PlanetMovement()
    {
        if (Input.GetButton("Horizontal") && Input.GetAxis("Horizontal") < 0f && canMoveLeft)
        {
            MoveOnPlanet(false, false);
            lastRight = false;
        }

        else if (Input.GetButton("Horizontal") && Input.GetAxis("Horizontal") > 0f && canMoveRight)
        {
            MoveOnPlanet(false, true);
            lastRight = true;
        }

        else if (currentVelocity > 0)
        {
            if (lastRight)
                MoveOnPlanet(true, true);
            else
                MoveOnPlanet(true, false);
        }

        else
        {
            currentVelocity = 0;
            exponentialMultiplier = 1;

            astronautAnim.SetBool("Moving", false);
        }

        movementCapacityOnAir = true;
        astronautAnim.SetBool("OnGround", !jumping);
    }

    private void MoveOnPlanet(bool decelerating, bool right)
    {
        if ((faceRight && !right) || (!faceRight && right))
            Flip();

        if ((nextStepAngleLeft >= minSlideAngle) && !right)
            Stop(false, false);

        else if ((nextStepAngleRight >= minSlideAngle) && right)
            Stop(false, true);

        else
            MoveAgain();

        astronautAnim.SetBool("Moving", true);

        if (!decelerating)
        {
            if (Input.GetButton("Boost"))
            {
                Move(right, runSpeed);
                astronautAnim.SetFloat("Speed", 2.5f);

                if (!breathable)
                    GetComponent<Scr_AstronautStats>().currentOxygen -= 0.05f;
            }

            else
            {
                Move(right, walkSpeed);
                astronautAnim.SetFloat("Speed", 1.75f);
            }
        }

        else
            Move(right, 0);
    }

    private void Move(bool right, float targetVelocity)
    {
        if (right)
            movementVector = (hitRightGroundPoint - hitLeftGroundPoint).normalized;

        if (!right)
            movementVector = (hitLeftGroundPoint - hitRightGroundPoint).normalized;

        if (currentVelocity < targetVelocity)
        {
            currentVelocity += 0.0005f * exponentialMultiplier;
            exponentialMultiplier += 0.5f;

            if (currentVelocity >= targetVelocity)
            {
                currentVelocity = targetVelocity;
                exponentialMultiplier = 1;
            }
        }

        else if (currentVelocity > targetVelocity)
        {
            currentVelocity -= 0.001f * exponentialMultiplier;
            exponentialMultiplier += 1f;

            if (currentVelocity <= targetVelocity)
            {
                currentVelocity = targetVelocity;
                exponentialMultiplier = 1;
            }
        }

        transform.Translate(movementVector * currentVelocity, Space.World);
    }

    float speedInJump;

    private void Jumping()
    {
        if (jumping)
        {
            if (!toJump)
            {
                if (timeAfterJump > 0f)
                    timeAfterJump -= Time.deltaTime;

                else if (timeAfterJump <= 0f)
                    toJump = true;
            }

            speedInJump -= Time.fixedDeltaTime * gravity;
        }

        vectorJump = (transform.position - currentPlanet.transform.position).normalized * speedInJump;
        transform.Translate(vectorJump, Space.World);
    }

    private void Jetpacking()
    {
        if (!charge)
        {
            savedTimeCharge -= Time.deltaTime;

            if (savedTimeCharge <= 0)
            {
                charge = true;
                savedTimeCharge = timeCharge;
            }
        }

        else if (charge && Input.GetKeyDown(KeyCode.R) && unlockedJetpack)
        {
            vectorJump = (transform.position - currentPlanet.transform.position).normalized * speedJetpack;
            jumping = true;
            timeAtAir = 0;
            charge = false;
        }
    }

    private void MoveOnAir(bool right)
    {
        RaycastHit2D hitCollisionLeft = Physics2D.Raycast(transform.position + (transform.up * astronautHeight) + (-transform.right * astronautWidth), -transform.up, distance, collisionMask);
        RaycastHit2D hitCollisionRight = Physics2D.Raycast(transform.position + (transform.up * astronautHeight) + (transform.right * astronautWidth), -transform.up, distance, collisionMask);

        if (hitCollisionLeft || hitCollisionRight)
            currentVelocity = 0;

        if (right)
            movementVector = Vector2.Perpendicular((currentPlanet.transform.position - transform.position).normalized);

        if (!right)
            movementVector = -Vector2.Perpendicular((currentPlanet.transform.position - transform.position).normalized);

        if(currentVelocity > 0)
            currentVelocity -= (airDragModifier / 1000);

        if(currentVelocity == 0 && movementCapacityOnAir)
        {
            if(Input.GetButton("Horizontal") && Input.GetAxis("Horizontal") < 0f)
            {
                movementVector = -Vector2.Perpendicular((currentPlanet.transform.position - transform.position).normalized);
                currentVelocity = stopMovement;
                lastRight = false;
                movementCapacityOnAir = false;
            }

            else if (Input.GetButton("Horizontal") && Input.GetAxis("Horizontal") > 0f)
            {
                movementVector = Vector2.Perpendicular((currentPlanet.transform.position - transform.position).normalized);
                currentVelocity = stopMovement;
                lastRight = true;
                movementCapacityOnAir = false;
            }
        }

        if ((faceRight && !right) || (!faceRight && right))
            Flip();

        transform.Translate(movementVector * currentVelocity, Space.World);
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
                    Flip();

                else if (Input.GetKeyDown(KeyCode.D) && !faceRight)
                    Flip();

                if (Input.GetAxis("Vertical") > 0f)
                    astronautRb.AddForce(transform.up * spaceWalkSpeed);

                else if (Input.GetAxis("Vertical") < 0f)
                    astronautRb.AddForce(-transform.up * spaceWalkSpeed);

                if (Input.GetAxis("Horizontal") > 0f)
                    astronautRb.AddForce(transform.right * spaceWalkSpeed);

                else if (Input.GetAxis("Horizontal") < 0f)
                    astronautRb.AddForce(-transform.right * spaceWalkSpeed);

                RaycastHit2D attachToAsteroid = Physics2D.Raycast(transform.position, -transform.up, attachDistance, asteroidMask);

                //revisar
                RaycastHit2D hitL = Physics2D.Raycast(rayPointLeft.transform.position, -rayPointLeft.transform.up, Mathf.Infinity, collisionMask);
                RaycastHit2D hitR = Physics2D.Raycast(rayPointRight.transform.position, -rayPointRight.transform.up, Mathf.Infinity, collisionMask);

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

                        if (timeDettaching <= 0)
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