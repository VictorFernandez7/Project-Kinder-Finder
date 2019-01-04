using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Scr_UpgradeInfo 
{
    public string m_name;
    public SVGImage m_icon;

    [Header("Info")]
    public string m_info;

    [Header("Requirements")]
    public string m_requirements;

    [Header("Resources")]
    public List<string> resourceNameList = new List<string>();
    public List<int> resourceAmountList = new List<int>();

    public bool activated;
}

public class Scr_UpgradeList : ScriptableObject
{
    public List<Scr_UpgradeInfo> UpgradeList;
}
