using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]

public class Scr_Planet : MonoBehaviour
{
    [Header("Planet Properties")]
    [Range(0.1f, 0.5f)] [SerializeField] private float movementSpeed;
    [SerializeField] float maxClampDistance;
    [SerializeField] float minClampDistance;

    [Header("References")]
    [SerializeField] private GameObject rotationPivot;
    [SerializeField] private GameObject mapIndicator;
    [SerializeField] private GameObject directionIndicator;
    [SerializeField] private Scr_MapManager mapManager;

    private double gravity;
    private GameObject playerShip;
    private Vector3 lastFrameRotationPivot;
    private Rigidbody2D planetRb;
    private Rigidbody2D playerShipRb;

    private void Start()
    {
        playerShip = GameObject.Find("PlayerShip");
        lastFrameRotationPivot = rotationPivot.transform.position;
        planetRb = GetComponent<Rigidbody2D>();
        playerShipRb = playerShip.GetComponent<Rigidbody2D>();
        gravity = 6.674 * (10 ^ -11);
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
            float gravity = (float)(planetRb.mass * playerShipRb.mass * this.gravity) / ((gravityDirection.magnitude) * (gravityDirection.magnitude));
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
        float gravity = (float)(planetRb.mass * playerShipRb.mass * this.gravity) / ((gravityDirection.magnitude) * (gravityDirection.magnitude));
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
                mapIndicator.SetActive(true);
                directionIndicator.SetActive(true);
                directionIndicator.GetComponent<Scr_MapManager>().currentTarget = this.gameObject;
                mapIndicator.GetComponent<Scr_MapManager>().target = this.gameObject;
                mapIndicator.transform.position = transform.position + new Vector3(0f, ((GetComponent<Renderer>().bounds.size.x) / 2) + 10f, 0f);
            }
        }
    }
}