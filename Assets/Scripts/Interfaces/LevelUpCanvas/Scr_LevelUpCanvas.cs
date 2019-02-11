using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Scr_LevelUpCanvas : MonoBehaviour
{
    [Header("Panel Settings")]
    [SerializeField] private bool closeTime;
    [SerializeField] private float timeToShow;

    [Header("Panel References")]
    [SerializeField] private Slider expSlider;
    [SerializeField] private GameObject content;
    [SerializeField] private TextMeshProUGUI experienceText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI titleText;

    [Header("Unlock References")]
    [SerializeField] private GameObject unlock1;
    [SerializeField] private TextMeshProUGUI unlock1Text;
    [SerializeField] private Image unlock1Image;
    [SerializeField] private GameObject unlock2;
    [SerializeField] private TextMeshProUGUI unlock2Text;
    [SerializeField] private Image unlock2Image;

    private float savedTimeToShow;

    private void Start()
    {
        savedTimeToShow = timeToShow;
    }

    private void Update()
    {
        if (content.activeInHierarchy && closeTime)
        {
            savedTimeToShow -= Time.deltaTime;

            if (savedTimeToShow <= 0)
            {
                savedTimeToShow = timeToShow;
                content.SetActive(false);
            }
        }
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
            unlock2.SetActive(false);

            unlock1Text.text = unlock1Name;
            unlock1Image = unlock1Icon;

            unlock2Text.text = unlock2Name;
            unlock2Image = unlock2Icon;
        }

        expSlider.maxValue = targetExperience;
        expSlider.value = currentExperience;

        content.SetActive(true);
    }

    public void ClosePanel()
    {
        content.SetActive(false);
    }
}