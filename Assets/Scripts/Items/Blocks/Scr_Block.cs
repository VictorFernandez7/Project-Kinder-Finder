using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Block : MonoBehaviour {

    [Header("Gas Zone Type")]
    [SerializeField] private BlockType blockType;

    [Header("Resource Properties")]
    public float ResistanceTime;

    [HideInInspector] public GameObject currentResource;

    private Scr_ReferenceManager referenceManager;

    private enum BlockType
    {
        iron
    }

    private void Start()
    {
        referenceManager = GameObject.Find("ReferenceManager").GetComponent<Scr_ReferenceManager>();

        switch (blockType)
        {
            case BlockType.iron:
                currentResource = referenceManager.BlockResources[0];
                break;
        }
    }

    private void Update()
    {
        if (ResistanceTime <= 0)
        {
            GameObject resource = Instantiate(currentResource, transform.position, transform.rotation);
            resource.transform.SetParent(transform.parent);
            Destroy(gameObject);
        }   
    }
}
