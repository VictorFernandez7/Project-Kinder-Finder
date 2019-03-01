using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Ore : MonoBehaviour
{
    [Header("Ore Block Type")]
    [SerializeField] private OreResourceType oreResourceType;
    [SerializeField] private CrystalResourceType crystalResourceType;
    [SerializeField] private BlockType blockType;

    [Header("Resource Properties")]
    public float amount;

    [Header("References")]
    [SerializeField] private Scr_ReferenceManager referenceManager;

    [HideInInspector] public GameObject currentResource;

    private float initalAmount;
    private float rest = 1;

    private enum BlockType
    {
        Ore,
        Crystal
    }

    private enum OreResourceType
    {
        Oxygen,
        Fuel,
        Carbon,
        Silicon,
        Iron,
        Aluminum,
        Magnetite,
        Ceramic,
        Termatite
    }

    private enum CrystalResourceType
    {
        Crystal
    }

    private void Start()
    {
        initalAmount = amount;

        switch(blockType)
        {
            case BlockType.Ore:
                switch (oreResourceType)
                {
                    case OreResourceType.Oxygen:
                        currentResource = referenceManager.Resources[0];
                        break;

                    case OreResourceType.Fuel:
                        currentResource = referenceManager.Resources[1];
                        break;

                    case OreResourceType.Carbon:
                        currentResource = referenceManager.Resources[2];
                        break;

                    case OreResourceType.Silicon:
                        currentResource = referenceManager.Resources[3];
                        break;

                    case OreResourceType.Iron:
                        currentResource = referenceManager.Resources[6];
                        break;

                    case OreResourceType.Aluminum:
                        currentResource = referenceManager.Resources[7];
                        break;

                    case OreResourceType.Magnetite:
                        currentResource = referenceManager.Resources[8];
                        break;

                    case OreResourceType.Ceramic:
                        currentResource = referenceManager.Resources[10];
                        break;

                    case OreResourceType.Termatite:
                        currentResource = referenceManager.Resources[11];
                        break;
                }
                break;

            case BlockType.Crystal:
                switch (crystalResourceType)
                {
                    case CrystalResourceType.Crystal:

                        break;
                }
                break;
        }
    }

    private void Update()
    {
        if(amount <= (initalAmount - rest))
        {
            rest += 1;
            GameObject resource = Instantiate(currentResource, transform.position, transform.rotation);
            resource.transform.SetParent(transform.parent);
        }

        if (amount <= 0)
        {
            GameObject resource = Instantiate(currentResource, transform.position, transform.rotation);
            resource.transform.SetParent(transform.parent);
            Destroy(gameObject);
        }   
    }
}