using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_OreZone : MonoBehaviour
{
    [Header("Ore Zone Type")]
    [SerializeField] private OreType oreType;

    [Header("Resource Properties")]
    [SerializeField] public float amount;

    [Header("References")]
    [SerializeField] private Scr_ReferenceManager referenceManager;

    [HideInInspector] public GameObject currentResource;

    private float initialAmount;

    private enum OreType
    {
        iron
    }

    private void Start()
    {
        initialAmount = amount;

        switch (oreType)
        {
            case OreType.iron:
                currentResource = referenceManager.SolidResources[0];
                break;
        }
    }

    private void Update()
    {
        CheckAmount();
    }

    private void CheckAmount()
    {
        if (amount <= 0)
            Destroy(gameObject);
    }
}