using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scr_MapManager : MonoBehaviour
{
    [Header("Map Properties")]
    [SerializeField] private float dragSpeed;
    [SerializeField] float maxZoom;
    [SerializeField] float minZoom;
    [SerializeField] float zoomSpeed;
    [SerializeField] Vector2 clampBorderSize;

    [Header("Map Properties")]
    [SerializeField] private float distance;

    [Header("References")]
    [SerializeField] private Camera mapCamera;
    [SerializeField] private GameObject mapVisuals;
    [SerializeField] private TextMeshProUGUI distanceText;

    [HideInInspector] public bool mapActive;
    [HideInInspector] public bool waypointActive;
    [HideInInspector] public bool indicatorActive;
    [HideInInspector] public GameObject target;
    [HideInInspector] public GameObject currentTarget;
    [HideInInspector] public GameObject directionIndicator;
    [HideInInspector] public GameObject mapIndicator;
    [HideInInspector] public RectTransform myRectTransform;

    private bool clampToScreen = true;
    private bool slow;
    private Camera mainCamera;
    private Vector3 dragOrigin;
    private float distanceHUD;
    private GameObject mainCanvas;
    private GameObject mapCanvas;
    private GameObject playerShip;

    private void Start()
    {
        playerShip = GameObject.Find("PlayerShip");
        mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        mainCanvas = GameObject.Find("MainCanvas");
        mapCanvas = GameObject.Find("MapCanvas");

        mapCanvas.SetActive(false);
        currentTarget = null;

        distanceHUD = Vector3.Distance(distanceText.transform.position, playerShip.transform.position);
    }

    private void Update()
    {
        Halo();
        MapActivation();

        if (mapActive)
            MapControl();

        if (target != null)
            mapIndicator.transform.position = target.transform.position + new Vector3(0f, ((target.transform.GetChild(0).GetComponent<Renderer>().bounds.size.x) / 2) + 10f, 0f);

        if (Input.GetMouseButtonDown(1))
        {
            Destroy(mapIndicator);
            Destroy(directionIndicator);
            indicatorActive = false;
            waypointActive = false;
            target = null;
        }
    }

    private void MapActivation()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            playerShip.GetComponent<Scr_PlayerShipMovement>().canControlShip = mapActive;
            mapCamera.gameObject.SetActive(!mapActive);
            mainCanvas.SetActive(mapActive);
            mapCanvas.SetActive(!mapActive);
            mapActive = !mapActive;
            mapVisuals.SetActive(mapActive);
        }

        else if (mapActive)
        {
            playerShip.GetComponent<Scr_PlayerShipPrediction>().enabled = false;
            Time.timeScale = 0f;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            slow = true;
        }

        else if (!mapActive && Time.timeScale < 1 && slow == true)
        {
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
            Time.timeScale += 0.25f * Time.unscaledDeltaTime;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            if (Time.timeScale >= 1 )
            {
                Time.timeScale = 1;
                playerShip.GetComponent<Scr_PlayerShipPrediction>().enabled = true;
                slow = false;
            }
        }
    }

    private void MapControl()
    {
        float zoom = mapCamera.orthographicSize;
        zoom += Input.GetAxis("Mouse ScrollWheel") * -zoomSpeed;
        zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
        mapCamera.orthographicSize = zoom;

        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = mapCamera.ScreenToViewportPoint(Input.mousePosition);
            return;
        }

        if (!Input.GetMouseButton(0))
            return;

        Vector3 mousePosition = mapCamera.ScreenToViewportPoint(Input.mousePosition) - dragOrigin;
        Vector3 newPosition = new Vector3(mousePosition.x * dragSpeed, mousePosition.y * dragSpeed, 0f);

        mapCamera.transform.Translate(newPosition, Space.World);
    }

    float totalDistance;
    float currentDistance;
    float thousands;
    float hundreds;
    string lowHundreds;
    bool lowHundredsEnabled = false;

    private void Halo()
    {
        distanceText.transform.up = mainCamera.transform.up;
        distanceText.transform.position = playerShip.transform. position + (mainCamera.transform.up * distanceHUD);

        if (currentTarget != null)
        {
            currentDistance = Vector3.Distance(playerShip.transform.position, currentTarget.transform.position);
            totalDistance = currentDistance * 1000;
            thousands = totalDistance / 1000;
            hundreds = totalDistance - ((int) thousands * 1000);

            if (hundreds >= 0 && hundreds <= 99)
            {
                lowHundredsEnabled = true;

                if (hundreds >= 0 && hundreds <= 9)
                    lowHundreds = "00" + (int) hundreds;

                else if (hundreds >= 10 && hundreds <= 99)
                    lowHundreds = "0" + (int) hundreds;
            }

            else
                lowHundredsEnabled = false;
        }


        if (lowHundredsEnabled)
            distanceText.text = "" + (int) thousands + "." + lowHundreds + " km";

        else
            distanceText.text = "" + (int)thousands + "." + (int) hundreds + " km";

        if (waypointActive)
        {
            DirectionIndicator();

            if (playerShip.GetComponent<Scr_PlayerShipMovement>().playerShipState == Scr_PlayerShipMovement.PlayerShipState.inSpace)
            {
                directionIndicator.GetComponent<SVGImage>().enabled = true;
                distanceText.gameObject.SetActive(true);
            }

            else
            {
                directionIndicator.GetComponent<SVGImage>().enabled = false;
                distanceText.gameObject.SetActive(false);
            }
        }
    }

    private void DirectionIndicator()
    {
        Vector3 difference = (currentTarget.transform.position - playerShip.transform.position);
        difference.Normalize();

        float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        directionIndicator.transform.rotation = Quaternion.Euler(0f, 0f, rotation_z + 90f - mainCamera.transform.rotation.eulerAngles.z);

        directionIndicator.transform.localPosition = -directionIndicator.transform.up * distance;
    }
}