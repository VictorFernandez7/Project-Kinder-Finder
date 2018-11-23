using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Scr_MenuManager : MonoBehaviour
{
    [Header("Button Detection")]
    [SerializeField] private LayerMask layerMask;

    [Header("Animations")]
    [SerializeField] public Animation playAnimation;
    [SerializeField] public Animation controlsAnimation;
    [SerializeField] public Animation aboutUsAnimation;
    [SerializeField] public Animation extitAnimation;

    private GameObject mainCamera;
    private GameObject targetPlanet;

    private void Start()
    {
        mainCamera = GameObject.Find("MainCamera");
    }

    private void FixedUpdate()
    {
        RaycastHit buttonHit;
        Ray ratCastToMousePos = mainCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ratCastToMousePos, out buttonHit, layerMask))
        {
            Debug.DrawLine(mainCamera.transform.position, buttonHit.point, Color.yellow);

            targetPlanet = buttonHit.transform.gameObject;

            targetPlanet.GetComponent<Animator>().SetBool("Rotate", true);

            if (buttonHit.transform.gameObject.CompareTag("PlayButton"))
                playAnimation["Anim_PlayButtonRotation"].speed = 1;

            else if (buttonHit.transform.gameObject.CompareTag("ControlsButton"))
                controlsAnimation["Anim_ControlButtonRotate"].speed = 1;

            else if (buttonHit.transform.gameObject.CompareTag("AboutUsButton"))
                aboutUsAnimation["Anim_AboutUsButtonRotate"].speed = 1;

            else if (buttonHit.transform.gameObject.CompareTag("ExitButton"))
                extitAnimation["Anim_ExitButtonRotate"].speed = 1;

            if (Input.GetMouseButtonDown(0))
            {
                if (buttonHit.transform.gameObject.CompareTag("PlayButton"))
                    PlayGame();

                else if (buttonHit.transform.gameObject.CompareTag("ControlsButton"))
                    ControlsPlanet();

                else if (buttonHit.transform.gameObject.CompareTag("AboutUsButton"))
                    AboutUsPlanet();

                else if (buttonHit.transform.gameObject.CompareTag("ExitButton"))
                    ExitGame();
            }
        }

        else if (targetPlanet != null)
        {
            playAnimation["Anim_PlayButtonRotation"].speed = 0;
            controlsAnimation["Anim_ControlButtonRotate"].speed = 0;
            aboutUsAnimation["Anim_AboutUsButtonRotate"].speed = 0;
            extitAnimation["Anim_ExitButtonRotate"].speed = 0;
        }

        print(targetPlanet);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void ControlsPlanet()
    {

    }

    private void AboutUsPlanet()
    {

    }

    public void ExitGame()
    {
        Application.Quit();
    }
}