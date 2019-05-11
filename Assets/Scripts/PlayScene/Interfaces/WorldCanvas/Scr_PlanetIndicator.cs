using UnityEngine.UI;
using UnityEngine;

public class Scr_PlanetIndicator : MonoBehaviour
{
    [Header("Sprite References")]
    [SerializeField] private Sprite arid1;
    [SerializeField] private Sprite arid2;
    [SerializeField] private Sprite earthLike;
    [SerializeField] private Sprite frozen1;
    [SerializeField] private Sprite frozen2;
    [SerializeField] private Sprite sun;
    [SerializeField] private Sprite toxic;
    [SerializeField] private Sprite volcanic1;
    [SerializeField] private Sprite volcanic2;

    [HideInInspector] public Scr_Planet.PlanetType indicatorType;

    private Image iconImage;

    private void Start()
    {
        iconImage = GetComponentInChildren<Image>();

        switch (indicatorType)
        {
            case Scr_Planet.PlanetType.EarthLike:
                iconImage.sprite = earthLike;
                break;
            case Scr_Planet.PlanetType.Frozen:
                int random1 = Random.Range(0, 2);
                if (random1 == 0)
                    iconImage.sprite = frozen1;
                else
                    iconImage.sprite = frozen2;
                break;
            case Scr_Planet.PlanetType.Volcanic:
                int random2 = Random.Range(0, 2);
                if (random2 == 0)
                    iconImage.sprite = volcanic1;
                else
                    iconImage.sprite = volcanic2;
                break;
            case Scr_Planet.PlanetType.Arid:
                int random3 = Random.Range(0, 2);
                if (random3 == 0)
                    iconImage.sprite = arid1;
                else
                    iconImage.sprite = arid2;
                break;
            case Scr_Planet.PlanetType.Toxic:
                iconImage.sprite = toxic;
                break;
        }
    }
}