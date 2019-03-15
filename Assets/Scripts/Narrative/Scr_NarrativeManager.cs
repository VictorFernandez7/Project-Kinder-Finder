﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Scr_NarrativeManager : MonoBehaviour
{
    [Header("Dialogues")]
    [SerializeField] private float speedText;
    [SerializeField] private Dialogue[] dialogues;

    [Header("References")]
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI texts;
    [SerializeField] private Scr_AstronautMovement astronautMovement;

    [HideInInspector] public bool onDialogue;

    private int step = 0;
    private int characterIndex = 0;
    private Queue<string> sentences;

    private void Start()
    {
        sentences = new Queue<string>();

        if (step == 0)
            StartDialogue(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) && onDialogue)
            DisplayNextSentence();
    }

    public void StartDialogue(int index)
    {
        astronautMovement.Stop();
        panel.SetActive(true);
        onDialogue = true;

        sentences.Clear();

        foreach(string sentence in dialogues[index].dialogTexts)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentences(sentence));
    }

    IEnumerator TypeSentences(string sentence)
    {
        texts.text = "";

        foreach(char letter in sentence.ToCharArray())
        {
            texts.text += letter;
            yield return new WaitForSeconds(speedText);
        }
    }

    private void EndDialogue()
    {
        panel.SetActive(false);
        astronautMovement.canMove = true;
    }
}

[System.Serializable]
public class Dialogue
{
    [TextArea] public string[] dialogTexts;
}