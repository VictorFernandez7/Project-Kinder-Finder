using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Scr_LevelInfo
{
    public string m_name;
    public int experienceNeeded;

    public List<int> levelRewards = new List<int>();
}

public class Scr_LevelData : ScriptableObject
{
    public List<Scr_LevelInfo> LevelList;
}