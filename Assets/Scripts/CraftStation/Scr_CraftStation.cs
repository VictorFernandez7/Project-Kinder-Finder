using UnityEngine;
using TMPro;

public class Scr_CraftStation : MonoBehaviour
{
    [Header("Interaction Properties")]
    [SerializeField] private KeyCode interactKey;
    [SerializeField] private string interactMessage;

    [Header("References")]
    [SerializeField] private Animator craftCanvasAnim;
    [SerializeField] private GameObject interactPanel;
    [SerializeField] private GameObject visuals;
    [SerializeField] private Scr_MainCamera mainCamera;
    [SerializeField] private Scr_InterfaceManager interfaceManager;

    private bool onRange;
    private bool interacting;
    private TextMeshProUGUI interactText;

    private void Start()
    {
        interactText = interactPanel.GetComponentInChildren<TextMeshProUGUI>();
        interactText.text = interactMessage;
    }

    private void Update()
    {
        OnTriggerEvent();

        if (onRange && Input.GetKeyDown(interactKey))
        {
            interacting = !interacting;
            Interact(interacting);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Astronaut"))
            onRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Astronaut"))
            onRange = false;
    }

    private void OnTriggerEvent()
    {
        interactPanel.SetActive(interacting ? false : onRange);        
    }

    private void Interact(bool start)
    {
        interfaceManager.interacting = start;
        interfaceManager.ClearInterface(start);
        mainCamera.interacting = start;
        craftCanvasAnim.gameObject.SetActive(start);
        craftCanvasAnim.SetBool("Show", start);

        if (start)
            mainCamera.craftCenter = visuals;
    }
}