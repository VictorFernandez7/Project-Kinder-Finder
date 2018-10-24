using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scr_FuelCollector : MonoBehaviour
{
    [Header("Production Properties")]
    [SerializeField] private float productionTime;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI productionText;
    [SerializeField] private Transform fuelBlock;

    private int fuelAmount;
    private float productionTimeSaved;
    private bool onRange;
    private GameObject astronaut;

    private void Start()
    {
        astronaut = GameObject.FindGameObjectWithTag("Astronauta");

        productionTimeSaved = productionTime;
    }

    private void Update()
    {
        productionTime -= Time.deltaTime;

        if (productionTime <= 0)
            productionTime = productionTimeSaved;

        else
        {
            fuelAmount += 1;
            productionText.text = fuelAmount.ToString();
        }

        if (Input.GetKeyDown(KeyCode.E) && onRange)
        {
            if (fuelAmount > 0 && astronaut.GetComponent<Scr_AstronautsActions>().canGrab)
            {
                astronaut.GetComponent<Scr_AstronautsActions>().canGrab = false;
                fuelAmount -= 1;
                productionText.text = fuelAmount.ToString();

                Instantiate(fuelBlock, astronaut.GetComponent<Scr_AstronautsActions>().pickPoint.transform.position, astronaut.GetComponent<Scr_AstronautsActions>().pickPoint.transform.rotation).SetParent(astronaut.transform);
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Astronaut")
            onRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Astronaut")
            onRange = false;
    }
}