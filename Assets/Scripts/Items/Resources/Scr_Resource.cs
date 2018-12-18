using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Resource : MonoBehaviour
{
    [Header("Resource Properties")]
    [SerializeField] private ResourceType resourceType;
    [SerializeField] private int typeIndex;

    [HideInInspector] public GameObject resourceReference;

    public enum ResourceType
    {
        gas,
        solid
    }

    private void Start()
    {
        switch (resourceType)
        {
            case ResourceType.gas:
                resourceReference = GameObject.Find("ReferenceManager").GetComponent<Scr_ReferenceManager>().GasResources[typeIndex];
                break;

            case ResourceType.solid:
                resourceReference = GameObject.Find("ReferenceManager").GetComponent<Scr_ReferenceManager>().SolidResources[typeIndex];
                break;
        }
    }
}