using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Scr_PlayerShipActions))]
[RequireComponent(typeof(Scr_PlayerShipPrediction))]
[RequireComponent(typeof(Scr_PlayerShipMovement))]

public class Scr_PlayerShipStats : MonoBehaviour
{
    [Header("Fuel Properties")]
    [SerializeField] public float currentFuel;
    [SerializeField] private float maxFuel;
    [SerializeField] private float normalConsume;
    [SerializeField] private float boostConsume;

    private void Update()
    {
        currentFuel = Mathf.Clamp(currentFuel, 0f, maxFuel);
    }

    public void FuelConsumption(bool boost)
    {
        if (boost)
            currentFuel -= boostConsume;

        else
            currentFuel -= normalConsume;
    }

    public void ReFuel(float amount)
    {
        currentFuel += amount;
    }

    public void Death()
    {

    }
}