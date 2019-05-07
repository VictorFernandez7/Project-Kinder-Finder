using UnityEngine.EventSystems;
using UnityEngine;

public class Scr_OpenButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Button Type")]
    [SerializeField] private ButtonType buttonType;

    [Header("References")]
    [SerializeField] private Animator playerShipWindowAnim;
    [SerializeField] private Scr_InterfaceManager interfaceManager;

    private Animator openButtonAnim;

    private enum ButtonType
    {
        Open,
        Close
    }

    private void Start()
    {
        if (buttonType == ButtonType.Open)
            openButtonAnim = GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (buttonType == ButtonType.Open)
            openButtonAnim.SetBool("ShowText", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (buttonType == ButtonType.Open)
            openButtonAnim.SetBool("ShowText", false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (buttonType == ButtonType.Open)
        {
            playerShipWindowAnim.SetBool("Show", true);
            openButtonAnim.SetBool("ShowText", false);
            interfaceManager.PlayerShipWindow();
        }

        else if (buttonType == ButtonType.Close)
        {
            playerShipWindowAnim.SetBool("Show", false);
            interfaceManager.PlayerShipWindow();
        }
    }
}