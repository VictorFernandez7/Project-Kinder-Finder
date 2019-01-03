﻿using System.Collections;
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
    public int m_fuel;
    public int m_iron;
}

public class Scr_CraftData : ScriptableObject
{
    public List<Scr_CraftInfo> CraftList;
}