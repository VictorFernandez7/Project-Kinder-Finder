using UnityEngine;

public class Scr_MapCamera : MonoBehaviour
{
    [Header("Camera Properties")]
    [SerializeField] private float initialZoom;
    [SerializeField] private float maxZoom;
    [SerializeField] private float minZoom;
    [SerializeField] private float wheelSpeed;

    [Header("Camera Movement")]
    [SerializeField] private bool inverted;
    [SerializeField] private float speedRatio;
    [SerializeField] private float moveTimer;

    [Header("Focus Properties")]
    [SerializeField] private float focusZoom;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float xOffset;

    [Header("References")]
    [SerializeField] private GameObject playerShip;
    [SerializeField] private Scr_InterfaceManager interfaceManager;

    [HideInInspector] public bool focus;
    [HideInInspector] public Animator mapCanvasAnim;
    [HideInInspector] public GameObject target;

    private float currentZoom;
    private float savedTimer;
    private Camera mapCamera;
    private Vector3 targetPos;
    private Vector3 dragOrigin;

    private void Start()
    {
        mapCamera = GetComponent<Camera>();

        currentZoom = initialZoom;
        savedTimer = moveTimer;

        mapCamera.orthographicSize = initialZoom;
        mapCamera.transform.position = new Vector3(playerShip.transform.position.x, playerShip.transform.position.y, transform.position.z);
    }

    private void Update()
    {
        if (interfaceManager.mapActive)
        {
            CameraZoom();

            if (!focus)
                MapMovement();
        }
    }

    private void MapMovement()
    {
        if (Input.GetMouseButtonDown(0))
            dragOrigin = Input.mousePosition;

        if (Input.GetMouseButton(0))
        {
            savedTimer -= Time.deltaTime;

            if (savedTimer > 0)
            {
                float targetSpeed = currentZoom / 100 * speedRatio;
                Vector3 cameraPos = GetComponent<Camera>().ScreenToViewportPoint(Input.mousePosition - dragOrigin).normalized;
                Vector3 movementVector = new Vector3(cameraPos.x, cameraPos.y, 0) * targetSpeed;

                transform.position += inverted ? movementVector : -movementVector;
            }
        }

        if (Input.GetMouseButtonUp(0))
            savedTimer = moveTimer;
    }

    public void CalculateCameraPos()
    {
        targetPos = new Vector3(target.transform.position.x + xOffset, target.transform.position.y, -100);
    }

    private void CameraZoom()
    {
        if (focus)
        {
            mapCamera.orthographicSize = Mathf.Lerp(mapCamera.orthographicSize, focusZoom, Time.unscaledDeltaTime * zoomSpeed);
            mapCamera.transform.position = Vector3.Lerp(mapCamera.transform.position, targetPos, Time.unscaledDeltaTime * moveSpeed);
        }

        else
        {
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
            currentZoom += Input.GetAxis("Mouse ScrollWheel") * -wheelSpeed;

            if (currentZoom <= maxZoom && currentZoom >= minZoom)
                mapCamera.orthographicSize = currentZoom;
        }
    }

    public void StopFocus()
    {
        focus = false;
    }
}