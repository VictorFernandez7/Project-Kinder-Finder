using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]

public class Scr_Planet : MonoBehaviour
{
    [Header("Planet Properties")]
    [SerializeField] private float movementSpeed;
    [SerializeField] float maxClampDistance;
    [SerializeField] float minClampDistance;

    [Header("References")]
    [SerializeField] private GameObject rotationPivot;
    [SerializeField] private GameObject mapIndicator;
    [SerializeField] private GameObject directionIndicator;
    [SerializeField] private Scr_MapManager mapManager;
    [SerializeField] private GameObject mapVisuals;
    [SerializeField] private GameObject canvas;

    private double gravityConstant;
    private GameObject playerShip;
    private GameObject astronaut;
    private Vector3 lastFrameRotationPivot;
    private Rigidbody2D planetRb;
    private Rigidbody2D playerShipRb;
    private Rigidbody2D astronautRB;

    private void Start()
    {
        playerShip = GameObject.Find("PlayerShip");
        astronaut = GameObject.Find("Astronaut");
        lastFrameRotationPivot = rotationPivot.transform.position;
        planetRb = GetComponent<Rigidbody2D>();
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

        if (playerShip.GetComponent<Scr_PlayerShipMovement>().onGround == false)
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
                mapIndicator = Instantiate(mapIndicator);
                directionIndicator = Instantiate(directionIndicator);
                mapManager.mapIndicator = mapIndicator;
                mapManager.directionIndicator = directionIndicator;
                directionIndicator.transform.SetParent(canvas.transform);
                mapManager.myRectTransform = directionIndicator.GetComponent<RectTransform>();
                mapManager.currentTarget = this.gameObject;
                mapManager.target = this.gameObject;
                mapManager.waypointActive = true;
                mapIndicator.transform.position = transform.position + new Vector3(0f, ((transform.GetChild(1).GetComponent<Renderer>().bounds.size.x) / 2) + 10f, 0f);
            }
        }
    }
}