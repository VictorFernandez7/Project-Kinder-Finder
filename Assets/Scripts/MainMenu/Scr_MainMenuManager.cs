using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_MainMenuManager : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private float cameraSpeed;

    [Header("References")]
    [SerializeField] public GameObject mainCamera;

    [Header("Buttons References")]
    [SerializeField] public GameObject playButtons;
    [SerializeField] public GameObject settingsButtons;
    [SerializeField] public GameObject aboutUsButtons;

    [HideInInspector] public Vector3 savedMainSpot;
    [HideInInspector] public Vector3 currentCameraPos;
    [HideInInspector] public MainMenuLevel mainMenuLevel;

    private Vector3 initialCameraPos;

    public enum MainMenuLevel
    {
        Initial,
        Main,
        Secondary
    }

    private void Start()
    {
        initialCameraPos = mainCamera.transform.position;
        currentCameraPos = initialCameraPos;
    }

    private void Update()
    {
        CheckInput();
        CameraMovement();
    }

    private void CheckInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (mainMenuLevel == MainMenuLevel.Main)
            {
                mainMenuLevel = MainMenuLevel.Initial;
                currentCameraPos = initialCameraPos;

                playButtons.SetActive(false);
                settingsButtons.SetActive(false);
                aboutUsButtons.SetActive(false);
            }

            else if (mainMenuLevel == MainMenuLevel.Secondary)
            {
                mainMenuLevel = MainMenuLevel.Main;
                currentCameraPos = savedMainSpot;
            }
        }
    }

    private void CameraMovement()
    {
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, currentCameraPos, Time.deltaTime * cameraSpeed);
    }
}