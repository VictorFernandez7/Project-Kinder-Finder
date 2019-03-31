using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;

public class Scr_Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Tooltip Type")]
    [SerializeField] private bool staticTooltip;

    [Header("Type Tooltip Text")]
    [TextArea] [SerializeField] private string tipText;

    [Header("Text Parameters")]
    [SerializeField] private float xPos;
    [SerializeField] private float yPos;
    [SerializeField] private float fontSize;

    [Header("References")]
    [SerializeField] private GameObject tooltip;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform tooltipPos;

    private TextMeshProUGUI tooltipText;

    private void Start()
    {
        tooltipText = tooltip.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (tooltip.activeInHierarchy)
            ToolTipMovement();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltipText.text = tipText;
        tooltipText.fontSize = fontSize;
        tooltip.SetActive(true);

        if (!staticTooltip)
            tooltipText.transform.localPosition = new Vector3(xPos, yPos, tooltip.transform.position.z);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.SetActive(false);
    }

    private void ToolTipMovement()
    {
        if (!staticTooltip)
        {
            Vector3 targetPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, tooltip.transform.position.z);

            tooltip.transform.position = targetPos;
        }

        else
            tooltip.transform.position = tooltipPos.position;
    }
}