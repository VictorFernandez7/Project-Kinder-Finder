using UnityEngine;
using TMPro;

public class Scr_PlanetTooltip : MonoBehaviour
{
    [Header("Select System")]
    [SerializeField] private Scr_Levels.LevelToLoad system;

    [Header("Select Planet Type")]
    [SerializeField] private Planet planet;

    [Header("References")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject toolTip;
    [SerializeField] private Scr_SystemSelectionManager systemSelectionManager;

    private TextMeshProUGUI tooltipText;
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
        tooltipText = toolTip.GetComponentInChildren<TextMeshProUGUI>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        UpdateCollider();

        if (toolTip.activeInHierarchy)
            ToolTipMovement();
    }

    private void OnMouseEnter()
    {
        UpdateSize();
        UpdatePlanetTypeText();

        toolTip.SetActive(true);
    }

    private void OnMouseExit()
    {
        toolTip.SetActive(false);
    }

    private void UpdateCollider()
    {
        if (systemSelectionManager.interfaceLevel == Scr_SystemSelectionManager.InterfaceLevel.System)
            circleCollider.enabled = true;

        else
            circleCollider.enabled = false;
    }

    private void UpdateSize()
    {
        switch (system)
        {
            case Scr_Levels.LevelToLoad.PlanetSystem1:
                toolTip.transform.localScale = 1.7f * Vector3.one;
                break;
            case Scr_Levels.LevelToLoad.PlanetSystem2:
                toolTip.transform.localScale = 2.9f * Vector3.one;
                break;
        }
    }

    private void ToolTipMovement()
    {
        Vector3 targetPos = new Vector3(mainCamera.ScreenToWorldPoint(Input.mousePosition).x, mainCamera.ScreenToWorldPoint(Input.mousePosition).y, toolTip.transform.position.z);

        toolTip.transform.position = targetPos;
    }

    private void UpdatePlanetTypeText()
    {
        switch (planet)
        {
            case Planet.EarthLike:
                tooltipText.text = "Earth Like";
                break;
            case Planet.Volcanic:
                tooltipText.text = "Volcanic";
                break;
            case Planet.Frozen:
                tooltipText.text = "Frozen";
                break;
            case Planet.Arid:
                tooltipText.text = "Arid";
                break;
            case Planet.Moon:
                tooltipText.text = "Moon";
                break;
        }
    }
}