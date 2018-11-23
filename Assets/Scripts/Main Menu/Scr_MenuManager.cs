using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Scr_MenuManager : MonoBehaviour
{
    [Header("Button Detection")]
    [SerializeField] private LayerMask buttonMask;
    [SerializeField] private LayerMask backMask;

    [Header("Camera Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform controlsCamSpot;
    [SerializeField] private Transform controlsBackSpot;
    [SerializeField] private Transform aboutUsCamSpot;
    [SerializeField] private Transform aboutUsBackSpot;

    [Header("Text References")]
    [SerializeField] private Animator playText;
    [SerializeField] private Animator controlsText;
    [SerializeField] private Animator controlPanel;
    [SerializeField] private Animator aboutUsText;
    [SerializeField] private Animator exitText;
    [SerializeField] private Animator backText;

    private bool moveToSpot;
    private bool mainMenu;
    private Vector3 currentCameraSpot;
    private Vector3 currentBackSpot;
    private Vector3 initialCameraSpot;
    private Vector3 initialBackSpot;
    private GameObject mainCamera;
    private GameObject backPlanet;
    private GameObject targetPlanet;

    private void Start()
    {
        mainCamera = GameObject.Find("MainCamera");
        backPlanet = GameObject.Find("BackPlanet");

        initialCameraSpot = mainCamera.transform.position;
        initialBackSpot = backPlanet.transform.position;

        mainMenu = true;
    }

    private void Update()
    {
        CheckSelection();
        CheckInput();

        if (moveToSpot)
            MoveToSpot();
    }

    private void CheckSelection()
    {
        RaycastHit buttonHit;
        Ray ratCastToMousePos = mainCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ratCastToMousePos, out buttonHit, buttonMask))
        {
            Debug.DrawLine(mainCamera.transform.position, buttonHit.point, Color.yellow);

            targetPlanet = buttonHit.transform.gameObject;

            if (mainMenu)
            {
                if (!targetPlanet.gameObject.CompareTag("BackButton"))
                {
                    targetPlanet.GetComponent<Animator>().SetBool("Rotate", true);
                    targetPlanet.GetComponent<Animator>().SetFloat("Speed", 1);
                }

                if (targetPlanet.gameObject.CompareTag("PlayButton"))
                    playText.SetBool("ShowText", true);

                else if (targetPlanet.gameObject.CompareTag("ControlsButton"))
                    controlsText.SetBool("ShowText", true);

                else if (targetPlanet.gameObject.CompareTag("AboutUsButton"))
                    aboutUsText.SetBool("ShowText", true);

                else if (targetPlanet.gameObject.CompareTag("ExitButton"))
                    exitText.SetBool("ShowText", true);
            }

            else
            {
                if (targetPlanet.gameObject.CompareTag("BackButton"))
                {
                    targetPlanet.GetComponent<Animator>().SetBool("Rotate", true);
                    targetPlanet.GetComponent<Animator>().SetFloat("Speed", 1);
                }

                if (targetPlanet.gameObject.CompareTag("BackButton"))
                    backText.SetBool("ShowText", true);
            }
        }

        else if (targetPlanet != null)
        {
            targetPlanet.GetComponent<Animator>().SetFloat("Speed", 0);
            playText.SetBool("ShowText", false);
            controlsText.SetBool("ShowText", false);
            aboutUsText.SetBool("ShowText", false);
            exitText.SetBool("ShowText", false);
            backText.SetBool("ShowText", false);
        }
    }

    private void CheckInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (mainMenu)
            {
                if (targetPlanet.gameObject.CompareTag("PlayButton"))
                    PlayGame();

                if (targetPlanet.gameObject.CompareTag("ControlsButton"))
                    ControlsPlanet();

                if (targetPlanet.gameObject.CompareTag("AboutUsButton"))
                    AboutUsPlanet();

                if (targetPlanet.gameObject.CompareTag("ExitButton"))
                    ExitGame();
            }

            else
            {
                if (targetPlanet.gameObject.CompareTag("BackButton"))
                    Back();
            }
        }
    }

    private void MoveToSpot()
    {
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, currentCameraSpot, Time.deltaTime * moveSpeed);

        if (currentBackSpot == initialBackSpot)
            backPlanet.transform.position = Vector3.Lerp(backPlanet.transform.position, currentBackSpot, Time.deltaTime * moveSpeed * 3);

        else
            backPlanet.transform.position = Vector3.Lerp(backPlanet.transform.position, currentBackSpot, Time.deltaTime * moveSpeed);
    }

    private void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void ControlsPlanet()
    {
        moveToSpot = true;
        mainMenu = false;
        controlsText.SetBool("ShowText", false);
        controlPanel.SetBool("ShowText", true);
        currentCameraSpot = controlsCamSpot.position;
        currentBackSpot = controlsBackSpot.position;
    }

    private void AboutUsPlanet()
    {
        moveToSpot = true;
        mainMenu = false;
        aboutUsText.SetBool("ShowText", false);
        currentCameraSpot = aboutUsCamSpot.position;
        currentBackSpot = aboutUsBackSpot.position;
    }

    private void Back()
    {
        moveToSpot = true;
        mainMenu = true;
        backText.SetBool("ShowText", false);
        controlPanel.SetBool("ShowText", false);
        currentCameraSpot = initialCameraSpot;
        currentBackSpot = initialBackSpot;
    }

    private void ExitGame()
    {
        Application.Quit();
    }
}