using UnityEngine;
using TMPro;

public class Scr_SystemSelectionManager : MonoBehaviour
{
    [Header("Input Settings")]
    [SerializeField] private KeyCode backCode;

    [Header("References")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private TextMeshProUGUI travelFuelText;
    [SerializeField] private Animator travelHelp;

    [HideInInspector] public float currentZoom;
    [HideInInspector] public float savedZoom;
    [HideInInspector] public float currentZoomSpeed;
    [HideInInspector] public float currentMovementSpeed;
    [HideInInspector] public float travelFuelAmount;
    [HideInInspector] public Vector3 currentPos;
    [HideInInspector] public Vector3 savedPos;
    [HideInInspector] public InterfaceLevel interfaceLevel;

    private float initialZoom;
    private Vector3 initialPos;

    public enum InterfaceLevel
    {
        Initial,
        Galaxy,
        System
    }

    private void Start()
    {
        initialPos = mainCamera.transform.position;
        initialZoom = mainCamera.orthographicSize;
        currentZoom = initialZoom;
        savedZoom = initialZoom;

        currentPos = initialPos;
    }

    private void Update()
    {
        CheckInput();
        UpdateCamera();
        UpdateTravelFuelAmountText();

        if (interfaceLevel == InterfaceLevel.System)
            travelHelp.SetBool("Show", true);

        else
            travelHelp.SetBool("Show", false);
    }

    private void UpdateCamera()
    {
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, currentZoom, Time.deltaTime * currentZoomSpeed);
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, currentPos, Time.deltaTime * currentMovementSpeed);
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(backCode))
        {
            if (interfaceLevel == InterfaceLevel.System)
            {
                interfaceLevel = InterfaceLevel.Galaxy;
                currentZoom = savedZoom;
                currentPos = savedPos;
            }

            else if (interfaceLevel == InterfaceLevel.Galaxy)
            {
                interfaceLevel = InterfaceLevel.Initial;
                currentZoom = initialZoom;
                currentPos = initialPos;
            }
        }
    }

    private void UpdateTravelFuelAmountText()
    {
        travelFuelText.text = "x " + travelFuelAmount;
    }
}