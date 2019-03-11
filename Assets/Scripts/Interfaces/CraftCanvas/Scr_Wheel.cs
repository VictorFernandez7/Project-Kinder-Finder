using UnityEngine.EventSystems;
using UnityEngine;

public class Scr_Wheel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Selection Icons")]
    [SerializeField] public GameObject[] selectionIcons;

    [Header("Selection Sprites")]
    [SerializeField] public GameObject[] selectionSprites;

    [Header("References")]
    [SerializeField] private GameObject wheel;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();

        wheel.transform.localScale = Vector3.zero;
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        anim.SetBool("Show", true);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        anim.SetBool("Show", false);
    }
}