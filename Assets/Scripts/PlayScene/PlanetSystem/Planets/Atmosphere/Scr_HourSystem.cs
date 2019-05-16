using UnityEngine.UI;
using UnityEngine;

public class Scr_HourSystem : MonoBehaviour
{
    [Header("Main Parameters")]
    [SerializeField] private bool changeColor;

    [Header("Color Parameters")]
    [SerializeField] Gradient hourColors;

    [Header("References")]
    [SerializeField] private SpriteRenderer atmosphereImage;
    [SerializeField] private Transform sun;
    [SerializeField] private Transform astronaut;
    [SerializeField] private Transform playerShip;
    [SerializeField] private Scr_SunLight sunLight;

    private float hourAngle;
    private Color32 temporaryColor;
    private Vector3 desiredVector;
    private Scr_PlayerShipMovement playerShipMovement;

    private void Start()
    {
        playerShipMovement = playerShip.GetComponent<Scr_PlayerShipMovement>();
        temporaryColor = atmosphereImage.color;
    }

    void Update()
    {
        if (changeColor)
        {
            AtmosphereColor();
            AngleCalculation();
        }

        atmosphereImage.color = temporaryColor;
    }

    private void AngleCalculation()
    {
        if (astronaut.gameObject.activeInHierarchy)
        {
            desiredVector = astronaut.position - sun.position;
            hourAngle = Vector3.Angle(desiredVector, astronaut.up);
        }

        else if (playerShipMovement.currentPlanet != null)
        {
            desiredVector = playerShip.position - sun.position;
            hourAngle = Vector3.Angle(desiredVector, playerShip.transform.position - playerShipMovement.currentPlanet.transform.position);
        }

    }

    private void AtmosphereColor()
    {
        float desiredRGB = hourAngle / 180;

        temporaryColor = hourColors.Evaluate(desiredRGB);
    }
}