using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VolumeSettings", menuName = "HumanHorizon/MusicLibrary", order = 1)]
public class VolumeSettings : ScriptableObject {
    
    public List<Scr_MusicManager.VolumeSetting> volumeSettings;
}
