using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_TrailAdapter : MonoBehaviour
{
    [Header("Parent Parameters")]
    [SerializeField] private GameObject parentObject;
    [SerializeField] private float targetSize;

    private TrailRenderer trailRenderer;

    private void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();
    }

    private void Update()
    {
        if (parentObject.transform.localScale.x == targetSize)
            trailRenderer.emitting = true;

        else
        {
            trailRenderer.Clear();
            trailRenderer.emitting = false;
        }
    }
}