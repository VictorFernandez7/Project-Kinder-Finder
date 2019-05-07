using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_IALight : MonoBehaviour
{
    [Header("Range Parameters")]
    [SerializeField] private float range;

    [Header("References")]
    [SerializeField] private Light pointLight;

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
        LightRange();
        LightDirection();
        LightActivation();
    }

    private void LightRange()
    {
        float distance = Vector3.Distance(transform.position, astronaut.transform.position);

        spotLight.range = distance * range;
    }

    private void LightDirection()
    {
        Vector3 targetRotation = astronaut.transform.position - transform.position;

        transform.forward = targetRotation;
    }

    private void LightActivation()
    {
        spotLight.enabled = !sunLight.hitByLight;
        pointLight.enabled = !sunLight.hitByLight;
    }
}