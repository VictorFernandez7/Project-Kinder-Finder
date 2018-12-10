using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundDefition", menuName = "HumanHorizon/Sound", order = 1)]
public class SoundDefinition : ScriptableObject {

	[SerializeField]
    Scr_MusicManager.SoundData sound;

    public Scr_MusicManager.SoundData Sound { get { return sound; } }
}
