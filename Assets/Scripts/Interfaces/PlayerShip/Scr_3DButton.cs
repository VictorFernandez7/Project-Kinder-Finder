using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_3DButton : MonoBehaviour
{
    [Header("Select Button Type")]
    [SerializeField] private ButtonType buttonType;

    [Header("Select System Number")]
    [Range(0, 7)] [SerializeField] private int systemNumber;

    [Header("References")]
    [SerializeField] private Scr_SunButton sunButton;
    [SerializeField] private GameObject indicator;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject canvas;

    [Header("Planet Systems")]
    [SerializeField] private GameObject[] systems;

    private enum ButtonType
    {
        System,
        Planet
    }

    private void Update()
    {
        if (buttonType == ButtonType.Planet)
            UpdateCanvasPosition();
    }

    private void OnMouseOver()
    {
        indicator.SetActive(true);

        if (Input.GetMouseButtonDown(0))
        {
            if (buttonType == ButtonType.System)
            {
                sunButton.interfacelevel = Scr_SunButton.Interfacelevel.PlanetSelection;

                for (int i = 0; i < systems.Length; i++)
                {
                    if (i == systemNumber)
                        systems[i].SetActive(true);

                    else
                        systems[i].SetActive(false);
                }
            }

            else if (buttonType == ButtonType.Planet)
                sunButton.interfacelevel = Scr_SunButton.Interfacelevel.PlanetInfo;
        }
    }

    private void OnMouseExit()
    {
        indicator.SetActive(false);
    }

    private void UpdateCanvasPosition()
    {
        Vector3 desiredUp = mainCamera.transform.up;

        canvas.transform.rotation = Quaternion.Euler(desiredUp);
    }
}