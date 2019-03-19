using UnityEngine;

public class Scr_PlanetFilesPanel : MonoBehaviour
{
    [Header("Current Interface Level")]
    [SerializeField] public InterfaceLevel interfaceLevel;

    [Header("Camera Settings")]
    [SerializeField] private float cameraSpeed;
    [SerializeField] private float xPositioning;

    [Header("References")]
    [SerializeField] public GameObject planetPanel;
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject dragText;
    [SerializeField] private GameObject planetFilesCamera;

    [HideInInspector] public Vector3 targetCameraPos;

    private Vector3 initialCameraPos;

    public enum InterfaceLevel
    {
        SystemSelection,
        PlanetSelection,
        PlanetInfo
    }

    private void Start()
    {
        initialCameraPos = planetFilesCamera.transform.position;
        targetCameraPos = initialCameraPos;
    }

    private void Update()
    {
        InputCheck();
        CameraPosUpdate();
        InterfaceLevelCheck();
    }

    private void InterfaceLevelCheck()
    {
        switch (interfaceLevel)
        {
            case InterfaceLevel.SystemSelection:
                anim.SetInteger("InterfaceLevel", 0);
                break;
            case InterfaceLevel.PlanetSelection:
                anim.SetInteger("InterfaceLevel", 1);
                targetCameraPos = initialCameraPos;
                dragText.SetActive(true);
                break;
            case InterfaceLevel.PlanetInfo:
                dragText.SetActive(false);
                break;
        }
    }

    private void InputCheck()
    {
        if (Input.GetMouseButtonDown(1))
        {
            switch (interfaceLevel)
            {
                case InterfaceLevel.PlanetSelection:
                    interfaceLevel = InterfaceLevel.SystemSelection;
                    break;
                case InterfaceLevel.PlanetInfo:
                    interfaceLevel = InterfaceLevel.PlanetSelection;
                    break;
            }
        }
    }

    private void CameraPosUpdate()
    {
        if (interfaceLevel == InterfaceLevel.PlanetInfo)
        {
            Vector3 planetCameraPos = new Vector3(targetCameraPos.x + xPositioning, targetCameraPos.y, targetCameraPos.z);
            planetFilesCamera.transform.position = Vector3.Lerp(planetFilesCamera.transform.position, planetCameraPos, Time.deltaTime * cameraSpeed);
        }

        else
            planetFilesCamera.transform.position = Vector3.Lerp(planetFilesCamera.transform.position, targetCameraPos, Time.deltaTime * cameraSpeed);
    }
}