using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour {


    [SerializeField]
    SoundDefinition step;

    bool played = false;
    private void Start()
    {
    }

    // Update is called once per frame
    void Update () {
        if (!played)
        {
            //Scr_MusicManager.Instance.PlaySound(step.Sound);

        }
	}
}
