using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_PlayershipCraft : MonoBehaviour {

    [SerializeField] private Dictionary<string, int> Resources = new Dictionary<string, int>();

    private Scr_PlayerShipStats playerShipStats;

    private void Start()
    {
        playerShipStats = GetComponent<Scr_PlayerShipStats>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            Resources.Clear();
            CalculateResources();

            List<string> keyr = new List<string>(Resources.Keys);

            foreach (string k in keyr)
            {
                Debug.Log(k + " " + Resources[k]);
            }
        }
    }

    private void CalculateResources()
    {
        
        for (int i = 0; i < playerShipStats.resourceWarehouse.Length; i++)
        {
            List<string> keys = new List<string>(Resources.Keys);

            if (!playerShipStats.resourceWarehouse[i])
            {
                break;
            }

            else
            {
                if (Resources.Count != 0)
                {
                    foreach (string key in keys)
                    {
                        if (key == playerShipStats.resourceWarehouse[i].name)
                        {
                            Resources[key] += 1;
                            break;
                        }

                        else
                        {
                            Resources.Add(playerShipStats.resourceWarehouse[i].name, 1);
                            break;
                        }
                    }
                }

                else
                    Resources.Add(playerShipStats.resourceWarehouse[i].name, 1);
            }
        }
    }
}
