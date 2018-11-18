﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_MapManager : MonoBehaviour
{
    [Header("Map Properties")]
    [SerializeField] private float dragSpeed;
    [SerializeField] float maxZoom;
    [SerializeField] float minZoom;
    [SerializeField] float zoomSpeed;
    [SerializeField] Vector2 clampBorderSize;

    [Header("Map Properties")]
    [SerializeField] private Camera mapCamera;
    
    [HideInInspector] public GameObject target;
    [HideInInspector] public bool mapActive;
    [HideInInspector] public GameObject currentTarget;
    [HideInInspector] public bool waypointActive;
    [HideInInspector] public RectTransform myRectTransform;
    [HideInInspector] public GameObject directionIndicator;
    [HideInInspector] public GameObject mapIndicator;
    [HideInInspector] public bool indicatorActive;

    private bool clampToScreen = true;
    private Camera mainCamera;
    private Vector3 dragOrigin;
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
    }

    private void Update()
    {
        if (waypointActive)
            DirectionIndicator();

        if (Input.GetKeyDown(KeyCode.M))
        {
            mapCamera.gameObject.SetActive(!mapActive);
            mainCanvas.SetActive(mapActive);
            mapCanvas.SetActive(!mapActive);
            mapActive = !mapActive;

            if (mapActive)
                Time.timeScale = 0.1f;
            else if (!mapActive)
                Time.timeScale = 1f;
        }

        if (mapActive)
            MinimapControl();

        if(target != null)
            mapIndicator.transform.position = target.transform.position + new Vector3(0f, ((target.transform.GetChild(1).GetComponent<Renderer>().bounds.size.x) / 2) + 10f, 0f);
    }

    private void MinimapControl()
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

    private void DirectionIndicator()
    {
        Vector3 noClampPosition = mainCamera.WorldToScreenPoint(currentTarget.transform.position + Vector3.zero);
        Vector3 clampedPosition = new Vector3(Mathf.Clamp(noClampPosition.x, 0 + clampBorderSize.x, Screen.width - clampBorderSize.x), Mathf.Clamp(noClampPosition.y, 0 + clampBorderSize.y, Screen.height - clampBorderSize.y), noClampPosition.z);

        myRectTransform.position = clampToScreen ? clampedPosition : noClampPosition;

        Vector3 difference = (currentTarget.transform.position - playerShip.transform.position);
        difference.Normalize();

        float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        directionIndicator.transform.rotation = Quaternion.Euler(0f, 0f, rotation_z + 90f);
    }
}