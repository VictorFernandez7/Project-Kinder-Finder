using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_TrailAdapter : MonoBehaviour
{
    [Header("Parent Parameters")]
    [SerializeField] private float targetSize1;
    [SerializeField] private float targetSize2;

    [Header("References")]
    [SerializeField] private GameObject galaxy;
    [SerializeField] private GameObject system;
    [SerializeField] private Scr_SystemSelectionManager systemSelectionManager;

    private TrailRenderer trailRenderer;

    private void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();
    }

    private void Update()
    {
        if (systemSelectionManager.interfaceLevel == Scr_SystemSelectionManager.InterfaceLevel.Initial)
        {
            if (galaxy.transform.localScale.x == targetSize1)
                trailRenderer.emitting = true;

            else
            {
                trailRenderer.Clear();
                trailRenderer.emitting = false;
            }
        }

        else if (systemSelectionManager.interfaceLevel == Scr_SystemSelectionManager.InterfaceLevel.Galaxy)
        {
            if (system.transform.localScale.x == targetSize1)
                trailRenderer.emitting = true;

            else
            {
                trailRenderer.Clear();
                trailRenderer.emitting = false;
            }
        }
    }
}