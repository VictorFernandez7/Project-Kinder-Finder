using UnityEngine;
using TMPro;

public class Scr_CraftStation : MonoBehaviour
{
    [Header("Interface Properties")]
    [SerializeField] private float delay;

    [Header("Interaction Properties")]
    [SerializeField] private KeyCode interactKey;
    [SerializeField] private string interactMessage;

    [Header("References")]
    [SerializeField] private Animator craftCanvasAnim;
    [SerializeField] private GameObject interactPanel;
    [SerializeField] private GameObject cameraSpot;
    [SerializeField] private Scr_MainCamera mainCamera;
    [SerializeField] public ParticleSystem interactionParticles;
    [SerializeField] private Scr_InterfaceManager interfaceManager;
    [SerializeField] private Scr_AstronautMovement astronautMovement;

    private bool onRange;
    private bool interacting;
    private GameObject astronaut;
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

            if (interacting)
                astronaut.GetComponent<Scr_AstronautMovement>().Stop(true, true);

            else
                astronaut.GetComponent<Scr_AstronautMovement>().MoveAgain();
        }

        astronautMovement.canMove = !interacting;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Astronaut"))
        {
            onRange = true;
            astronaut = collision.gameObject;
        }
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
        mainCamera.interacting = start;
        interactionParticles.Stop();

        if (start)
            mainCamera.craftCenter = cameraSpot;

        if (interacting)
            Invoke("ShowInterface", delay);

        else
            ShowInterface();

        interfaceManager.interacting = interacting;
        interfaceManager.ClearInterface(interacting);
    }

    private void ShowInterface()
    {
        if (!craftCanvasAnim.gameObject.activeInHierarchy)
            craftCanvasAnim.gameObject.SetActive(true);

        craftCanvasAnim.SetBool("Show", interacting);
    }
}