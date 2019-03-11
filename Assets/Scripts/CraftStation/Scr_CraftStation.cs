using UnityEngine;
using TMPro;

public class Scr_CraftStation : MonoBehaviour
{
    [Header("Interaction Properties")]
    [SerializeField] private KeyCode interactKey;
    [SerializeField] private string interactMessage;

    [Header("References")]
    [SerializeField] private GameObject interactPanel;
    [SerializeField] private GameObject visuals;
    [SerializeField] private Scr_MainCamera mainCamera;

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
        interactPanel.SetActive(onRange);
    }

    private void Interact(bool start)
    {
        mainCamera.interacting = start;

        if (start)
            mainCamera.craftCenter = visuals;
    }
}