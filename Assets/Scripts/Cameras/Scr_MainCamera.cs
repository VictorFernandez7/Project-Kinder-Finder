using EZCameraShake;
using UnityEngine;

public class Scr_MainCamera : MonoBehaviour
{
    [Header("Zoom Properties")]
    [SerializeField] private float zoomInSpace;
    [SerializeField] private float zoomInPlanet;
    [SerializeField] private float miningZoom;
    [SerializeField] private float craftZoom;
    [SerializeField] private float zoomSpeed;

    [Header("Rotation Properties")]
    [SerializeField] private float shipRotationSpeed;
    [SerializeField] private float astronautRotationSpeed;

    [Header("Shake Properties")]
    [SerializeField] private float magnitude = 2f;
    [SerializeField] private float roughness = 10f;
    [SerializeField] private float fadeOutTime = 5f;

    [Header("References")]
    [SerializeField] private GameObject playerShip;
    [SerializeField] private GameObject astronaut;

    [HideInInspector] public bool followAstronaut = true;
    [HideInInspector] public bool smoothRotation = true;
    [HideInInspector] public bool mining;
    [HideInInspector] public bool interacting;
    [HideInInspector] public GameObject craftCenter;

    private float zoomDistance;
    private Vector3 desiredUp;
    private Camera mainCamera;
    private GameObject currentPlanet;
    private Scr_PlayerShipMovement playerShipMovement;
    private Scr_PlayerShipActions playerShipActions;
    private CameraShakeInstance shakeInstance;

    private void Start()
    {
        playerShipMovement = playerShip.GetComponent<Scr_PlayerShipMovement>();
        playerShipActions = playerShip.GetComponent<Scr_PlayerShipActions>();
        mainCamera = GetComponent<Camera>();
        desiredUp = transform.up;

        mainCamera.orthographicSize = zoomInPlanet;
        smoothRotation = true;

        shakeInstance = CameraShaker.Instance.StartShake(magnitude, roughness, fadeOutTime);
        shakeInstance.StartFadeOut(0);
        shakeInstance.DeleteOnInactive = true;
    }

    private void Update()
    {
        currentPlanet = playerShipMovement.currentPlanet;
        zoomDistance = playerShipMovement.landDistance;

        CameraZoom();
        CameraPosition();
        CameraRotation();
    }

    private void CameraPosition()
    {
        if (followAstronaut)
        {
            if (interacting)
                transform.position = new Vector3(craftCenter.transform.position.x, craftCenter.transform.position.y, -100);

            else
                transform.position = new Vector3(astronaut.transform.position.x, astronaut.transform.position.y, -100);
        }

        else
            transform.position = new Vector3(playerShip.transform.position.x, playerShip.transform.position.y, -100);
    }

    private void CameraRotation()
    {
        if (smoothRotation)
        {
            if (interacting)
            {
                Vector3 craftCenterUp = craftCenter.transform.up;

                desiredUp = Vector3.Lerp(desiredUp, craftCenterUp, Time.deltaTime);

                transform.rotation = Quaternion.LookRotation(transform.forward,desiredUp);
            }

            else
            {
                Vector3 astronautUpVector = astronaut.transform.up;
                Vector3 playerShipVectorUp = playerShip.transform.up;

                desiredUp = Vector3.Lerp(desiredUp, followAstronaut ? astronautUpVector : playerShipVectorUp, Time.deltaTime * (followAstronaut ? astronautRotationSpeed : shipRotationSpeed));

                transform.rotation = Quaternion.LookRotation(transform.forward, followAstronaut ? astronautUpVector : desiredUp);
            }
        }
    }

    private void CameraZoom()
    {
        if (playerShipMovement.takingOff || playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.inSpace)
        {
            if (mining)
            {
                if (playerShipActions.doingSpaceWalk)
                {
                    float spaceWalkZoom = Vector3.Distance(astronaut.transform.position, playerShip.transform.position);
                    spaceWalkZoom = Mathf.Clamp(spaceWalkZoom, 4, 7);
                    mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, spaceWalkZoom, Time.deltaTime * zoomSpeed * 2);
                }

                else
                    mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, miningZoom, Time.deltaTime * zoomSpeed);
            }

            else if (playerShipActions.doingSpaceWalk)
            {
                float spaceWalkZoom = Vector3.Distance(astronaut.transform.position, playerShip.transform.position);
                spaceWalkZoom = Mathf.Clamp(spaceWalkZoom, 2, 7);
                mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, spaceWalkZoom, Time.deltaTime * zoomSpeed * 2);
            }
                
            else
                mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, zoomInSpace, Time.deltaTime * zoomSpeed);
        }

        if (playerShipMovement.landing || playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.landed)
        {
            if (interacting)
                mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, craftZoom, Time.deltaTime * zoomSpeed);

            else
                mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, zoomInPlanet, Time.deltaTime * zoomSpeed);
        }
    }

    public void CameraShake()
    {
        CameraShaker.Instance.ShakeOnce(magnitude, roughness, 0, fadeOutTime);
    }

    public void CameraStartShake(bool fadeIn)
    {
        if (fadeIn)
            shakeInstance.StartFadeIn(1f);
        else
            shakeInstance.StartFadeOut(3f);
    }
}