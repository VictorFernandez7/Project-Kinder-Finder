using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_PlayerShipStats : MonoBehaviour
{
    [Header("Fuel Properties")]
    [SerializeField] private float maxFuel;
    [SerializeField] private float normalConsume;
    [SerializeField] private float boostConsume;

    [HideInInspector] public float fuel;

    private void Update()
    {
        fuel = Mathf.Clamp(fuel, 0f, maxFuel);
    }

    public void FuelConsumption(bool boost)
    {
        if (boost)
            fuel -= boostConsume;

        else
            fuel -= normalConsume;
    }

    public void ReFuel(float amount)
    {
        fuel += amount;
    }

    public void Death()
    {

    }
}