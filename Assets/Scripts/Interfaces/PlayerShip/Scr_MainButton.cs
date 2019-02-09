using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;

public class Scr_MainButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Select Button")]
    [SerializeField] private DesiredButton desiredButton;

    [Header("References")]
    [SerializeField] private Animator filesAnim;

    private Animator anim;

    private enum DesiredButton
    {
        Warehouse,
        Files,
        Planets,
        Treasure
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        anim.SetBool("ShowText", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        anim.SetBool("ShowText", false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (desiredButton)
        {
            case DesiredButton.Warehouse:
                filesAnim.SetBool("ShowButtons", false);
                break;
            case DesiredButton.Files:
                filesAnim.SetBool("ShowButtons", true);
                break;
            case DesiredButton.Planets:
                break;
            case DesiredButton.Treasure:
                break;
        }
    }
} 