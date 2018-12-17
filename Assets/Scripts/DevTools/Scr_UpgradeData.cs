using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Scr_UpgradeData 
{
    public string m_name;
    public SVGImage m_icon;

    [Header("Info")]
    public string m_info;

    [Header("Requirements")]
    public string m_requirements;

    [Header("Resources")]
    public int m_fuel;
    public int m_iron;
}

public class Scr_UpgradeList : ScriptableObject
{
    public List<Scr_UpgradeData> UpgradeList;
}
