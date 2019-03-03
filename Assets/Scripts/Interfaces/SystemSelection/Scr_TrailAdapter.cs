using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_TrailAdapter : MonoBehaviour
{
    [Header("Parent Parameters")]
    [SerializeField] private float targetSize;

    [Header("References")]
    [SerializeField] private GameObject targetParent;

    private TrailRenderer trailRenderer;

    private void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();
    }

    private void Update()
    {
        if (targetParent.transform.localScale.x == targetSize)
            trailRenderer.emitting = true;

        else
        {
            trailRenderer.Clear();
            trailRenderer.emitting = false;
        }
    }
}