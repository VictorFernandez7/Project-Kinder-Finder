using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_UIRotate : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float speed;

    void Update()
    {
        transform.Rotate(speed * Vector3.forward * Time.deltaTime);
    }
}