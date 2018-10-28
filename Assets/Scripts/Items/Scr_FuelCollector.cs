using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]

public class Scr_FuelCollector : MonoBehaviour
{
    [Header("Production Properties")]
    [SerializeField] private float productionTime;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI productionText;
    [SerializeField] private Transform fuelBlock;

    [HideInInspector] public bool canCollect;
    [HideInInspector] public int fuelAmount;
    
    private float productionTimeSaved;
    private bool onRange;
    private GameObject astronaut;
    private Scr_GameManager gameManager;
    private Scr_AstronautMovement astronautMovement;

    private void Start()
    {
        astronautMovement = GameObject.Find("Astronaut").GetComponent<Scr_AstronautMovement>();
        gameManager = GameObject.Find("GameManager").GetComponent<Scr_GameManager>();

        productionTimeSaved = productionTime;

        transform.SetParent(gameManager.initialPlanet.transform);
    }

    private void Update()
    {
        productionTimeSaved -= Time.deltaTime;
        productionText.text = fuelAmount.ToString();

        if (productionTimeSaved <= 0)
        {
            fuelAmount += 1;
            productionTimeSaved = productionTime;
        }

        if (fuelAmount > 0)
            canCollect = true;
        else
            canCollect = false;
    }

    public void CollectFuel()
    {
        if (canCollect)
            fuelAmount -= 1;
    }
}