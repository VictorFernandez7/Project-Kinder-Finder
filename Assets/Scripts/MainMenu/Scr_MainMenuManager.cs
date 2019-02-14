using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Scr_MainMenuManager : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private float cameraSpeed;

    [Header("Audio References")]
    [SerializeField] private Slider soundFxSlider;
    [SerializeField] private Slider musicSlider;

    [Header("Resolution References")]
    [SerializeField] private TMP_Dropdown resolutionsDropdown;
    [SerializeField] private TextMeshProUGUI graphicsDropdownLabel;

    [Header("References")]
    [SerializeField] public GameObject mainCamera;
    [SerializeField] public Animator mainButtonsAnim;
    [SerializeField] public Animator secondaryButtonsAnim;
    [SerializeField] public Animator settingsAnim;
    [SerializeField] public Animator mainCanvasAnim;

    [HideInInspector] public Vector3 savedMainSpot;
    [HideInInspector] public Vector3 currentCameraPos;
    [HideInInspector] public MainMenuLevel mainMenuLevel;

    private Vector3 initialCameraPos;
    private Resolution[] resolutions;

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

        Graphics();
        Resolution();
    }

    private void Update()
    {
        CheckInput();
        CameraMovement();
    }

    private void Resolution()
    {
        resolutions = Screen.resolutions;

        resolutionsDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                currentResolutionIndex = i;
        }

        resolutionsDropdown.AddOptions(options);
        resolutionsDropdown.value = currentResolutionIndex;
        resolutionsDropdown.RefreshShownValue();
    }

    private void Graphics()
    {
        switch (QualitySettings.GetQualityLevel())
        {
            case 0:
                graphicsDropdownLabel.text = "Very Low";
                break;
            case 1:
                graphicsDropdownLabel.text = "Low";
                break;
            case 2:
                graphicsDropdownLabel.text = "Medium";
                break;
            case 3:
                graphicsDropdownLabel.text = "High";
                break;
            case 4:
                graphicsDropdownLabel.text = "Very High";
                break;
            case 5:
                graphicsDropdownLabel.text = "Ultra";
                break;
        }
    }

    private void CheckInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (mainMenuLevel == MainMenuLevel.Main)
            {
                mainMenuLevel = MainMenuLevel.Initial;
                currentCameraPos = initialCameraPos;

                secondaryButtonsAnim.SetBool("Play", false);
                secondaryButtonsAnim.SetBool("Settings", false);
                secondaryButtonsAnim.SetBool("AboutUs", false);
            }

            else if (mainMenuLevel == MainMenuLevel.Secondary)
            {
                mainMenuLevel = MainMenuLevel.Main;
                currentCameraPos = savedMainSpot;

                settingsAnim.SetBool("Audio", false);
                settingsAnim.SetBool("Video", false);
                settingsAnim.SetBool("Game", false);
            }
        }
    }

    private void CameraMovement()
    {
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, currentCameraPos, Time.deltaTime * cameraSpeed);
    }

    public void OnSoundFxValueChanged()
    {
        //Scr_MusicManager.Instance.SfxVolume = soundFxSlider.value;
    }

    public void OnMusicValueChanged()
    {
        //Scr_MusicManager.Instance.MusicVolume = musicSlider.value;
    }

    public void SaveVolumes()
    {
        //Scr_MusicManager.Instance.MusicVolumeSave = musicSlider.value;
        //Scr_MusicManager.Instance.SfxVolumeSave = soundFxSlider.value;
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}