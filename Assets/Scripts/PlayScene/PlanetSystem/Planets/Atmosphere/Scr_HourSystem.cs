using UnityEngine.UI;
using UnityEngine;

public class Scr_HourSystem : MonoBehaviour
{
    [Header("Alpha Parameters")]
    [Range(400, 600)] [SerializeField] private float dayRelation;
    [Range(0, 0.3f)] [SerializeField] private float nightAmount;

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

    private Color temporaryColor;
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
        if (hourAngle >= 0)
            alphaAmount = Mathf.Lerp(alphaAmount, hourAngle / dayRelation, Time.deltaTime * lerpSpeed);

        else
            alphaAmount = Mathf.Lerp(alphaAmount, nightAmount, Time.deltaTime);

        temporaryColor.a = alphaAmount;
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

        temporaryColor.r = redAmount;
        temporaryColor.b = blueAmount;
    }
}