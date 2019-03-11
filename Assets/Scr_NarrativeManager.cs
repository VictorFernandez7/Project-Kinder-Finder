using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Scr_NarrativeManager : MonoBehaviour
{
    [Header("Dialogs")]
    [SerializeField] private float speedText;
    [TextArea] [SerializeField] private string[] dialogTexts;

    [Header("References")]
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI texts;

    [HideInInspector] public bool onDialog;

    private int step = 0;
    private int characterIndex = 0;

    private void Start()
    {
        StartCoroutine(DisplayTimer());
    }

    private void Update()
    {
        if(step == 0)
        {
        }
    }

    IEnumerator DisplayTimer()
    {
        while(1 == 1)
        {
            yield return new WaitForSeconds(speedText);
            if (characterIndex < dialogTexts[step].Length)
                continue;
            texts.text = dialogTexts[step].Substring(1, characterIndex);
            characterIndex++;
        }
    }
}
