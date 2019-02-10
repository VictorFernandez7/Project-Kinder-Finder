using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_OrbitAdapter : MonoBehaviour
{
    [Header("Line Parameters")]
    [SerializeField] private float lineWidth;

    [Header("References")]
    [SerializeField] private Camera mainCamera;

    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (mainCamera != null)
        {
            lineRenderer.startWidth = mainCamera.orthographicSize * lineWidth;
            lineRenderer.endWidth = mainCamera.orthographicSize * lineWidth;
        }        
    }
}