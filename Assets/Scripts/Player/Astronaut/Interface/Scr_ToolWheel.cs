using UnityEngine;
using TMPro;

public class Scr_ToolWheel : MonoBehaviour
{
    [Header("Tools")]
    [SerializeField] public GameObject[] tools;

    [Header("Texts")]
    [SerializeField] public TextMeshProUGUI toolName;

    [Header("Animators")]
    [SerializeField] public Animator wheelAnim;
    [SerializeField] public Animator toolsAnim;
}