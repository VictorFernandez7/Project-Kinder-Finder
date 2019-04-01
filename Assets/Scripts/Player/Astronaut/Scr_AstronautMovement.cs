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
    [Tooltip("Smaller value fall slower")]
    [SerializeField] private float fallModifier;
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
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;

    [Header("References")]
    [SerializeField] private GameObject rayPointLeft;
    [SerializeField] private GameObject rayPointRight;
    [SerializeField] private ParticleSystem walkingParticles;
    [SerializeField] private Transform transforms;
    [SerializeField] private GameObject astronautVisuals;
    [SerializeField] private Animator astronautAnim;
    [SerializeField] private Animator bodyAnim;
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
    [HideInInspector] public bool unlockedJetpack;
    [HideInInspector] public float timeAtAir;
    [HideInInspector] public float velocity;
    [HideInInspector] public float exponentialMultiplier;
    [HideInInspector] public Vector3 planetPosition;
    [HideInInspector] public Vector3 vectorJump;
    [HideInInspector] public Quaternion planetRotation;
    [HideInInspector] public GameObject currentPlanet;

    private bool toJump;
    private bool canMoveRight = true;
    private bool canMoveLeft = true;
    private bool lastRight;
    private bool attached;
    private bool dettaching; 
    private bool charge;
    private float savedTimeCharge;
    private float timeAfterJump = 0.5f;
    private float savedTimeAfterJump = 0.5f;
    private float timeDettaching = 1;
    private float baseDistance;
    private float currentAngle;
    private float currentDistance;
    private float inertialTime;
    private float surfaceAngle;
    private Vector2 movementVector;
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

        savedTimeCharge = timeCharge;

        charge = true;
        unlockedJetpack = false;

        hitCentral = Physics2D.Raycast(transform.position, (playerShipMovement.currentPlanet.transform.position - transform.position).normalized, Mathf.Infinity, collisionMask);

        planetRotation = new Quaternion(0, 0, 0, 0);

        if (hitCentral)
            baseDistance = Vector3.Distance(transform.position, hitCentral.point);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Stop(true, true);
        }

        else if (Input.GetMouseButtonUp(1))
        {
            MoveAgain();
        }

        if (playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.landed && !interfaceManager.gamePaused)
        {
            Jumping();
            Jetpacking();
        }


        if (playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.inSpace)
            InSpaceMovement();
    }

    private void FixedUpdate()
    {
        if (playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.landed)
        {
            hitL = Physics2D.Raycast(rayPointLeft.transform.position, -rayPointLeft.transform.up, Mathf.Infinity, collisionMask);
            hitR = Physics2D.Raycast(rayPointRight.transform.position, -rayPointRight.transform.up, Mathf.Infinity, collisionMask);

            surfaceAngle = Vector2.Angle(hitR.point - hitL.point, transform.right);

            if ((surfaceAngle < minSlideAngle) || jumping)
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
            GetComponent<Scr_AstronautsActions>().toolOnFloor = collision.gameObject;

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

    private void SlideDown()
    {
        if (Vector3.Project(hitL.point, transform.up).magnitude > Vector3.Project(hitR.point, transform.up).magnitude)
            movementVector = (hitL.point - hitR.point).normalized;

        else
            movementVector = (hitR.point - hitL.point).normalized;

        velocity += Vector3.Project((-transform.up * gravity), movementVector).magnitude / 2;

        transform.Translate(movementVector * velocity, Space.World);
    }

    public void Stop(bool complete, bool right)
    {
        velocity = 0;
        exponentialMultiplier = 1;

        astronautAnim.SetBool("Moving", false);
        bodyAnim.SetBool("Moving", false);

        if (complete)
        {
            canMoveLeft = false;
            canMoveRight = false;
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
        SnapToFloor();

        if (Input.GetButton("Horizontal") && Input.GetAxis("Horizontal") < 0f && canMoveLeft)
        {
            MoveOnPlanet(false, false);

            if (!jumping)
                lastRight = false;

            if (onGround)
                bodyAnim.SetBool("Moving", true);

            else
                bodyAnim.SetBool("Moving", false);
        }

        else if (Input.GetButton("Horizontal") && Input.GetAxis("Horizontal") > 0f && canMoveRight)
        {
            MoveOnPlanet(false, true);

            if (!jumping)
                lastRight = true;

            if (onGround)
                bodyAnim.SetBool("Moving", true);

            else
                bodyAnim.SetBool("Moving", false);
        }

        else if (velocity > 0)
        {
            if (lastRight)
                MoveOnPlanet(true, true);
            else
                MoveOnPlanet(true, false);
        }

        else
        {
            velocity = 0;
            exponentialMultiplier = 1;

            astronautAnim.SetBool("Moving", false);
            bodyAnim.SetBool("Moving", false);
        }

        astronautAnim.SetBool("OnGround", !jumping);
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

    private void MoveOnPlanet(bool decelerating, bool right)
    {
        float nextStepAngle = 0;

        if (!right)
        {
            hitAngleUp = Physics2D.Raycast(transform.position + (-transform.up * 0.03f), -transform.right, 0.8f, collisionMask);
            hitAngleDown = Physics2D.Raycast(transform.position + (-transform.up * 0.05f), -transform.right, 0.8f, collisionMask);
        }

        else
        {
            hitAngleUp = Physics2D.Raycast(transform.position + (-transform.up * 0.03f), transform.right, 0.08f, collisionMask);
            hitAngleDown = Physics2D.Raycast(transform.position + (-transform.up * 0.05f), transform.right, 0.08f, collisionMask);
        }

        if (hitAngleUp && hitAngleDown)
        {
            if(right)
                nextStepAngle = Vector2.Angle(hitAngleUp.point - hitAngleDown.point, transform.right);

            else
                nextStepAngle = Vector2.Angle(hitAngleUp.point - hitAngleDown.point, -transform.right);
        }

        if (!jumping)
        {
            if ((faceRight && !right) || (!faceRight && right))
                Flip();

            if (nextStepAngle < minSlideAngle)
            {
                SprintOrWalk(right, decelerating);
                MoveAgain();
            }

            else
                Stop(false, lastRight);
        }
    }

    private void SprintOrWalk(bool right, bool decelerating)
    {
        astronautAnim.SetBool("Moving", true);

        if (!decelerating)
        {
            if (Input.GetButton("Boost"))
            {
                Move(right, sprintSpeed);
                astronautAnim.SetFloat("Speed", 2);
                bodyAnim.SetFloat("Speed", runSpeed);

                if (!breathable)
                    GetComponent<Scr_AstronautStats>().currentOxygen -= 0.05f;
            }

            else
            {
                Move(right, walkingSpeed);
                astronautAnim.SetFloat("Speed", 1);
                bodyAnim.SetFloat("Speed", walkSpeed);
            }
        }

        else
            Move(right, 0);
    }

    private void Move(bool right, float movement)
    {
        if (!jumping)
        {
            if (right)
                movementVector = (hitR.point - hitL.point).normalized;

            if (!right)
                movementVector = (hitL.point - hitR.point).normalized;
        }

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

            if (velocity <= movement)
            {
                velocity = movement;
                exponentialMultiplier = 1;
            }
        }

        transform.Translate(movementVector * velocity, Space.World);
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
            if (Input.GetButtonDown("Jump") && (surfaceAngle < minSlideAngle && surfaceAngle > -minSlideAngle))
            {
                timeAtAir = 0;
                vectorJump = (transform.position - currentPlanet.transform.position).normalized * speedJump;
                jumping = true;
            }
        }

        else if (jumping)
        {
            timeAtAir += Time.deltaTime * 10;
            vectorJump -= (transform.position - currentPlanet.transform.position).normalized * gravity * fallModifier * timeAtAir * Time.deltaTime;
        }

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
        hitJL = Physics2D.Raycast(transform.position + (transform.up * astronautHeight) + (-transform.right * astronautWidth), -transform.up, distance, collisionMask);
        hitJR = Physics2D.Raycast(transform.position + (transform.up * astronautHeight) + (transform.right * astronautWidth), -transform.up, distance, collisionMask);

        if (hitJL || hitJR)
            velocity = 0;

        if (right)
            movementVector = Vector2.Perpendicular((currentPlanet.transform.position - transform.position).normalized);

        if (!right)
            movementVector = -Vector2.Perpendicular((currentPlanet.transform.position - transform.position).normalized);

        if(velocity > 0)
            velocity -= (airDragModifier / 1000);

        transform.Translate(movementVector * velocity, Space.World);
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