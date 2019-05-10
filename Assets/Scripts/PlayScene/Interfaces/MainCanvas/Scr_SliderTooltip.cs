using UnityEngine.EventSystems;
using UnityEngine;

public class Scr_SliderTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("References")]
    [SerializeField] private Scr_InterfaceManager interfaceManager;

    private Animator tooltips;

    private void Start()
    {
        tooltips = GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (interfaceManager.tooltipsOn)
            tooltips.SetBool("Show", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (interfaceManager.tooltipsOn)
            tooltips.SetBool("Show", false);
    }
}