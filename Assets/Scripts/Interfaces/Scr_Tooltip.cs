using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;

public class Scr_Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Tooltip Type")]
    [SerializeField] private bool staticTooltip;

    [Header("Type Tooltip Text")]
    [TextArea] [SerializeField] public string tipText;

    [Header("Text Parameters")]
    [SerializeField] private float xPos;
    [SerializeField] private float yPos;
    [SerializeField] private float fontSize;

    [Header("References")]
    [SerializeField] private GameObject tooltip;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform tooltipPos;

    [HideInInspector] public bool isItem;
    [HideInInspector] public bool isJustActive;

    private bool mouseOver;
    private TextMeshProUGUI tooltipText;

    private void Start()
    {
        tooltipText = tooltip.GetComponentInChildren<TextMeshProUGUI>();
        isItem = true;
    }

    private void Update()
    {
        if (mouseOver)
            ToolTipMovement();

        if (isItem && isJustActive)
        {
            ActivateTooltip();
            isJustActive = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isItem)
            ActivateTooltip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;

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

    private void ActivateTooltip()
    {
        mouseOver = true;

        tooltipText.text = tipText;
        tooltipText.fontSize = fontSize;
        tooltip.SetActive(true);

        if (!staticTooltip)
            tooltipText.transform.localPosition = new Vector3(xPos, yPos, tooltip.transform.position.z);
    }
}