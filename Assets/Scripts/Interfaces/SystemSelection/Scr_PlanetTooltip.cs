using UnityEngine;
using TMPro;

public class Scr_PlanetTooltip : MonoBehaviour
{
    [Header("Select Planet Type")]
    [SerializeField] private Planet planet;

    [Header("References")]
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private TextMeshProUGUI planetTypeText;
    [SerializeField] private Scr_SystemSelectionManager systemSelectionManager;

    private GameObject planetType;

    private CircleCollider2D circleCollider;

    private enum Planet
    {
        EarthLike,
        Volcanic,
        Frozen,
        Arid,
        Moon
    }

    private void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();

        planetType = planetTypeText.transform.parent.gameObject;

        UpdatePlanetTypeText();
    }

    private void Update()
    {
        UpdateCollider();
        UpdateCanvasRotation();
    }

    private void OnMouseEnter()
    {
        planetType.SetActive(true);
    }

    private void OnMouseExit()
    {
        planetType.SetActive(false);
    }

    private void UpdateCanvasRotation()
    {
        planetType.transform.rotation = mainCamera.transform.rotation;
    }

    private void UpdateCollider()
    {
        if (systemSelectionManager.interfaceLevel == Scr_SystemSelectionManager.InterfaceLevel.System)
            circleCollider.enabled = true;

        else
            circleCollider.enabled = false;
    }

    private void UpdatePlanetTypeText()
    {
        switch (planet)
        {
            case Planet.EarthLike:
                planetTypeText.text = "Earth Like";
                break;
            case Planet.Volcanic:
                planetTypeText.text = "Volcanic";
                break;
            case Planet.Frozen:
                planetTypeText.text = "Frozen";
                break;
            case Planet.Arid:
                planetTypeText.text = "Arid";
                break;
            case Planet.Moon:
                planetTypeText.text = "Moon";
                break;
        }

        planetType.SetActive(false);
    }
}