using UnityEngine.UI;
using UnityEngine;

public class Scr_PlanetIndicator : MonoBehaviour
{
    [Header("Sprite References")]
    [SerializeField] private Sprite arid1;
    [SerializeField] private Sprite earthLike;
    [SerializeField] private Sprite frozen1;
    [SerializeField] private Sprite sun;
    [SerializeField] private Sprite toxic;
    [SerializeField] private Sprite volcanic1;
    [SerializeField] private Sprite moon;
    [SerializeField] private Sprite unknownPlanet;

    [HideInInspector] public Scr_Planet.PlanetType indicatorType;
    [HideInInspector] public bool discovered;
    [HideInInspector] public bool isMoon;

    private Image iconImage;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        iconImage = GetComponentInChildren<Image>();
    }

    private void Update()
    {
        SpriteChange();
        IconRotation();
    }

    private void SpriteChange()
    {
        if (discovered)
        {
            if (isMoon)
                iconImage.sprite = moon;

            else
            {
                switch (indicatorType)
                {
                    case Scr_Planet.PlanetType.EarthLike:
                        iconImage.sprite = earthLike;
                        break;
                    case Scr_Planet.PlanetType.Frozen:
                        iconImage.sprite = frozen1;
                        break;
                    case Scr_Planet.PlanetType.Volcanic:
                        iconImage.sprite = volcanic1;
                        break;
                    case Scr_Planet.PlanetType.Arid:
                        iconImage.sprite = arid1;
                        break;
                    case Scr_Planet.PlanetType.Toxic:
                        iconImage.sprite = toxic;
                        break;
                }
            }
        }

        else
            iconImage.sprite = unknownPlanet;
    }

    private void IconRotation()
    {
        Vector3 desiredRotation = mainCamera.transform.up;

        iconImage.transform.rotation = Quaternion.Euler(desiredRotation);
    }
}