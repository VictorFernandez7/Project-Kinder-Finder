using UnityEngine;
using TMPro;

public class Scr_LiquidDetection : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public GameObject inputText;
    [SerializeField] private GameObject tooltipPanel;
    [SerializeField] private TextMeshProUGUI resourceName;
    [SerializeField] private TextMeshProUGUI resourceAmount;
    [SerializeField] public Scr_AstronautsActions astronautsActions;

    private bool insideTrigger;
    private Scr_LiquidZone liquidOre;

    private void Start()
    {
        liquidOre = GetComponentInParent<Scr_LiquidZone>();

        resourceName.text = liquidOre.liquidType.ToString();
    }

    private void Update()
    {
        resourceAmount.text = ((int)liquidOre.amount).ToString();

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
            astronautsActions.spotType = Scr_AstronautsActions.SpotType.liquidSpot;
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

                if (astronautsActions.liquidTool.activeInHierarchy)
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