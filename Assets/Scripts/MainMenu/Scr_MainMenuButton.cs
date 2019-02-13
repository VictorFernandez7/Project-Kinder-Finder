using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;

public class Scr_MainMenuButton : MonoBehaviour
{
    [Header("Select Button")]
    [SerializeField] private MainMenuButton mainMenuButton;

    [Header("Camera Settings")]
    [SerializeField] private float cameraSpeed;

    [Header("Internal References")]
    [SerializeField] private Animator canvasAnim;
    [SerializeField] private Transform cameraSpot;
    [SerializeField] private GameObject visuals;
    [SerializeField] private TextMeshProUGUI buttonText;

    [Header("External References")]
    [SerializeField] private GameObject mainCamera;

    private Vector3 initialCameraPos;
    private Vector3 currentCameraPos;
    private Scr_ButtonVisuals buttonVisuals;

    private enum MainMenuButton
    {
        MainScreenButton,
        ContinueGame,
        NewGame,
        LoadGame,
        AudioSettings,
        VideoSettings,
        GameSettings,
        Team,
        RRSS,
        Exit
    }

    private void Start()
    {
        buttonVisuals = GetComponentInChildren<Scr_ButtonVisuals>();

        buttonText.enabled = false;
        initialCameraPos = mainCamera.transform.position;
        currentCameraPos = initialCameraPos;
        GetComponent<SphereCollider>().radius = visuals.transform.localScale.x / 100;
    }

    private void Update()
    {
        CameraMovement();
    }

    private void OnMouseOver()
    {
        buttonText.enabled = true;
        buttonVisuals.rotate = true;
        canvasAnim.SetBool("ShowText", true);

        if (Input.GetMouseButtonDown(0))
            currentCameraPos = cameraSpot.position;
    }

    private void OnMouseExit()
    {
        buttonVisuals.rotate = false;
        canvasAnim.SetBool("ShowText", false);
    }

    private void CameraMovement()
    {
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, currentCameraPos, Time.deltaTime * cameraSpeed);
    }
}