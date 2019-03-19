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
    [SerializeField] private Scr_CraftInterface craftInterface;

    [HideInInspector] public bool[] unlockedItems;

    private int craftIndex;
    private bool mouseOver;
    private float minDistance;
    private string selectedTool;
    private string savedSelectedTool;
    private Animator anim;
    private Scr_CraftInterface.TypeOfCraft category;

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

        if (mouseOver && Vector2.Distance(wheel.transform.position, mainCamera.ScreenToWorldPoint(Input.mousePosition)) > 0.1f)
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
                    craftIndex = i;

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

        switch (this.name)
        {
            case "Ship":
                category = Scr_CraftInterface.TypeOfCraft.Ship;
                break;
            case "Tools":
                category = Scr_CraftInterface.TypeOfCraft.Tool;
                break;
            case "Suit":
                category = Scr_CraftInterface.TypeOfCraft.Suit;
                break;
        }

        craftInterface.UpdateInfo(category, craftIndex);
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