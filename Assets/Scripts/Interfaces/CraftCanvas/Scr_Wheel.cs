using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class Scr_Wheel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Selection Icons")]
    [SerializeField] public GameObject[] selectionIcons;

    [Header("Selection Sprites")]
    [SerializeField] public GameObject[] selectionSprites;

    [Header("Color References")]
    [SerializeField] private Color lockedColor;
    [SerializeField] private Color unlockedColor;

    [Header("References")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject wheel;
    [SerializeField] private Animator infoPanel;

    [HideInInspector] public bool[] unlockedItems;

    private bool mouseOver;
    private float minDistance;
    private string selectedTool;
    private string savedSelectedTool;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();

        unlockedItems = new bool[selectionIcons.Length];
        anim.SetBool("Show", false);

        ResetDistance();

        infoPanel.SetBool("Show", false);
    }

    private void Update()
    {
        MouseOverWheel();
        UpdateSpriteColor();
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

            if (Input.GetMouseButtonDown(0) && selectedTool != null && savedSelectedTool != selectedTool)
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
                if (unlockedItems[i] == true)
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

                else
                {
                    for (int k = 0; k < selectionSprites.Length; k++)
                    {
                        selectionSprites[k].SetActive(false);
                    }
                }
            }
        }

        ResetDistance();
    }

    private void ClickEvent()
    {
        savedSelectedTool = selectedTool;

        if (infoPanel.GetBool("Show"))
            infoPanel.SetTrigger("Reload");

        else
            infoPanel.SetBool("Show", true);
    }

    private void ResetDistance()
    {
        minDistance = 1000000;
    }

    private void UpdateSpriteColor()
    {
        for (int i = 0; i < selectionIcons.Length; i++)
        {
            if (unlockedItems[i] == true)
                selectionIcons[i].GetComponent<Image>().color = unlockedColor;

            else
                selectionIcons[i].GetComponent<Image>().color = lockedColor;
        }
    }
}