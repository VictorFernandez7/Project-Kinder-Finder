using UnityEngine.EventSystems;
using UnityEngine;

public class Scr_OpenButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("References")]
    [SerializeField] private Animator playerShipWindowAnim;
    [SerializeField] private Scr_InterfaceManager interfaceManager;

    private Animator openButtonAnim;

    private void Start()
    {
        openButtonAnim = GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        openButtonAnim.SetBool("ShowText", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        openButtonAnim.SetBool("ShowText", false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        playerShipWindowAnim.SetBool("Show", true);
        interfaceManager.PlayerShipWindow();
    }
}