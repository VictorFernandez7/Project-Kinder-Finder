using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_OreDetection : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Scr_AstronautsActions astronautsActions;
    [SerializeField] public GameObject inputText;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Astronaut") && astronautsActions.solidTool.activeInHierarchy)
            inputText.SetActive(true);

        else
            inputText.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Astronaut"))
        {
            astronautsActions.miningSpot = this.gameObject;
            astronautsActions.spotType = Scr_AstronautsActions.SpotType.solidSpot;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Astronaut"))
            astronautsActions.miningSpot = null;
    }
}