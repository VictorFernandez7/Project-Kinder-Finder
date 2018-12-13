using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(EdgeCollider2D))]

public class Scr_Planet : MonoBehaviour
{
    [Header("Planet Properties")]
    [SerializeField] private float movementSpeed;
    [SerializeField] float maxClampDistance;
    [SerializeField] float minClampDistance;
    [SerializeField] [Range(-0.1f, 0.1f)]float rotationSpeed;

    [Header("References")]
    [SerializeField] private GameObject mapIndicator;
    [SerializeField] private GameObject directionIndicator;
    [SerializeField] private GameObject mapVisuals;
    [SerializeField] private GameObject rotationPivot;

    [HideInInspector] public bool switchGravity;

    private double gravityConstant;
    private Vector3 lastFrameRotationPivot;
    private GameObject playerShip;
    private GameObject astronaut;
    private GameObject mainCanvas;
    private Rigidbody2D planetRb;
    private Rigidbody2D playerShipRb;
    private Rigidbody2D astronautRB;
    private Scr_MapManager mapManager;

    private void Start()
    {
        playerShip = GameObject.Find("PlayerShip");
        astronaut = GameObject.Find("Astronaut");
        mainCanvas = GameObject.Find("MainCanvas");
        mapManager = GameObject.Find("MapManager").GetComponent<Scr_MapManager>();

        planetRb = GetComponent<Rigidbody2D>();

        switchGravity = true;
        lastFrameRotationPivot = rotationPivot.transform.position;
        playerShipRb = playerShip.GetComponent<Rigidbody2D>();
        astronautRB = astronaut.GetComponent<Rigidbody2D>();
        gravityConstant = 6.674 * (10 ^ -11);
        mapVisuals.SetActive(true);
    }

    private void FixedUpdate()
    {
        transform.position += (rotationPivot.transform.position - lastFrameRotationPivot);
        lastFrameRotationPivot = rotationPivot.transform.position;

        transform.RotateAround(lastFrameRotationPivot, Vector3.forward, movementSpeed * Time.fixedDeltaTime);

        transform.Rotate(new Vector3(0f, 0f, rotationSpeed), Space.Self);

        if (playerShip.GetComponent<Scr_PlayerShipMovement>().onGround == false && switchGravity)
        {
            Vector3 translocation = transform.position;
            translocation = transform.position - translocation;

            float clampedDistance = Mathf.Clamp(Vector3.Distance(playerShip.transform.position, this.transform.position), minClampDistance, maxClampDistance);
            float clamp = Mathf.Lerp(1, 0, (clampedDistance - minClampDistance) / (maxClampDistance - minClampDistance));
            playerShip.transform.position += clamp * translocation;

            Vector3 gravityDirection = (transform.position - playerShip.transform.position);
            float gravity = (float)(planetRb.mass * playerShipRb.mass * gravityConstant) / ((gravityDirection.magnitude) * (gravityDirection.magnitude));
            playerShipRb.AddForce(gravityDirection.normalized * -gravity * Time.fixedDeltaTime);
        }
    }

    public Vector3 GetFutureDisplacement(Vector3 position, float time)
    {
        transform.RotateAround(lastFrameRotationPivot, Vector3.forward, movementSpeed * (time - Time.fixedDeltaTime));
        Vector3 translocation = transform.position;
        transform.RotateAround(lastFrameRotationPivot, Vector3.forward, movementSpeed * Time.fixedDeltaTime);
        translocation = transform.position - translocation;

        float clampedDistance = Mathf.Clamp(Vector3.Distance(position, this.transform.position), minClampDistance, maxClampDistance);
        float clamp = Mathf.Lerp(1, 0, (clampedDistance - minClampDistance) / (maxClampDistance - minClampDistance));

        transform.RotateAround(lastFrameRotationPivot, Vector3.forward, -movementSpeed * time);

        return clamp * translocation;
    }

    public Vector3 GetFutureGravity(Vector3 position, float time)
    {
        transform.RotateAround(lastFrameRotationPivot, Vector3.forward, movementSpeed * time);

        Vector3 gravityDirection = (transform.position - position);
        float gravity = (float)(planetRb.mass * playerShipRb.mass * gravityConstant) / ((gravityDirection.magnitude) * (gravityDirection.magnitude));
        transform.RotateAround(lastFrameRotationPivot, Vector3.forward, -movementSpeed * time);

        return gravityDirection.normalized * -gravity * Time.fixedDeltaTime;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, minClampDistance);
        Gizmos.DrawWireSphere(transform.position, maxClampDistance);
    }

    void OnMouseOver()
    {
        if (mapManager.mapActive)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (mapManager.indicatorActive)
                {
                    Destroy (mapManager.mapIndicator);
                    Destroy (mapManager.directionIndicator);
                }

                mapManager.mapIndicator = Instantiate(mapIndicator);
                mapManager.directionIndicator = Instantiate(directionIndicator);
                mapManager.directionIndicator.transform.SetParent(mainCanvas.transform);
                mapManager.myRectTransform = mapManager.directionIndicator.GetComponent<RectTransform>();
                mapManager.currentTarget = this.gameObject;
                mapManager.target = this.gameObject;
                mapManager.waypointActive = true;
                mapManager.mapIndicator.transform.position = transform.position + new Vector3(0f, ((transform.GetChild(0).GetComponent<Renderer>().bounds.size.x) / 2) + 10f, 0f);
                mapManager.indicatorActive = true;
            }
        }
    }
}