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
    [SerializeField] private Transform scaledParent;

    [HideInInspector] public bool rotate;

    private void Start()
    {
        RandomRotation();
        AsignMaterials();
    }

    private void Update()
    {
        PlanetRotation(rotate, alwaysRotate);
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

    private void PlanetRotation(bool rotate, bool dontCheck)
    {
        if (rotationSpeed > 0 && rotate && (scaledParent == null || scaledParent != null && scaledParent.localScale.x == 1))
            transform.RotateAround(transform.position, transform.up, Time.deltaTime * rotationSpeed);
        
        if (dontCheck)
            transform.RotateAround(transform.position, transform.up, Time.deltaTime * rotationSpeed);
    }
}