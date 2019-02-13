using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;

public class Scr_MainMenuButton : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject visuals;
    [SerializeField] private Animator canvasAnim;
    [SerializeField] private TextMeshProUGUI buttonText;

    private Scr_ButtonVisuals buttonVisuals;

    private void Start()
    {
        buttonVisuals = GetComponentInChildren<Scr_ButtonVisuals>();

        GetComponent<SphereCollider>().radius = visuals.transform.localScale.x / 100;
        buttonText.color = Color.clear;
    }

    private void OnMouseEnter()
    {
        buttonVisuals.rotate = true;
    }

    private void OnMouseExit()
    {
        buttonVisuals.rotate = false;
    }
}