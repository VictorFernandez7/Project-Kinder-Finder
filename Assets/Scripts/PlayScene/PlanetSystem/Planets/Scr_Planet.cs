using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Scr_Planet : Scr_AstroBase
{
    [Header("Planet Info")]
    [SerializeField] public string planetName;
    [SerializeField] public PlanetType planetType;
    [SerializeField] public BlockType blockType;
    [SerializeField] public List<Scr_ReferenceManager.ResourceName> resources;

    [Header("Movement Properties")]
    [SerializeField] private float movementSpeed;
    [SerializeField] [Range(-0.1f, 0.1f)] private float rotationSpeed;

    [Header("Gravity Properties")]
    [SerializeField] private float maxClampDistance;
    [SerializeField] private float minClampDistance;

    [Header("Particle Properties")]
    [SerializeField] public float particleMultiplier;
    [SerializeField] public Material particlesMaterial;

    [Header("Internal References")]
    [SerializeField] private GameObject mapIndicator;
    [SerializeField] public Renderer renderer;
    [SerializeField] public Animator canvasAnim;
    [SerializeField] public TextMeshProUGUI canvasName;
    [SerializeField] public TextMeshProUGUI canvasType;
    [SerializeField] public TextMeshProUGUI canvasBlock;
    [SerializeField] public TextMeshProUGUI canvasResource1;
    [SerializeField] public TextMeshProUGUI canvasResource2;
    [SerializeField] public TextMeshProUGUI canvasResource3;
    [SerializeField] public TextMeshProUGUI canvasResource4;

    [Header("External References")]
    [SerializeField] private GameObject rotationPivot;
    [SerializeField] private Scr_InterfaceManager interfaceManager;
    [SerializeField] private GameObject playerShip;
    [SerializeField] private GameObject astronaut;
    [SerializeField] private GameObject mainCanvas;
    [SerializeField] private Scr_MapCamera mapCamera;
    [SerializeField] private Animator mapCanvasAnim;

    private double gravityConstant;
    private Vector3 lastFrameRotationPivot;
    private Rigidbody2D planetRb;
    private Rigidbody2D playerShipRb;
    private Rigidbody2D astronautRB;

    public enum PlanetType
    {
        EarthLike,
        Frozen,
        Volcanic,
        Arid,
        Toxic
    }

    public enum BlockType
    {
        None,
        HighTemperature,
        LowTemperature,
        Toxic
    }

    private void Start()
    {
        planetRb = GetComponent<Rigidbody2D>();

        switchGravity = true;
        lastFrameRotationPivot = rotationPivot.transform.position;
        playerShipRb = playerShip.GetComponent<Rigidbody2D>();
        astronautRB = astronaut.GetComponent<Rigidbody2D>();
        gravityConstant = 6.674 * (10 ^ -11);

        ChangePanelInfo();
    }

    private void Update()
    {
        MapAnimators();
    }

    public override void FixedUpdate()
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

    public override Vector3 GetFutureDisplacement(Vector3 position, float time)
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

    public override Vector3 GetFutureGravity(Vector3 position, float time)
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

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && interfaceManager.mapActive && !mapCamera.focus)
        {
            mapCamera.focus = true;
            mapCamera.target = gameObject;
            mapCamera.CalculateCameraPos();
            canvasAnim.SetBool("Show", true);
            mapCanvasAnim.SetBool("Show", true);
        }
    }

    private void ChangePanelInfo()
    {
        canvasName.text = planetName;
        canvasType.text = planetType.ToString();
        canvasBlock.text = blockType.ToString();
        canvasResource1.text = resources[0].ToString();
        canvasResource2.text = resources[1].ToString();
        canvasResource3.text = resources[2].ToString();
        canvasResource4.text = resources[3].ToString();
    }

    private void MapAnimators()
    {
        canvasAnim.SetBool("Show", mapCamera.focus);
        mapCanvasAnim.SetBool("Show", mapCamera.focus);
    }
}