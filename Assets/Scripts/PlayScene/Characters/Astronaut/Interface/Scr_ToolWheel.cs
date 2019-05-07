using UnityEngine;
using TMPro;

public class Scr_ToolWheel : MonoBehaviour
{
    [Header("Tools")]
    [SerializeField] public GameObject[] unlockedTools;
    [SerializeField] public GameObject[] lockedTools;

    [Header("Selection Sprites")]
    [SerializeField] public GameObject[] selectionSprites;

    [Header("Texts")]
    [SerializeField] public TextMeshProUGUI toolName;

    [Header("Animators")]
    [SerializeField] public Animator wheelAnim;
    [SerializeField] public Animator toolsAnim;

    private void Update()
    {
        CheckIfLocked();
    }

    private void CheckIfLocked()
    {
        for (int i = 0; i < Scr_LevelManager.unlockedTools.Length; i++)
        {
            if (Scr_LevelManager.unlockedTools[i] == true)
            {
                unlockedTools[i].SetActive(true);
                lockedTools[i].SetActive(false);
            }

            else
            {
                unlockedTools[i].SetActive(false);
                lockedTools[i].SetActive(true);
            }
        }
    }
}