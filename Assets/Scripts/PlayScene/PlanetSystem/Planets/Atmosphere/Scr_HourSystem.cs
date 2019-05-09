using UnityEngine.UI;
using UnityEngine;

public class Scr_HourSystem : MonoBehaviour
{
    [Header("Alpha Parameters")]
    [Range(0, 255)] [SerializeField] private float dayAmount;
    [Range(0, 255)] [SerializeField] private float nightAmount;

    [Header("Lerp Parameters")]
    [SerializeField] private float lerpSpeed;

    [Header("Color Parameters")]
    [SerializeField] private Color nightColor;
    [SerializeField] private Color dayColor;

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
        temporaryColor = nightColor;
    }

    void Update()
    {
        //AtmosphereColor();
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
            alphaAmount = Mathf.Lerp(alphaAmount, hourAngle / 2 * maxDivisor, Time.deltaTime * lerpSpeed);

        else
            alphaAmount = Mathf.Lerp(alphaAmount, nightAmount, Time.deltaTime);

        int newAlphaAmount = (int)alphaAmount;

        temporaryColor.a = (byte)newAlphaAmount;
    }

    private void AtmosphereColor()
    {
        float maxDivisor = 2.83f;

        float redHourAngle = hourAngle / 2;
        float blueHourAngle = hourAngle - 180;

        if (hourAngle >= 0)
        {
            redAmount = Mathf.Lerp(redAmount, redHourAngle * maxDivisor, Time.deltaTime * lerpSpeed);

            if (blueHourAngle > 0)
                blueAmount = Mathf.Lerp(blueAmount, blueHourAngle * maxDivisor, Time.deltaTime * lerpSpeed);

            else
                blueAmount = Mathf.Lerp(blueAmount, blueHourAngle * -maxDivisor, Time.deltaTime * lerpSpeed);
        }

        else
        {

        }

        //print(redAmount);

        int newRedAmount = (int)redAmount;
        int newBlueAmount = (int)blueAmount;

        temporaryColor.r = (byte)newRedAmount;
        temporaryColor.b = (byte)newBlueAmount;
    }
}