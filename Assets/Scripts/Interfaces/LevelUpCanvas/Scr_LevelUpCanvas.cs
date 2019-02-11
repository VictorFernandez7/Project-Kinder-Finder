using UnityEngine;
using TMPro;

public class Scr_LevelUpCanvas : MonoBehaviour
{
    [Header("Panel Settings")]
    [SerializeField] private float persistentTime;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI experienceText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI titleText;

    public void UpdatePanelInfo(int currentExperience, int targetExperience, string newLevel, string newTitle)
    {
        experienceText.text = currentExperience + " / " + targetExperience;
        levelText.text = newLevel;
        titleText.text = newTitle;
    }
}