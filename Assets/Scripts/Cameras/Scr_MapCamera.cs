using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_MapCamera : MonoBehaviour
{
    [Header("Camera Properties")]
    [SerializeField] private float initialZoom;
    [SerializeField] private float maxZoom;
    [SerializeField] private float minZoom;
    [SerializeField] private float wheelSpeed;

    [Header("Focus Properties")]
    [SerializeField] private float focusZoom;
    [SerializeField] private float positionSpeed;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float xDifference;

    [Header("References")]
    [SerializeField] private Scr_MapManager mapManager;
    [SerializeField] private Animator zoomPanel;
    [SerializeField] private GameObject playerShip;

    [HideInInspector] public bool focus;
    [HideInInspector] public GameObject target;

    private float currentZoom;
    private float savedZoom;
    private float savedPosition;
    private Camera mapCamera;

    private void Start()
    {
        mapCamera = GetComponent<Camera>();

        currentZoom = initialZoom;

        mapCamera.orthographicSize = initialZoom;
        mapCamera.transform.position = new Vector3(playerShip.transform.position.x, playerShip.transform.position.y, transform.position.z);
    }

    private void Update()
    {
        if (mapManager.mapActive && mapManager.canMove)
        {
            MapMovement();
            ZoomSystem();
        }
    }

    private void MapMovement()
    {
        
    }

    private void ZoomSystem()
    {
        if (focus)
        {
            mapCamera.orthographicSize = Mathf.Lerp(mapCamera.orthographicSize, focusZoom, Time.unscaledDeltaTime * zoomSpeed);
            mapCamera.transform.position = Vector3.Lerp(mapCamera.transform.position, new Vector3(target.transform.position.x + xDifference, target.transform.position.y, mapCamera.transform.position.z), positionSpeed * Time.unscaledDeltaTime);

            if (mapCamera.orthographicSize <= focusZoom + 20)
                zoomPanel.SetBool("Show", true);
        }

        else
        {
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
            currentZoom += Input.GetAxis("Mouse ScrollWheel") * -wheelSpeed;
            mapCamera.orthographicSize = currentZoom;

            zoomPanel.SetBool("Show", false);
        }
    }
}