using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_MapCamera : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Scr_MapManager mapManager;
    [SerializeField] private float distanceFromCenter;
    [SerializeField] private Camera mapCamera;

    private void Update()
    {
        if (mapManager.mapActive)
            MapMovement();
    }

    private void MapMovement()
    {
        float distance = Vector2.Distance(mapCamera.ScreenToWorldPoint(Input.mousePosition), mapCamera.transform.position);

        if (distance > distanceFromCenter)
            mapCamera.transform.Translate((mapCamera.ScreenToWorldPoint(Input.mousePosition) - mapCamera.transform.position).normalized * 1.5f, Space.World);
    }
}
