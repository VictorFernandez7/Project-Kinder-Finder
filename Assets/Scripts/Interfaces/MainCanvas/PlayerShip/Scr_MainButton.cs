using UnityEngine.EventSystems;
using UnityEngine;

public class Scr_MainButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Select Button")]
    [SerializeField] private DesiredButton desiredButton;

    [Header("References")]
    [SerializeField] private Animator filesAnim;

    private bool filesActive;
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
        if (desiredButton == DesiredButton.Files)
        {
            if (!filesActive)
                anim.SetBool("ShowText", true);
        }

        else
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
                filesActive = false;
                break;
            case DesiredButton.Files:
                filesAnim.SetBool("ShowButtons", true);
                filesActive = true;
                break;
        }
    }
}