using UnityEngine.UI;
using UnityEngine;

public class Scr_HourSystem : MonoBehaviour
{
    [Header("Alpha Parameters")]
    [Range(0, 255)] [SerializeField] private float dayAmount;
    [Range(0, 255)] [SerializeField] private float nightAmount;

    [Header("Day's Color Parameters")]
    [Range(0, 255)] [SerializeField] private float maxRed;
    [Range(0, 255)] [SerializeField] private float minRed;
    [Range(0, 255)] [SerializeField] private float maxGreen;
    [Range(0, 255)] [SerializeField] private float minGreen;
    [Range(0, 255)] [SerializeField] private float maxBlue;
    [Range(0, 255)] [SerializeField] private float minBlue;

    [Header("Night's Color Parameters")]
    [Range(0, 255)] [SerializeField] private float nightRed;
    [Range(0, 255)] [SerializeField] private float nightGreen;
    [Range(0, 255)] [SerializeField] private float nightBlue;

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

    private void Start()
    {
        temporaryColor = atmosphereImage.color;
    }

    void Update()
    {
        AtmosphereColor();
        AtmosphereAlpha();
        AngleCalculation();

        atmosphereImage.color = temporaryColor;
    }

    private void AngleCalculation()
    {
        sunAstronautVector = astronaut.position - sun.position;

        hourAngle = Vector3.Angle(sunAstronautVector, astronaut.up);

        if (!sunLight.hitByLight)
            hourAngle = -hourAngle;
    }

    private void AtmosphereAlpha()
    {
        float maxDivisor = dayAmount / 180;

        if (hourAngle >= 0)
            alphaAmount = Mathf.Lerp(alphaAmount, hourAngle * maxDivisor, Time.deltaTime * lerpSpeed);

        else
            alphaAmount = Mathf.Lerp(alphaAmount, nightAmount, Time.deltaTime);

        int newAlphaAmount = (int)alphaAmount;

        temporaryColor.a = (byte)newAlphaAmount;
    }

    private void AtmosphereColor()
    {
        if (hourAngle >= 0)
            DayRGB();

        else
            NightRGB();

        int newRedAmount = (int)redAmount;
        int newGreenAmount = (int)greenAmount;
        int newBlueAmount = (int)blueAmount;

        temporaryColor.r = (byte)newRedAmount;
        temporaryColor.g = (byte)newGreenAmount;
        temporaryColor.b = (byte)newBlueAmount;
    }

    private void DayRGB()
    {
        float redDivisor = maxRed / 180 * 2;
        float greenDivisor = maxGreen / 180 * 2;
        float blueDivisor = maxBlue / 180 * 2;

        float redHourAngle = hourAngle - 90;
        float blueHourAngle = hourAngle - 180;

        redAmount = Mathf.Lerp(redAmount, redHourAngle * redDivisor, Time.deltaTime * lerpSpeed);

        greenAmount = 255;

        if (blueHourAngle > 0)
            blueAmount = Mathf.Lerp(blueAmount, blueHourAngle * blueDivisor, Time.deltaTime * lerpSpeed);

        else
            blueAmount = Mathf.Lerp(blueAmount, blueHourAngle * -blueDivisor, Time.deltaTime * lerpSpeed);
    }

    private void NightRGB()
    {

    }
}