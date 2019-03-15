using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_NarrativeLauncher : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Scr_NarrativeManager narrativeManager;

    [Header("Narrative Parameters")]
    [SerializeField] private int dialogIndex;

    private bool launched;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Astronaut") && !launched)
        {
            narrativeManager.StartDialogue(dialogIndex);
            launched = true;
        }
    }
}
