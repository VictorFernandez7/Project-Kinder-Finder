using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_SimpleRotation : MonoBehaviour
{
    [Header("Planet Parameters")]
    [SerializeField] private bool initialRandomRot;

    [Header("References")]
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float translationSpeed;

    [Header("References")]
    [SerializeField] private Transform pivot;

    private void Start()
    {
        if (initialRandomRot)
        {
            Vector3 newRotation = new Vector3(Random.Range(-360, 360), Random.Range(-360, 360), Random.Range(-360, 360));

            transform.rotation = Quaternion.Euler(newRotation);
        }
    }

    private void Update()
    {
        if (rotationSpeed > 0)
            transform.RotateAround(transform.position, transform.up, Time.deltaTime * rotationSpeed);

        if (translationSpeed > 0)
            transform.RotateAround(pivot.position, Vector3.forward, Time.deltaTime * translationSpeed);
    }
}