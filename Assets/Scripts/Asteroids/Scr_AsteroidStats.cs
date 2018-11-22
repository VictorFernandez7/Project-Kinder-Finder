using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_AsteroidStats : MonoBehaviour
{
    [Header("Resources")]
    [SerializeField] public float steelAmount;
    [SerializeField] public GameObject steelBlock;

    [Header("References")]
    [SerializeField] public float maxHealth;

    [HideInInspector] public float currentHealth;

    private float steelSaved;
    private Scr_PlayerShipActions playerShipActions;

    private void Start()
    {
        playerShipActions = GameObject.Find("PlayerShip").GetComponent<Scr_PlayerShipActions>();

        steelSaved = steelAmount;
    }

    private void Update()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (steelAmount < steelSaved)
        {
            //Instantiate(steelBlock, playerShipActions.laserHitPosition, playerShipActions.transform.rotation);
            steelAmount = steelSaved;
        }
    }
}