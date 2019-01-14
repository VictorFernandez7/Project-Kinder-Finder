using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_MapCamera : MonoBehaviour
{
    [Header("Camera Properties")]
    [SerializeField] private float farZoom;
    [SerializeField] private float closeZoom;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float focusSpeed;
    [SerializeField] private float XOffset;

    [Header("Map Movement")]
    [SerializeField] private float distanceFromCenter;

    [Header("References")]
    [SerializeField] private Scr_MapManager mapManager;
    [SerializeField] private Animator zoomPanel;

    [HideInInspector] public bool focus;
    [HideInInspector] public GameObject target;

    private Camera mapCamera;

    private void Start()
    {
        mapCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        ZoomSystem();

        if (mapManager.mapActive && mapManager.canMove)
            MapMovement();
    }

    private void MapMovement()
    {
        float distance = Vector2.Distance(mapCamera.ScreenToWorldPoint(Input.mousePosition), mapCamera.transform.position);

        if (distance > distanceFromCenter)
            mapCamera.transform.Translate((mapCamera.ScreenToWorldPoint(Input.mousePosition) - mapCamera.transform.position).normalized * 1.5f, Space.World);
    }

    private void ZoomSystem()
    {
        mapCamera.orthographicSize = Mathf.Lerp(mapCamera.orthographicSize, focus ? closeZoom : farZoom, Time.deltaTime * zoomSpeed);

        if (focus)
        {
            mapCamera.transform.position = Vector3.Lerp(mapCamera.transform.position, new Vector3(target.transform.position.x + XOffset, target.transform.position.y, mapCamera.transform.position.z), focusSpeed * Time.deltaTime);

            if (mapCamera.orthographicSize <= closeZoom + 1)
                zoomPanel.SetBool("Show", true);            
        }

        else
            zoomPanel.SetBool("Show", false);
    }
}