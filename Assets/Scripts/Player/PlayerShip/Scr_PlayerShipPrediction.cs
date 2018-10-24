using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Scr_PlayerShipActions))]
[RequireComponent(typeof(Scr_PlayerShipStats))]
[RequireComponent(typeof(Scr_PlayerShipMovement))]

public class Scr_PlayerShipPrediction : MonoBehaviour
{
    [Header("Prediction Properties")]
    [Range(0, 7)] [SerializeField] private int predictionTime;

    [HideInInspector] public LineRenderer predictionLine;
    [HideInInspector] public LineRenderer predictionLineMap;

    private int pointNumber;
    private Scr_PlanetManager planetManager;
    private Rigidbody2D rb;

    private void Start()
    {
        planetManager = GameObject.Find("PlanetManager").GetComponent<Scr_PlanetManager>();

        rb = GetComponent<Rigidbody2D>();
        predictionLine = GetComponent<LineRenderer>();
        predictionLineMap = GetComponentInChildren<LineRenderer>();
    }

    private Vector3[] GeneratePredictionPoints()
    {
        pointNumber = Mathf.RoundToInt(predictionTime / Time.fixedDeltaTime);
        Vector3[] results = new Vector3[pointNumber];
        Vector3 moveStep = rb.velocity * Time.fixedDeltaTime;
        Vector3 currentPosition = transform.position;

        for (int i = 0; i < results.Length; ++i)
        {
            Vector3 gravity = new Vector3(0, 0, 0);
            Vector3 displacement = new Vector3(0, 0, 0);

            for (int j = 0; j < planetManager.planets.Length; ++j)
            {
                Vector3 gravityVectors = (planetManager.planets[j].GetComponent<Scr_Planet>().GetFutureGravity(currentPosition, i * Time.fixedDeltaTime));
                gravity += gravityVectors;
                displacement += planetManager.planets[j].GetComponent<Scr_Planet>().GetFutureDisplacement(currentPosition, i * Time.fixedDeltaTime);
            }

            gravity = gravity * Time.fixedDeltaTime * Time.fixedDeltaTime;
            moveStep += gravity;
            currentPosition += moveStep + displacement;
            results[i] = currentPosition;
        }

        return results;
    }

    private void Prediction()
    {
        Vector3[] points = GeneratePredictionPoints();

        predictionLine.positionCount = points.Length;
        predictionLine.SetPositions(points);
        predictionLineMap.positionCount = points.Length;
        predictionLineMap.SetPositions(points);
    }
}