using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour {


    [SerializeField]
    SoundDefinition sound;

    bool played = false;
    private void Start()
    {
    }

    // Update is called once per frame
    void Update () {
        if (!played)
        {
            Debug.Log(sound.Sound.soundType);
            Scr_MusicManager.Instance.PlaySound(sound.Sound);
            played = true;
        }
	}
}
