using UnityEngine;

public class Scr_3DButton : MonoBehaviour
{
    [Header("Select Button Type")]
    [SerializeField] private ButtonType buttonType;

    [Header("Select System Number")]
    [Range(0, 7)] [SerializeField] private int systemNumber;

    [Header("Planet Panel")]
    [SerializeField] private float delay;

    [Header("References (All)")]
    [SerializeField] private Scr_SunButton sunButton;
    [SerializeField] private GameObject indicator;

    [Header("References (Planet)")]
    [SerializeField] private Transform cameraSpot;
    [SerializeField] private Scr_PlanetPanel planetPanel;

    [Header("References (System)")]
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject[] systems;
    [SerializeField] private GameObject[] planets;

    private bool timerOn;
    private float savedDelay;
    private Scr_PlanetPanelInfo planetPanelInfo;

    private enum ButtonType
    {
        System,
        Planet
    }

    private void Start()
    {
        planetPanelInfo = GetComponent<Scr_PlanetPanelInfo>();

        savedDelay = delay;
    }

    private void Update()
    {
        if (buttonType == ButtonType.Planet)
        {
            DelayTimer();
            CheckPanel();
        }

        if (buttonType == ButtonType.System)
            CheckAnimation();
    }

    private void OnMouseOver()
    {
        indicator.SetActive(true);

        if (Input.GetMouseButtonDown(0))
        {
            if (buttonType == ButtonType.System)
            {
                sunButton.interfaceLevel = Scr_SunButton.InterfaceLevel.PlanetSelection;

                for (int i = 0; i < systems.Length; i++)
                {
                    if (i == systemNumber)
                        systems[i].SetActive(true);

                    else
                        systems[i].SetActive(false);
                }
            }

            else if (buttonType == ButtonType.Planet)
            {
                sunButton.interfaceLevel = Scr_SunButton.InterfaceLevel.PlanetInfo;

                planetPanel.UpdatePanelInfo(planetPanelInfo.planetName, planetPanelInfo.highTemp, planetPanelInfo.lowTemp, planetPanelInfo.toxic, planetPanelInfo.jetpack, planetPanelInfo.res1, planetPanelInfo.res2, planetPanelInfo.res3, planetPanelInfo.res4, planetPanelInfo.res5, planetPanelInfo.history);
                sunButton.targetCameraPos = cameraSpot.position;
                timerOn = true;
            }
        }
    }

    private void OnMouseExit()
    {
        indicator.SetActive(false);
    }

    private void CheckPanel()
    {
        if (Input.GetMouseButtonDown(1) && sunButton.planetPanel.activeInHierarchy)
            sunButton.planetPanel.SetActive(false);
    }

    private void DelayTimer()
    {
        if (timerOn)
        {
            savedDelay -= Time.deltaTime;

            if (savedDelay <= 0)
            {
                sunButton.planetPanel.SetActive(true);
                savedDelay = delay;
                timerOn = false;
            }
        }
    }

    private void CheckAnimation()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Anim_SystemToPlanet") || anim.GetCurrentAnimatorStateInfo(0).IsName("Anim_PlanetToSystem"))
        {
            for (int i = 0; i < planets.Length; i++)
            {
                planets[i].GetComponent<Scr_SimpleRotation>().enabled = false;
            }
        }

        else
        {
            for (int i = 0; i < planets.Length; i++)
            {
                planets[i].GetComponent<Scr_SimpleRotation>().enabled = true;
            }
        }
    }
}