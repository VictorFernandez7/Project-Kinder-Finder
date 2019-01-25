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
    public float resistanceTime;

    [Header("References")]
    [SerializeField] private Scr_ReferenceManager referenceManager;

    [HideInInspector] public GameObject currentResource;

    private enum BlockType
    {
        Ore,
        Crystal
    }

    private enum OreResourceType
    {
        Iron,
        Copper
    }

    private enum CrystalResourceType
    {
        Crystal
    }

    private void Start()
    {
        switch(blockType)
        {
            case BlockType.Ore:
                switch (oreResourceType)
                {
                    case OreResourceType.Iron:
                        currentResource = referenceManager.SolidResources[0];
                        break;

                    case OreResourceType.Copper:
                        currentResource = referenceManager.SolidResources[1];
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
        if (resistanceTime <= 0)
        {
            GameObject resource = Instantiate(currentResource, transform.position, transform.rotation);
            resource.transform.SetParent(transform.parent);
            Destroy(gameObject);
        }   
    }
}