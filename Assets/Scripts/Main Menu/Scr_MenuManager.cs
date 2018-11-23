using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Scr_MenuManager : MonoBehaviour
{
    [Header("Button Detection")]
    [SerializeField] private LayerMask layerMask;

    [Header("Text References")]
    [SerializeField] private Animator playText;
    [SerializeField] private Animator controlsText;
    [SerializeField] private Animator aboutUsText;
    [SerializeField] private Animator exitText;

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
            targetPlanet.GetComponent<Animator>().SetFloat("Speed", 1);

            if (buttonHit.transform.gameObject.CompareTag("PlayButton"))
                playText.SetBool("ShowText", true);

            else if (buttonHit.transform.gameObject.CompareTag("ControlsButton"))
                controlsText.SetBool("ShowText", true);

            else if (buttonHit.transform.gameObject.CompareTag("AboutUsButton"))
                aboutUsText.SetBool("ShowText", true);

            else if (buttonHit.transform.gameObject.CompareTag("ExitButton"))
                exitText.SetBool("ShowText", true);

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
            targetPlanet.GetComponent<Animator>().SetFloat("Speed", 0);
            playText.SetBool("ShowText", false);
            controlsText.SetBool("ShowText", false);
            aboutUsText.SetBool("ShowText", false);
            exitText.SetBool("ShowText", false);
        }
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