using UnityEngine;
using TMPro;

public class Scr_OreDetection : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public GameObject inputText;
    [SerializeField] private GameObject tooltipPanel;
    [SerializeField] private TextMeshProUGUI resourceName;
    [SerializeField] private TextMeshProUGUI resourceAmount;

    private bool insideTrigger;
    private Scr_Ore ore;
    private Scr_AstronautsActions astronautsActions;

    private void Start()
    {
        ore = GetComponentInParent<Scr_Ore>();
        astronautsActions = ore.astronautsActions;

        resourceName.text = ore.oreResourceType.ToString();
    }

    private void Update()
    {
        resourceAmount.text = (ore.initalAmount - ore.rest).ToString();

        if (!astronautsActions.gameObject.activeInHierarchy)
            insideTrigger = false;

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

                else
                    inputText.SetActive(false);
            }
        }

        else
        {
            tooltipPanel.SetActive(false);
            inputText.SetActive(false);
        }
    }
}