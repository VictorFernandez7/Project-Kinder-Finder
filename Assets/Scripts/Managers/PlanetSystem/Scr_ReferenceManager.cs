using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_ReferenceManager : MonoBehaviour
{
    [Header("Resources")]
    [SerializeField] public GameObject[] GasResources;
    [SerializeField] public GameObject[] SolidResources;

    [Header("Tools")]
    [SerializeField] public GameObject GasExtractor;
    [SerializeField] public GameObject OreExtractor;
    [SerializeField] public GameObject RepairingTool;
    [SerializeField] public GameObject Jetpack;
}