using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Clouds : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;

    [Header("Direction Timer")]
    [SerializeField] private float D_minTime;
    [SerializeField] private float D_maxTime;

    [Header("Speed Timer")]
    [SerializeField] private float S_minTime;
    [SerializeField] private float S_maxTime;

    private int targetDirection;
    private float targetSpeed;
    private float timeToSpeed;
    private float timeToDirection;

    private void Start()
    {
        timeToSpeed = Random.Range(S_minTime, S_maxTime);
        timeToDirection = Random.Range(D_minTime, D_maxTime);
        targetSpeed = Random.Range(minSpeed, maxSpeed);
        targetDirection = Random.Range(-1, 2);

        if (targetDirection == 0)
            targetDirection = 1;
    }

    void Update()
    {
        Timers();
        Movement();
    }

    private void Timers()
    {
        timeToSpeed -= Time.deltaTime;
        timeToDirection -= Time.deltaTime;

        if (timeToSpeed <= 0)
        {
            targetSpeed = Random.Range(minSpeed, maxSpeed);
            timeToSpeed = Random.Range(S_minTime, S_maxTime);
        }

        if (timeToDirection <= 0)
        {
            targetDirection = Random.Range(-1, 1);

            if (targetDirection == 0)
                targetDirection = 1;

            timeToDirection = Random.Range(S_minTime, S_maxTime);
        }
    }

    private void Movement()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * targetSpeed * targetDirection);
    }
}