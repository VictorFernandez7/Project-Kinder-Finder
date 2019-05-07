using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Ore : MonoBehaviour
{
    [Header("Block Type")]
    [SerializeField] private BlockType blockType;

    [Header("If Ore")]
    [SerializeField] private OreResourceType oreResourceType;

    [Header("If Crystal")]
    [SerializeField] private CrystalResourceType crystalResourceType;

    [Header("Resource Properties")]
    public float amount;

    [Header("References")]
    [SerializeField] private Scr_ReferenceManager referenceManager;
    [SerializeField] private Scr_PlayerShipStats playerShipStats;
    [SerializeField] private Scr_AstronautsActions astronautsActions;
    [SerializeField] private GameObject visuals;

    [HideInInspector] public GameObject currentResource;

    private float initalAmount;
    private float rest = 1;
    private GameObject[] oreVisuals;

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
        oreVisuals = new GameObject[visuals.transform.childCount];

        GetVisualsChildren();

        switch (blockType)
        {
            case BlockType.Ore:
                switch (oreResourceType)
                {
                    case OreResourceType.Oxygen:
                        currentResource = referenceManager.Resources[0];
                        ActivateTargetVisuals(0);
                        break;

                    case OreResourceType.Fuel:
                        currentResource = referenceManager.Resources[1];
                        ActivateTargetVisuals(1);
                        break;

                    case OreResourceType.Carbon:
                        currentResource = referenceManager.Resources[2];
                        ActivateTargetVisuals(2);
                        break;

                    case OreResourceType.Silicon:
                        currentResource = referenceManager.Resources[3];
                        ActivateTargetVisuals(3);
                        break;

                    case OreResourceType.Iron:
                        currentResource = referenceManager.Resources[6];
                        ActivateTargetVisuals(4);
                        break;

                    case OreResourceType.Aluminum:
                        currentResource = referenceManager.Resources[7];
                        ActivateTargetVisuals(5);
                        break;

                    case OreResourceType.Magnetite:
                        currentResource = referenceManager.Resources[8];
                        ActivateTargetVisuals(6);
                        break;

                    case OreResourceType.Ceramic:
                        currentResource = referenceManager.Resources[10];
                        ActivateTargetVisuals(7);
                        break;

                    case OreResourceType.Termatite:
                        currentResource = referenceManager.Resources[11];
                        ActivateTargetVisuals(8);
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
        if (amount <= (initalAmount - rest))
        {
            rest += 1;
            playerShipStats.GetExperience(20);
            GameObject resource = Instantiate(currentResource, transform.position + Vector3.back, transform.rotation);
            resource.transform.SetParent(transform.parent);
        }

        if (amount <= 0)
        {
            GameObject resource = Instantiate(currentResource, transform.position + Vector3.back, transform.rotation);
            resource.transform.SetParent(transform.parent);
            astronautsActions.miningSpot = null;
            Destroy(gameObject);
        }   
    }

    private void GetVisualsChildren()
    {
        for (int i = 0; i < visuals.transform.childCount; i++)
        {
            oreVisuals[i] = visuals.transform.GetChild(i).gameObject;
        }
    }

    private void ActivateTargetVisuals(int index)
    {
        for (int i = 0; i < oreVisuals.Length; i++)
        {
            if (i == index)
                oreVisuals[i].SetActive(true);

            else
                oreVisuals[i].SetActive(false);
        }
    }
}