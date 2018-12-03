using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_PlayerShipPrediction : MonoBehaviour
{
    [Header("Prediction Properties")]
    [Range(0, 7)] [SerializeField] public int predictionTime;

    [Header("References")]
    [SerializeField] LineRenderer predictionLineMap;

    [HideInInspector] public LineRenderer predictionLine;

    private int pointNumber;
    private Rigidbody2D rb;
    private Scr_PlanetManager planetManager;
    private GameObject currentPlanet;

    private void Start()
    {
        planetManager = GameObject.Find("PlanetManager").GetComponent<Scr_PlanetManager>();

        rb = GetComponent<Rigidbody2D>();
        predictionLine = GetComponent<LineRenderer>();
    }

    private void FixedUpdate()
    {
        currentPlanet = GetComponent<Scr_PlayerShipMovement>().currentPlanet;

        Prediction();
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

        int pointStartFade = points.Length / 2;
        int pointEndFade = points.Length-1;

        Gradient gradient = new Gradient();

        // Populate the color keys at the relative time 0 and 1 (0 and 100%)
        GradientColorKey[] colorKey = new GradientColorKey[1];
        colorKey[0].color = Color.white;
        colorKey[0].time = 0.0f;

        // Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
        GradientAlphaKey[] alphaKey = new GradientAlphaKey[2];
        alphaKey[0].alpha = 1.0f;
        alphaKey[0].time = pointStartFade/(float)points.Length;
        alphaKey[1].alpha = 0.0f;
        alphaKey[1].time = pointEndFade / (float)points.Length;

        gradient.SetKeys(colorKey, alphaKey);
        predictionLine.colorGradient = gradient;
    }
}