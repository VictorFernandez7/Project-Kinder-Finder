using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_BreakeableDetection : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject inputText;
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
            astronautsActions.spotType = Scr_AstronautsActions.SpotType.breakeable;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Astronaut"))
        {
            insideTrigger = false; ;

            astronautsActions.miningSpot = null;
        }
    }

    private void CanvasItemActivation()
    {
        if (insideTrigger)
        {
            if (astronautsActions.iAMovement.isMining)
                inputText.SetActive(false);

            else if (astronautsActions.solidTool.activeInHierarchy)
            {
                inputText.SetActive(true);
            }
        }

        else
            inputText.SetActive(false);
    }
}
