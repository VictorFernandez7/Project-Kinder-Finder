using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_TrailAdapter : MonoBehaviour
{
    [Header("Parent Parameters")]
    [SerializeField] private float targetSize;

    [Header("References")]
    [SerializeField] private GameObject targetParent;
    [SerializeField] private Scr_SystemSelectionManager systemSelectionManager;

    private TrailRenderer trailRenderer;

    private void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();
    }

    private void Update()
    {
        CheckState();
    }

    private void CheckState()
    {
        if (systemSelectionManager.interfaceLevel == Scr_SystemSelectionManager.InterfaceLevel.Initial || systemSelectionManager.interfaceLevel == Scr_SystemSelectionManager.InterfaceLevel.Galaxy)
        {
            if (targetParent.transform.localScale.x == targetSize)
                trailRenderer.emitting = true;

            else
                StopEmitting();
        }

        else if (systemSelectionManager.interfaceLevel == Scr_SystemSelectionManager.InterfaceLevel.System && targetParent.transform.localScale.x == 1)
            trailRenderer.emitting = true;

        else
            StopEmitting();
    }

    private void StopEmitting()
    {
        trailRenderer.Clear();
        trailRenderer.emitting = false;
    }
}