using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_BlinkingSparks : MonoBehaviour
{
    [Header("Light Parameters")]
    [SerializeField] private float minTime;
    [SerializeField] private float maxTime;

    private float timeToBlink;
    private ParticleSystem sparks;

    void Start()
    {
        sparks = GetComponent<ParticleSystem>();

        timeToBlink = Random.Range(minTime, maxTime);
    }

    void Update()
    {
        timeToBlink -= Time.deltaTime;
        
        if (timeToBlink <= 0)
        {
            sparks.Play();
            timeToBlink = Random.Range(minTime, maxTime);
        }
    }
}