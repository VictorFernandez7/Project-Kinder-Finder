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
    private string selectedTool;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();

        anim.SetBool("Show", false);
        ResetDistance();
    }

    private void Update()
    {
        MouseOverWheel();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
        ResetDistance();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
    }

    private void MouseOverWheel()
    {
        anim.SetBool("Show", mouseOver);

        if (mouseOver && Vector2.Distance(wheel.transform.position, mainCamera.ScreenToWorldPoint(Input.mousePosition)) > 0.2f)
        {
            UpdateSelectedTool();

            if (Input.GetMouseButtonDown(0))
                ClickEvent();
        }

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
                selectedTool = selectionIcons[i].name;

                for (int j = 0; j < selectionSprites.Length; j++)
                {
                    if (j == i)
                        selectionSprites[j].SetActive(true);

                    else
                        selectionSprites[j].SetActive(false);
                }
            }
        }

        ResetDistance();
    }

    private void ClickEvent()
    {
        print(selectedTool);
    }

    private void ResetDistance()
    {
        minDistance = 1000000;
    }
}