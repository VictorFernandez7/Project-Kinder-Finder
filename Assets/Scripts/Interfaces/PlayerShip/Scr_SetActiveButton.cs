using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_SetActiveButton : MonoBehaviour
{
    [Header("Select GameObjects")]
    [SerializeField] private GameObject[] setActiveObjects;

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (setActiveObjects.Length > 0)
            {
                for (int i = 0; i < setActiveObjects.Length; i++)
                {
                    setActiveObjects[i].SetActive(!setActiveObjects[i].activeInHierarchy);
                }
            }
        }
    }
}