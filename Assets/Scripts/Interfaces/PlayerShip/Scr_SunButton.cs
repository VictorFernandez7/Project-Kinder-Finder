using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class Scr_SunButton : MonoBehaviour
{
    [Header("Current Interface Level")]
    [SerializeField] public Interfacelevel interfacelevel;

    [Header("References")]
    [SerializeField] private GameObject planets;
    [SerializeField] private GameObject systems;
    [SerializeField] private Animator anim;

    public enum Interfacelevel
    {
        SystemSelection,
        PlanetSelection,
        PlanetInfo
    }

    private void Update()
    {
        LevelCheck();

        if (Input.GetMouseButtonDown(1))
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
    }

    private void LevelCheck()
    {
        switch (interfacelevel)
        {
            case Interfacelevel.SystemSelection:
                anim.SetInteger("InterfaceLevel", 0);
                break;
            case Interfacelevel.PlanetSelection:
                anim.SetInteger("InterfaceLevel", 1);
                break;
            case Interfacelevel.PlanetInfo:
                break;
        }
    }
}