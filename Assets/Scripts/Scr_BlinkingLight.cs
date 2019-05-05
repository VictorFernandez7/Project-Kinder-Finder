using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_BlinkingLight : MonoBehaviour
{
    [Header("Light Parameters")]
    [SerializeField] private float minTime;
    [SerializeField] private float maxTime;

    private float timeToBlink;
    private bool blinking;
    private Light spotLight;

    void Start()
    {
        spotLight = GetComponent<Light>();

        timeToBlink = Random.Range(minTime, maxTime);
    }

    void Update()
    {
        if (!blinking)
            timeToBlink -= Time.deltaTime;

        if (timeToBlink <= 0)
        {
            blinking = true;
            timeToBlink = Random.Range(minTime, maxTime);
            Blink();
        }
    }

    private void Blink()
    {
        spotLight.enabled = false;

        Invoke("TurnOnLight", 0.5f);
    }

    private void TurnOnLight()
    {
        spotLight.enabled = true;
        blinking = false;
    }
}