using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_3DButton : MonoBehaviour
{
    [Header("Select Button Type")]
    [SerializeField] private ButtonType buttonType;

    [Header("Select System Number")]
    [SerializeField] private SystemNumber systemNumber;

    [Header("References")]
    [SerializeField] private Scr_SunButton sunButton;
    [SerializeField] private GameObject indicator;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject canvas;

    [Header("Planet Systems")]
    [SerializeField] private GameObject system1;
    [SerializeField] private GameObject system2;
    [SerializeField] private GameObject system3;
    [SerializeField] private GameObject system4;
    [SerializeField] private GameObject system5;
    [SerializeField] private GameObject system6;
    [SerializeField] private GameObject system7;
    [SerializeField] private GameObject system8;

    private enum ButtonType
    {
        System,
        Planet
    }

    private enum SystemNumber
    {
        System1,
        System2,
        System3,
        System4,
        System5,
        System6,
        System7,
        System8,
    }

    private void Update()
    {
        if (buttonType == ButtonType.Planet)
            UpdateCanvasPosition();
    }

    private void OnMouseOver()
    {
        indicator.SetActive(true);
    }

    private void OnMouseExit()
    {
        indicator.SetActive(false);
    }

    private void OnMouseUp()
    {
        switch (buttonType)
        {
            case ButtonType.System:
                sunButton.interfacelevel = Scr_SunButton.Interfacelevel.PlanetSelection;
                switch (systemNumber)
                {
                    case SystemNumber.System1:
                        system1.SetActive(true);
                        system2.SetActive(false);
                        system3.SetActive(false);
                        system4.SetActive(false);
                        system5.SetActive(false);
                        system6.SetActive(false);
                        system7.SetActive(false);
                        system8.SetActive(false);
                        break;
                    case SystemNumber.System2:
                        system1.SetActive(false);
                        system2.SetActive(true);
                        system3.SetActive(false);
                        system4.SetActive(false);
                        system5.SetActive(false);
                        system6.SetActive(false);
                        system7.SetActive(false);
                        system8.SetActive(false);
                        break;
                    case SystemNumber.System3:
                        system1.SetActive(false);
                        system2.SetActive(false);
                        system3.SetActive(true);
                        system4.SetActive(false);
                        system5.SetActive(false);
                        system6.SetActive(false);
                        system7.SetActive(false);
                        system8.SetActive(false);
                        break;
                    case SystemNumber.System4:
                        system1.SetActive(false);
                        system2.SetActive(false);
                        system3.SetActive(false);
                        system4.SetActive(true);
                        system5.SetActive(false);
                        system6.SetActive(false);
                        system7.SetActive(false);
                        system8.SetActive(false);
                        break;
                    case SystemNumber.System5:
                        system1.SetActive(false);
                        system2.SetActive(false);
                        system3.SetActive(false);
                        system4.SetActive(false);
                        system5.SetActive(true);
                        system6.SetActive(false);
                        system7.SetActive(false);
                        system8.SetActive(false);
                        break;
                    case SystemNumber.System6:
                        system1.SetActive(false);
                        system2.SetActive(false);
                        system3.SetActive(false);
                        system4.SetActive(false);
                        system5.SetActive(false);
                        system6.SetActive(true);
                        system7.SetActive(false);
                        system8.SetActive(false);
                        break;
                    case SystemNumber.System7:
                        system1.SetActive(false);
                        system2.SetActive(false);
                        system3.SetActive(false);
                        system4.SetActive(false);
                        system5.SetActive(false);
                        system6.SetActive(false);
                        system7.SetActive(true);
                        system8.SetActive(false);
                        break;
                    case SystemNumber.System8:
                        system1.SetActive(false);
                        system2.SetActive(false);
                        system3.SetActive(false);
                        system4.SetActive(false);
                        system5.SetActive(false);
                        system6.SetActive(false);
                        system7.SetActive(false);
                        system8.SetActive(true);
                        break;
                }
                break;
            case ButtonType.Planet:
                sunButton.interfacelevel = Scr_SunButton.Interfacelevel.PlanetInfo;
                break;
        }
    }

    private void UpdateCanvasPosition()
    {
        Vector3 desiredUp = mainCamera.transform.up;

        canvas.transform.rotation = Quaternion.Euler(desiredUp);
    }
}