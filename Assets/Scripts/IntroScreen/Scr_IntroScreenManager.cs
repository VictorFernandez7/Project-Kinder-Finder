using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Scr_IntroScreenManager : MonoBehaviour
{
    [Header("Intro Screen Settings")]
    [SerializeField] private float timeToShowText;

    [Header("References")]
    [SerializeField] private GameObject continueText;
    [SerializeField] private Animator fadeImage;

    private void Update()
    {
        timeToShowText -= Time.deltaTime;

        if (timeToShowText <= 0)
        {
            continueText.SetActive(true);

            if (Input.GetMouseButtonDown(0))
            {
                fadeImage.SetBool("Fade", true);

                Invoke("LoadMainMenu", 1f);
            }
        }
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}