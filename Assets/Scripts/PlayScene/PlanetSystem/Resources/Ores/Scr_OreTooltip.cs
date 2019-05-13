﻿using UnityEngine;

public class Scr_OreTooltip : MonoBehaviour
{
    [Header("Paramaters")]
    [SerializeField] private float scaleRatio;
    [SerializeField] private float posRatio;

    [Header("References")]
    [SerializeField] private Camera mainCamera;

    private Vector3 initialPos;

    private void Start()
    {
        initialPos = transform.position;
    }

    void Update()
    {
        transform.localScale = Vector3.one * mainCamera.orthographicSize * scaleRatio;
        transform.position = new Vector3(initialPos.x, initialPos.y + (mainCamera.orthographicSize * posRatio), initialPos.z);
    }
}