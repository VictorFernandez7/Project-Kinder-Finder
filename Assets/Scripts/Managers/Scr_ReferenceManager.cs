using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_ReferenceManager : MonoBehaviour {

    [Header("Resources")]
    public GameObject[] GasResources;
    public GameObject[] BlockResources;

    [Header("Tools")]
    public GameObject GasExtractor;
    public GameObject RepairingTool;
    public GameObject Jetpack;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
