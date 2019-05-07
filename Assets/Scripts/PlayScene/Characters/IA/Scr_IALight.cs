using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_IALight : MonoBehaviour
{
    private Light spotLight;
    private GameObject astronaut;
    private Scr_SunLight sunLight;

    void Start()
    {
        astronaut = GameObject.Find("Astronaut");
        sunLight = GameObject.Find("SunLight").GetComponent<Scr_SunLight>();
        spotLight = GetComponent<Light>();
    }

    void Update()
    {
        LightDirection();
        LightActivation();
    }

    private void LightDirection()
    {
        Vector3 targetRotation = astronaut.transform.position - transform.position;

        transform.forward = targetRotation;
    }

    private void LightActivation()
    {
        spotLight.enabled = !sunLight.hitByLight;
    }
}