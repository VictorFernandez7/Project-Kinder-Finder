using UnityEngine;
using TMPro;

public class Scr_NarrativeSceneManager : MonoBehaviour
{
    [Header("Set Texts")]
    [TextArea] [SerializeField] private string[] texts;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI narrativeText;
    [SerializeField] private Animator fadeImageAnim;

    private int currentText;

    private void Start()
    {
        fadeImageAnim.SetBool("Show", true);

        narrativeText.text = texts[currentText];
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (currentText < texts.Length - 1)
            {
                currentText += 1;
                NextScreen();
            }

            else
            {
                fadeImageAnim.SetBool("Show", false);
                Invoke("ExitScene", 2.5f);
            }
        }
    }

    private void NextScreen()
    {
        narrativeText.text = texts[currentText];
    }

    private void ExitScene()
    {
        Scr_LevelManager.LoadPlanetSystem(Scr_Levels.LevelToLoad.PlanetSystem1);
    }
}