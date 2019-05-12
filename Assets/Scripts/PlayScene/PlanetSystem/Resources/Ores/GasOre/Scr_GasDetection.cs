using UnityEngine;
using TMPro;

public class Scr_GasDetection : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public GameObject inputText;
    [SerializeField] private GameObject tooltipPanel;
    [SerializeField] private TextMeshProUGUI resourceName;
    [SerializeField] private TextMeshProUGUI resourceAmount;
    [SerializeField] public Scr_AstronautsActions astronautsActions;

    private bool insideTrigger;
    private Scr_GasZone gasOre;

    private void Start()
    {
        gasOre = GetComponentInParent<Scr_GasZone>();

        resourceName.text = gasOre.gasType.ToString();
    }

    private void Update()
    {
        resourceAmount.text = ((int)gasOre.amount).ToString();

        CanvasItemActivation();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Astronaut"))
        {
            insideTrigger = true;

            astronautsActions.miningSpot = this.gameObject;
            astronautsActions.spotType = Scr_AstronautsActions.SpotType.gasSpot;
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

                if (astronautsActions.gasTool.activeInHierarchy)
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