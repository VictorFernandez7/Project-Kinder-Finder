using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Scr_LevelUpCanvas : MonoBehaviour
{
    [Header("Panel References")]
    [SerializeField] private Slider expSlider;
    [SerializeField] private TextMeshProUGUI experienceText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Animator anim;

    [Header("Unlock References")]
    [SerializeField] private GameObject unlock1;
    [SerializeField] private TextMeshProUGUI unlock1Text;
    [SerializeField] private Image unlock1Image;
    [SerializeField] private GameObject unlock2;
    [SerializeField] private TextMeshProUGUI unlock2Text;
    [SerializeField] private Image unlock2Image;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        CheckInput();
    }

    public void UpdatePanelInfo(int currentExperience, int targetExperience, string newLevel, string newTitle, bool singleUnlock, string unlock1Name, Image unlock1Icon, string unlock2Name, Image unlock2Icon)
    {
        experienceText.text = currentExperience + " / " + targetExperience;
        levelText.text = newLevel;
        titleText.text = newTitle;

        unlock1.SetActive(singleUnlock);
        unlock2.SetActive(!singleUnlock);

        if (singleUnlock)
        {
            unlock1.SetActive(true);
            unlock2.SetActive(false);

            unlock1Text.text = unlock1Name;
            unlock1Image = unlock1Icon;
        }

        else
        {
            unlock1.SetActive(true);
            unlock2.SetActive(true);

            unlock1Text.text = unlock1Name;
            unlock1Image = unlock1Icon;

            unlock2Text.text = unlock2Name;
            unlock2Image = unlock2Icon;
        }

        expSlider.maxValue = targetExperience;
        expSlider.value = currentExperience;

        anim.SetBool("Show", true);
    }

    private void CheckInput()
    {
        if (Input.GetMouseButtonDown(0))
            anim.SetBool("Show", false);
    }
}