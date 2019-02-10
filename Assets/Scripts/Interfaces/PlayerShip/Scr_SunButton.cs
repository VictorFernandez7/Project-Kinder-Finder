using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class Scr_SunButton : MonoBehaviour, IPointerClickHandler
{
    [Header("Current Interface Level")]
    [SerializeField] public Interfacelevel interfacelevel;

    [Header("References")]
    [SerializeField] private GameObject planets;
    [SerializeField] private GameObject systems;

    public enum Interfacelevel
    {
        SystemSelection,
        PlanetSelection,
        PlanetInfo
    }

    private void Update()
    {
        LevelCheck();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (interfacelevel)
        {
            case Interfacelevel.PlanetSelection:
                interfacelevel = Interfacelevel.SystemSelection;
                break;
            case Interfacelevel.PlanetInfo:
                interfacelevel = Interfacelevel.PlanetSelection;
                break;
        }
    }

    private void LevelCheck()
    {
        switch (interfacelevel)
        {
            case Interfacelevel.SystemSelection:
                systems.SetActive(true);
                planets.SetActive(false);
                break;
            case Interfacelevel.PlanetSelection:
                systems.SetActive(true);
                planets.SetActive(false);
                break;
            case Interfacelevel.PlanetInfo:
                break;
        }
    }
}