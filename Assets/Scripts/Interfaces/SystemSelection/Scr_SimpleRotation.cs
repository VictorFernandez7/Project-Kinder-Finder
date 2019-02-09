using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_SimpleRotation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float translationSpeed;

    [Header("References")]
    [SerializeField] private Transform pivot;

    private void Update()
    {
        if (rotationSpeed > 0)
            transform.RotateAround(transform.position, transform.up, Time.deltaTime * rotationSpeed);

        if (translationSpeed > 0)
            transform.RotateAround(pivot.position, Vector3.forward, Time.deltaTime * translationSpeed);
    }
}