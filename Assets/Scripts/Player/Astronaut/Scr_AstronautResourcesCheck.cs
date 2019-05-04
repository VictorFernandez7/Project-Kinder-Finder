using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_AstronautResourcesCheck : MonoBehaviour
{
    public List<GameObject> resourceList = new List<GameObject>();
    private int count = 0;

    [HideInInspector] public GameObject miningSpot;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            for(int i = 0; i < resourceList.Count; i++)
            {
                print(i + " >>  " + resourceList[i].GetComponent<Scr_Resource>().iD);
            }
        }

        if (count != resourceList.Count)
        {
            for (int i = 0; i < resourceList.Count; i++)
            {
                resourceList[i].GetComponent<Scr_Resource>().iD = i;
                count = resourceList.Count;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Resources"))
            resourceList.Add(collision.gameObject);

        if (collision.CompareTag("MiningSpot"))
            miningSpot = collision.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Resources"))
        {
            for (int i = 0; i < resourceList.Count; i++)
            {
                if(resourceList[i].name == collision.gameObject.name && resourceList[i].GetComponent<Scr_Resource>().iD == collision.gameObject.GetComponent<Scr_Resource>().iD)
                    resourceList.RemoveAt(i);
            }
        }

        if (collision.CompareTag("MiningSpot"))
            miningSpot = null;
    }
}
