using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_FilesPanelAnimator : MonoBehaviour
{
    [HideInInspector] public bool animationPlaying;

    private void ChangeToTrue()
    {
        animationPlaying = true;
    }

    private void ChangeToFalse()
    {
        animationPlaying = false;
    }
}