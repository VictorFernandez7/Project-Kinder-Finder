using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_3DButton : MonoBehaviour
{
    [Header("Select Button Type")]
    [SerializeField] private ButtonType buttonType;

    [Header("Select Button Type")]
    [SerializeField] private Scr_SunButton sunButton;

    private enum ButtonType
    {
        System,
        Planet
    }

    private void OnMouseUp()
    {
        switch (buttonType)
        {
            case ButtonType.System:
                sunButton.interfacelevel = Scr_SunButton.Interfacelevel.PlanetSelection;
                break;
            case ButtonType.Planet:
                sunButton.interfacelevel = Scr_SunButton.Interfacelevel.PlanetInfo;
                break;
        }
    }
}