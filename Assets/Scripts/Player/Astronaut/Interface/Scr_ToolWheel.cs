using UnityEngine;
using TMPro;

public class Scr_ToolWheel : MonoBehaviour
{
    [Header("Tools")]
    [SerializeField] public GameObject[] tools;

    [Header("Selection Sprites")]
    [SerializeField] public GameObject[] selectionSprites;

    [Header("Texts")]
    [SerializeField] public TextMeshProUGUI toolName;

    [Header("Animators")]
    [SerializeField] public Animator wheelAnim;
    [SerializeField] public Animator toolsAnim;
}