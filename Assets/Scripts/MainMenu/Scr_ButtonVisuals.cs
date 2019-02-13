using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_ButtonVisuals : MonoBehaviour
{
    [Header("Select Materials")]
    [SerializeField] private Material[] planetMaterials;

    [Header("Rotation Parameters")]
    [SerializeField] private bool alwaysRotate;
    [SerializeField] private bool randomRotation;
    [SerializeField] private float rotationSpeed;

    [HideInInspector] public bool rotate;

    private void Start()
    {
        RandomRotation();
        AsignMaterials();

        if (alwaysRotate)
            rotate = true;
    }

    private void Update()
    {
        if (rotate)
            PlanetRotation();
    }

    private void RandomRotation()
    {
        if (randomRotation)
        {
            Vector3 newRotation = new Vector3(Random.Range(-360, 360), Random.Range(-360, 360), Random.Range(-360, 360));
            transform.rotation = Quaternion.Euler(newRotation);
        }
    }

    private void AsignMaterials()
    {
        GetComponent<MeshRenderer>().materials = planetMaterials;
    }

    private void PlanetRotation()
    {
        if (rotationSpeed > 0)
            transform.RotateAround(transform.position, transform.up, Time.deltaTime * rotationSpeed);
    }
}