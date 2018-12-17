using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_TutorialItem : MonoBehaviour
{
    [Header("Set Parent Of")]
    [SerializeField] private Transform targetParent;

    private void Start()
    {
        transform.SetParent(targetParent);
    }
}