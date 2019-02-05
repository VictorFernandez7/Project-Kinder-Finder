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
        transform.RotateAround(pivot.position, transform.up, Time.deltaTime * rotationSpeed);
        transform.RotateAround(pivot.position, Vector3.forward, Time.deltaTime * translationSpeed);
    }
}