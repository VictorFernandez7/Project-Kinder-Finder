using UnityEngine.EventSystems;
using UnityEngine;

public class Scr_Wheel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Selection Icons")]
    [SerializeField] public GameObject[] selectionIcons;

    [Header("Selection Sprites")]
    [SerializeField] public GameObject[] selectionSprites;

    [Header("References")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject wheel;

    private bool mouseOver;
    private float minDistance;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();

        anim.SetBool("Show", false);
        minDistance = 100;
    }

    private void Update()
    {
        MouseOverWheel();
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
        minDistance = 100;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
    }

    private void MouseOverWheel()
    {
        anim.SetBool("Show", mouseOver);

        if (Vector2.Distance(wheel.transform.position, mainCamera.ScreenToWorldPoint(Input.mousePosition)) > 0.15f && mouseOver)
            UpdateSelectedTool();

        else
        {
            for (int i = 0; i < selectionSprites.Length; i++)
            {
                selectionSprites[i].SetActive(false);
            }
        }
    }

    private void UpdateSelectedTool()
    {
        for (int i = 0; i < selectionIcons.Length; i++)
        {
            if (Vector2.Distance(selectionIcons[i].transform.position, mainCamera.ScreenToWorldPoint(Input.mousePosition)) < minDistance)
            {
                minDistance = Vector2.Distance(selectionIcons[i].transform.position, mainCamera.ScreenToWorldPoint(Input.mousePosition));

                for (int j = 0; j < selectionSprites.Length; j++)
                {
                    if (j == i)
                        selectionSprites[j].SetActive(true);

                    else
                        selectionSprites[j].SetActive(false);
                }
            }
        }
    }
}