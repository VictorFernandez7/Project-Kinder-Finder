using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_OreZone : MonoBehaviour {

    [Header("Ore Zone Type")]
    [SerializeField] private OreType oreType;


    [Header("Resource Properties")]
    [SerializeField] public float amount;
    [SerializeField] public float zoneSize;

    [HideInInspector] public GameObject currentResource;

    private float initialAmount;
    private Scr_ReferenceManager referenceManager;

    private enum OreType
    {
        iron
    }

    private void Start()
    {
        referenceManager = GameObject.Find("ReferenceManager").GetComponent<Scr_ReferenceManager>();

        initialAmount = amount;

        switch (oreType)
        {
            case OreType.iron:
                currentResource = referenceManager.OreResources[0];
                break;
        }
    }

    private void Update()
    {
        CheckAmount();
        OreZoneSize();
    }

    private void CheckAmount()
    {
        if (amount <= 0)
            Destroy(gameObject);
    }

    private void OreZoneSize()
    {
        GetComponent<CircleCollider2D>().radius = zoneSize;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, zoneSize);
    }
}
