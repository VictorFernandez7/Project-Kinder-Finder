using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_PickUpMiningTool : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Scr_NarrativeManager narrativeManager;
    [SerializeField] private GameObject canvas;
    [SerializeField] private Scr_AstronautsActions astronautActions;

    [Header("Narrative Parameters")]
    [SerializeField] private int dialogIndex;

    private bool onRange;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && onRange)
        {
            astronautActions.unlockedTools[0] = true;
            narrativeManager.StartDialogue(dialogIndex);
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Astronaut"))
        {
            canvas.SetActive(true);
            onRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Astronaut"))
        {
            canvas.SetActive(false);
            onRange = false;
        }
    }
}
