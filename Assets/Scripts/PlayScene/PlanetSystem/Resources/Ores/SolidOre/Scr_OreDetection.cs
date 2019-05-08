using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_OreDetection : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public GameObject inputText;
    [SerializeField] private GameObject tooltipPanel;
    [SerializeField] private Scr_AstronautsActions astronautsActions;

    private bool insideTrigger;

    private void Update()
    {
        CanvasItemActivation();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Astronaut"))
        {
            insideTrigger = true;

            astronautsActions.miningSpot = this.gameObject;
            astronautsActions.spotType = Scr_AstronautsActions.SpotType.solidSpot;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Astronaut"))
        {
            insideTrigger = false;

            astronautsActions.miningSpot = null;
        }
    }

    private void CanvasItemActivation()
    {
        if (insideTrigger)
        {
            if (astronautsActions.iAMovement.isMining)
            {
                tooltipPanel.SetActive(false);
                inputText.SetActive(false);
            }

            else
            {
                tooltipPanel.SetActive(true);

                if (astronautsActions.solidTool.activeInHierarchy)
                    inputText.SetActive(true);
            }
        }

        else
        {
            tooltipPanel.SetActive(false);
            inputText.SetActive(false);
        }
    }
}