using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Scr_MenuManager : MonoBehaviour
{
    [Header("Intro Screen Settings")]
    [SerializeField] private float timeToInteract;
    [SerializeField] private string continueText;
    [SerializeField] private float timeToGoBack;
    [SerializeField] private KeyCode keyToGoBack;

    [Header("Intro Screen References")]
    [SerializeField] private Animator logoAnim;
    [SerializeField] private Animator continueTextAnim;

    [Header("Main Menu Settings")]
    [SerializeField] private float cameraMoveSpeed;
    [SerializeField] private KeyCode keyToBack;
    [SerializeField] private LayerMask buttonMask;
    [SerializeField] private LayerMask backMask;

    [Header("Main Menu References")]
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private Transform controlsCamSpot;
    [SerializeField] private Transform controlsBackSpot;
    [SerializeField] private Transform aboutUsCamSpot;
    [SerializeField] private Transform aboutUsBackSpot;
    [SerializeField] private Transform playCamSpot;
    [SerializeField] private Animator buttonsAnim;
    [SerializeField] private Animator lightAnim;
    [SerializeField] private Animator playTextAnim;
    [SerializeField] private Animator controlsTextAnim;
    [SerializeField] private Animator controlPanelAnim;
    [SerializeField] private Animator aboutUsTextAnim;
    [SerializeField] private Animator aboutUsPanelAnim;
    [SerializeField] private Animator exitTextAnim;
    [SerializeField] private Animator exitCrashAnim;
    [SerializeField] private Animator backTextAnim;
    [SerializeField] private Animator fadeImageAnim;
    [SerializeField] private GameObject playPlanet;
    [SerializeField] private GameObject backPlanet;

    private bool firstMenuScreen;
    private bool checkIntroScreen;
    private float initialTimeToInteract;
    private float initialtimeToGoBack;
    private float checkMouseMovement = 1f;
    private Vector3 currentMousePos;
    private Vector3 currentCameraSpot;
    private Vector3 currentBackSpot;
    private Vector3 initialCameraSpot;
    private Vector3 initialBackSpot;
    private Vector3 initialPlayPlanetPos;
    private Vector3 initialControlsPlanetPos;
    private Vector3 initialAboutUsPlanetPos;
    private Vector3 initialBackPlanetPos;
    private GameObject targetPlanet;

    [DllImport("user32.dll")]
    static extern bool SetCursorPos(int X, int Y);

    private void Start()
    {
        fadeImageAnim.SetBool("Fade", true);
        continueTextAnim.SetBool("Show", true);
        logoAnim.SetBool("Show", true);
        continueTextAnim.SetFloat("TimeToInteract", timeToInteract);

        initialCameraSpot = mainCamera.transform.position;
        initialBackSpot = backPlanet.transform.position;
        initialTimeToInteract = timeToInteract;
        initialtimeToGoBack = timeToGoBack;

        currentCameraSpot = initialCameraSpot;
        currentBackSpot = initialBackSpot;

        checkIntroScreen = true;
    }

    private void Update()
    {
        MoveToSpot();

        if (checkIntroScreen)
            IntroScreen();

        else
        {
            CheckReturnToIntro();
            CheckSelection();
            CheckInput();
        }
    }

    private void IntroScreen()
    {
        buttonsAnim.SetBool("Show", false);
        lightAnim.SetBool("MainMenu", false);

        timeToInteract -= Time.deltaTime;
        continueTextAnim.SetFloat("TimeToInteract", timeToInteract);

        if (timeToInteract <= 0)
        {
            if (Input.anyKeyDown)
            {
                firstMenuScreen = true;
                timeToInteract = initialTimeToInteract;
                continueTextAnim.SetBool("Show", false);
                logoAnim.SetBool("Show", false);
                SetCursorPos(Screen.width / 2, Screen.height / 4);
                checkIntroScreen = false;
            }
        }
    }

    private void CheckReturnToIntro()
    {
        timeToGoBack -= Time.deltaTime;
        checkMouseMovement -= Time.deltaTime;

        if (timeToGoBack <= 0 || (Input.GetKeyDown(keyToGoBack) && firstMenuScreen))
        {
            firstMenuScreen = false;
            timeToGoBack = initialtimeToGoBack;
            continueTextAnim.SetBool("Show", true);
            logoAnim.SetBool("Show", true);
            currentCameraSpot = initialCameraSpot;
            checkIntroScreen = true;
        }

        if (checkMouseMovement <= 0)
        {
            currentMousePos = Input.mousePosition;
            checkMouseMovement = 1f;
        }

        if (Input.anyKeyDown || Input.mousePosition != currentMousePos)
            timeToGoBack = initialtimeToGoBack;
    }

    private void CheckSelection()
    {
        buttonsAnim.SetBool("Show", true);
        lightAnim.SetBool("MainMenu", true);

        RaycastHit buttonHit;
        Ray ratCastToMousePos = mainCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ratCastToMousePos, out buttonHit, buttonMask))
        {
            Debug.DrawLine(mainCamera.transform.position, buttonHit.point, Color.yellow);

            if (firstMenuScreen)
            {
                if (targetPlanet != null)
                {
                    targetPlanet.GetComponent<Animator>().SetBool("Rotate", true);
                    targetPlanet.GetComponent<Animator>().SetFloat("Speed", 1);

                    if (targetPlanet.gameObject.CompareTag("PlayButton"))
                        playTextAnim.SetBool("ShowText", true);

                    else if (targetPlanet.gameObject.CompareTag("ControlsButton"))
                        controlsTextAnim.SetBool("ShowText", true);

                    else if (targetPlanet.gameObject.CompareTag("AboutUsButton"))
                        aboutUsTextAnim.SetBool("ShowText", true);

                    else if (targetPlanet.gameObject.CompareTag("ExitButton"))
                        exitTextAnim.SetBool("ShowText", true);
                }

                if (buttonHit.transform.gameObject.CompareTag("MenuBackground"))
                {
                    if (targetPlanet != null)
                    {
                        targetPlanet.GetComponent<Animator>().SetFloat("Speed", 0);
                        targetPlanet = null;
                    }

                    playTextAnim.SetBool("ShowText", false);
                    controlsTextAnim.SetBool("ShowText", false);
                    aboutUsTextAnim.SetBool("ShowText", false);
                    exitTextAnim.SetBool("ShowText", false);
                }

                else
                    targetPlanet = buttonHit.transform.gameObject;
            }

            else
            {
                if (buttonHit.transform.gameObject.CompareTag("MenuBackground"))
                {
                    if (targetPlanet != null)
                    {
                        targetPlanet.GetComponent<Animator>().SetFloat("Speed", 0);
                        targetPlanet = null;
                    }

                    backTextAnim.SetBool("ShowText", false);
                }

                else
                    targetPlanet = buttonHit.transform.gameObject;

                if (targetPlanet != null)
                {
                    if (targetPlanet.gameObject.CompareTag("BackButton"))
                    {
                        targetPlanet.GetComponent<Animator>().SetBool("Rotate", true);
                        targetPlanet.GetComponent<Animator>().SetFloat("Speed", 1);
                        backTextAnim.SetBool("ShowText", true);
                    }
                }
            }
        }
    }

    private void CheckInput()
    {
        if (Input.GetMouseButtonDown(0) && targetPlanet != null)
        {
            if (firstMenuScreen)
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

        if (Input.GetKeyDown(keyToBack) && !firstMenuScreen)
            Back();
    }

    private void MoveToSpot()
    {
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, currentCameraSpot, Time.deltaTime * cameraMoveSpeed);

        if (currentBackSpot == initialBackSpot)
            backPlanet.transform.position = Vector3.Lerp(backPlanet.transform.position, currentBackSpot, Time.deltaTime * cameraMoveSpeed * 3);

        else
            backPlanet.transform.position = Vector3.Lerp(backPlanet.transform.position, currentBackSpot, Time.deltaTime * cameraMoveSpeed);
    }

    private void PlayGame()
    {
        firstMenuScreen = false;
        playTextAnim.SetBool("ShowText", false);
        currentCameraSpot = playCamSpot.position;

        Invoke("FadeOut", 2f);
        Invoke("ChangeScene", 3f);
    }

    private void ChangeScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void FadeOut()
    {
        fadeImageAnim.SetBool("Fade", false);
    }

    private void ControlsPlanet()
    {
        firstMenuScreen = false;
        controlsTextAnim.SetBool("ShowText", false);
        controlPanelAnim.SetBool("ShowText", true);
        currentCameraSpot = controlsCamSpot.position;
        currentBackSpot = controlsBackSpot.position;
    }

    private void AboutUsPlanet()
    {
        firstMenuScreen = false;
        aboutUsTextAnim.SetBool("ShowText", false);
        aboutUsPanelAnim.SetBool("ShowText", true);
        currentCameraSpot = aboutUsCamSpot.position;
        currentBackSpot = aboutUsBackSpot.position;
    }

    private void Back()
    {
        firstMenuScreen = true;
        backTextAnim.SetBool("ShowText", false);
        controlPanelAnim.SetBool("ShowText", false);
        aboutUsPanelAnim.SetBool("ShowText", false);
        currentCameraSpot = initialCameraSpot;
        currentBackSpot = initialBackSpot;
    }

    private void ExitGame()
    {
        firstMenuScreen = false;
        exitTextAnim.SetBool("ShowText", false);
        exitCrashAnim.SetTrigger("Crash");

        Invoke("Quit", 2.5f);
    }

    private void Quit()
    {
        Application.Quit();
    }
}