using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Scr_CraftInfo
{
    public string m_name;
    public Image m_icon;

    [Header("Info")]
    public string m_info;

    [Header("Resources")]
    public List<string> resourceNameList = new List<string>();
    public List<int> resourceAmountList = new List<int>();

    public CraftType craftType = new CraftType();

    public int ToolNum;

    public bool crafteable;
}

public enum CraftType
{
    tools,
    suits,
    spaceship,
    jumpCell,
    spacewalk,
    jetpack,
    miningLaser
}

public class Scr_CraftData : ScriptableObject
{
    public List<Scr_CraftInfo> CraftList;
}
