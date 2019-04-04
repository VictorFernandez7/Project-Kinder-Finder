using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_LanternCorrection : MonoBehaviour
{
    [SerializeField] private GameObject transforms;

    void Update()
    {
        if (transforms.transform.rotation.y == 180)
            transform.position = new Vector3(transform.position.x, transform.position.y, -0.5f);

        else
            transform.position = new Vector3(transform.position.x, transform.position.y, 0.5f);
    }
}
