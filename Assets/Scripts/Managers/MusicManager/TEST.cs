using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour {

	// Use this for initialization
	void Start () {
        MusicManager.Instance.PlayBackgroundMusic(FixedSound.Discovery);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
