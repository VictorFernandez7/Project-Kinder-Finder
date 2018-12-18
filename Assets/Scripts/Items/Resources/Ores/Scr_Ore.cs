using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Ore : MonoBehaviour
{
    [Header("Ore Block Type")]
    [SerializeField] private BlockType blockType;

    [Header("Resource Properties")]
    public float resistanceTime;

    [Header("References")]
    [SerializeField] private Scr_ReferenceManager referenceManager;

    [HideInInspector] public GameObject currentResource;

    private enum BlockType
    {
        iron
    }

    private void Start()
    {
        switch (blockType)
        {
            case BlockType.iron:
                currentResource = referenceManager.SolidResources[0];
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