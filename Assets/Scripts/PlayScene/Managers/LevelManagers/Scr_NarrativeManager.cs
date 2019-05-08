using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Scr_NarrativeManager : MonoBehaviour
{
    [Header("Dialogues")]
    [SerializeField] private float speedText;
    [SerializeField] private Dialogue[] dialogues;

    [Header("Image References")]
    [SerializeField] private GameObject jackImage;
    [SerializeField] private GameObject iaImage;

    [Header("References")]
    [SerializeField] private Animator panelAnim;
    [SerializeField] private ParticleSystem astronautGlow;
    [SerializeField] private ParticleSystem IAGlow;
    [SerializeField] private TextMeshProUGUI texts;
    [SerializeField] private TextMeshProUGUI speakerName;
    [SerializeField] private Scr_AstronautMovement astronautMovement;

    [HideInInspector] public bool onDialogue;

    private int step = 0;
    private int speakerIndex = 0;
    private int textIndex = 0;
    private Queue<string> sentences;
    private string sentence;
    private bool isFinished = true;

    private void Start()
    {
        sentences = new Queue<string>();

        if (step == 0)
            StartDialogue(0);
    }

    private void Update()
    {
        DialogueEffects();

        if (Input.GetMouseButtonDown(0) && onDialogue)
            DisplayNextSentence(textIndex);
    }

    public void StartDialogue(int index)
    {
        textIndex = index;
        astronautMovement.Stop(true, true);
        astronautMovement.onDialogue = true;
        panelAnim.gameObject.SetActive(true);
        panelAnim.SetBool("Show", true);
        onDialogue = true;

        sentences.Clear();

        for(int i = 0; i < dialogues[index].speaks.Length; i++)
        {
            sentences.Enqueue(dialogues[index].speaks[i].dialogTexts);
        }

        DisplayNextSentence(textIndex);
    }

    public void DisplayNextSentence(int index)
    {
        if(isFinished)
        {
            if (sentences.Count == 0)
            {
                EndDialogue();
                return;
            }

            switch (dialogues[index].speaks[speakerIndex].speaker)
            {
                case speakerID.Jack:
                    speakerName.text = "Jack";
                    jackImage.SetActive(true);
                    iaImage.SetActive(false);
                    break;

                case speakerID.IA:
                    speakerName.text = "IA";
                    jackImage.SetActive(false);
                    iaImage.SetActive(true);
                    break;
            }

            sentence = sentences.Dequeue();
            speakerIndex += 1;
            isFinished = false;
            StopAllCoroutines();
            StartCoroutine(TypeSentences(sentence));
        }

        else
        {
            texts.text = sentence;
            isFinished = true;
            StopAllCoroutines();
        }

    }

    IEnumerator TypeSentences(string sentence)
    {
        texts.text = "";

        foreach(char letter in sentence.ToCharArray())
        {
            texts.text += letter;
            yield return new WaitForSeconds(speedText);
        }

        isFinished = true;
    }

    private void EndDialogue()
    {
        speakerIndex = 0;
        panelAnim.SetBool("Show", false);
        onDialogue = false;
        astronautMovement.onDialogue = false;
        astronautMovement.MoveAgain();
    }

    private void DialogueEffects()
    {
        if (onDialogue)
        {
            if (speakerName.text == "Jack")
            {
                if (!astronautGlow.isPlaying)
                {
                    astronautGlow.Play();
                    IAGlow.Stop();
                }
            }

            else if (speakerName.text == "IA")
            {
                if (!IAGlow.isPlaying)
                {
                    IAGlow.Play();
                    astronautGlow.Stop();
                }
            }
        }

        else
        {
            IAGlow.Stop();
            astronautGlow.Stop();
        }
    }
}

[System.Serializable]
public class Dialogue
{
    public Speak[] speaks;
}

[System.Serializable]
public class Speak
{
    public speakerID speaker;
    [TextArea] public string dialogTexts;
}

public enum speakerID
{
    Jack,
    IA
}
