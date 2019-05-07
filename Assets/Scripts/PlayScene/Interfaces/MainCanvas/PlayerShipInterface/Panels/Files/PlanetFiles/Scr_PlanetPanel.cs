using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Scr_PlanetPanel : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI planetText;
    [SerializeField] private GameObject highTemperatureImage;
    [SerializeField] private GameObject lowTemperatureImage;
    [SerializeField] private GameObject toxicityImage;
    [SerializeField] private GameObject jetpackImage;
    [SerializeField] private Image resource1Image;
    [SerializeField] private Image resource2Image;
    [SerializeField] private Image resource3Image;
    [SerializeField] private Image resource4Image;
    [SerializeField] private Image resource5Image;
    [SerializeField] private TextMeshProUGUI historyText;
    [SerializeField] private GameObject blocks;
    [SerializeField] private GameObject noneText;

    private GameObject resource1;
    private GameObject resource2;
    private GameObject resource3;
    private GameObject resource4;
    private GameObject resource5;

    private void Start()
    {
        resource1 = resource1Image.gameObject;
        resource2 = resource2Image.gameObject;
        resource3 = resource3Image.gameObject;
        resource4 = resource4Image.gameObject;
        resource5 = resource5Image.gameObject;
    }

    public void UpdatePanelInfo(string planetName, bool highTemp, bool lowTemp, bool toxic, bool jetpack, Sprite res1, Sprite res2, Sprite res3, Sprite res4, Sprite res5, string history)
    {
        planetText.text = planetName;

        highTemperatureImage.SetActive(highTemp);
        lowTemperatureImage.SetActive(lowTemp);
        toxicityImage.SetActive(toxic);
        jetpackImage.SetActive(jetpack);

        if (!highTemp && !lowTemp && !toxic && !jetpack)
        {
            blocks.SetActive(false);
            noneText.SetActive(true);
        }

        else
        {
            blocks.SetActive(true);
            noneText.SetActive(false);
        }

        if (res1 != null)
        {
            resource1.SetActive(true);
            resource1Image.sprite = res1;
        }

        else
            resource1.SetActive(false);

        if (res2 != null)
        {
            resource2.SetActive(true);
            resource2Image.sprite = res2;
        }

        else
            resource2.SetActive(false);

        if (res3 != null)
        {
            resource3.SetActive(true);
            resource3Image.sprite = res3;
        }

        else
            resource3.SetActive(false);

        if (res4 != null)
        {
            resource4.SetActive(true);
            resource4Image.sprite = res4;
        }

        else
            resource4.SetActive(false);

        if (res5 != null)
        {
            resource5.SetActive(true);
            resource5Image.sprite = res5;
        }

        else
            resource5.SetActive(false);

        historyText.text = history;
    }
}