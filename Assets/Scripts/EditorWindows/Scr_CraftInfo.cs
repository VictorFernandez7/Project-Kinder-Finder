using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Scr_CraftInfo
{
    public string m_name;
    public SVGImage m_icon;

    [Header("Info")]
    public string m_info;

    [Header("Resources")]
    public Dictionary<string, int> Resources = new Dictionary<string, int>();
}

public class Scr_CraftData : ScriptableObject
{
    public List<Scr_CraftInfo> CraftList;
}
