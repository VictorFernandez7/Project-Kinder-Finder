using UnityEngine.UI;
using UnityEngine;

public class Scr_HourSystem : MonoBehaviour
{
    [Header("Main Parameters")]
    [SerializeField] private bool changeAlpha;
    [SerializeField] private bool changeColor;

    [Header("Alpha Parameters")]
    [Range(0, 255)] [SerializeField] private float dayAmount;
    [Range(0, 255)] [SerializeField] private float nightAmount;

    [Header("Lerp Parameters")]
    [SerializeField] private float lerpSpeed;

    [Header("References")]
    [SerializeField] private SpriteRenderer atmosphereImage;
    [SerializeField] private Transform sun;
    [SerializeField] private Transform astronaut;
    [SerializeField] private Scr_SunLight sunLight;

    private Color32 temporaryColor;
    private float hourAngle;
    private float alphaAmount;
    private float redAmount;
    private float greenAmount;
    private float blueAmount;
    private Vector3 sunAstronautVector;

    [SerializeField]
    Gradient colorines;

    private void Start()
    {
        temporaryColor = atmosphereImage.color;
    }

    void Update()
    {
        if (changeColor)
            AtmosphereColor();

        if (changeAlpha)
            AtmosphereAlpha();

        if (changeAlpha || changeColor)
            AngleCalculation();

        atmosphereImage.color = temporaryColor;
    }

    private void AngleCalculation()
    {
        sunAstronautVector = astronaut.position - sun.position;

        hourAngle = Vector3.Angle(sunAstronautVector, astronaut.up);

        //if (!sunLight.hitByLight)
        //    hourAngle = -hourAngle;
    }

    private void AtmosphereAlpha()
    {
        float maxDivisor = dayAmount / 180;

        if (hourAngle >= 0)
            alphaAmount = Mathf.Lerp(alphaAmount, hourAngle * maxDivisor, Time.deltaTime * lerpSpeed);

        else
            alphaAmount = Mathf.Lerp(alphaAmount, nightAmount, Time.deltaTime);

        int newAlphaAmount = (int)alphaAmount;

        //temporaryColor.a = (byte)newAlphaAmount;
    }

    private void AtmosphereColor()
    {
        float desiredRGB = hourAngle / 180;

        temporaryColor = colorines.Evaluate(desiredRGB);
    }
}