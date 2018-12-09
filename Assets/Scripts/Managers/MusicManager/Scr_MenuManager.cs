using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject options;
    public Slider m_musicSlider;
    public Slider m_sfxSlider;
    private bool m_onOptions;
    public void StartGame()
    {
        Blackboard.m_gameType = GameType.NOTBASIC;
        SceneManager.LoadScene(FixedScenes.LVL01_SCENE);
    }
    public void ToggleOptions(bool goToOption)
    {
        mainMenu.SetActive(!goToOption);
        options.SetActive(goToOption);

        PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        if (!goToOption)
        {
            MusicManager.Instance.MusicVolumeSave = m_musicSlider.value;
            MusicManager.Instance.SfxVolumeSave = m_sfxSlider.value;
        }
    }

    public void OnMusicVolumeChange()
    {
        MusicManager.Instance.MusicVolume = m_musicSlider.value;
    }
    public void OnSFXVolumeChange()
    {
        MusicManager.Instance.SfxVolume = m_sfxSlider.value;
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    // Use this for initialization
    void Start () {
        //<MusicManager.Instance.PlayBackgroundMusic(FixedSound.TEMITA);

    }
	
	// Update is called once per frame
	void Update () {

		
	}
}
