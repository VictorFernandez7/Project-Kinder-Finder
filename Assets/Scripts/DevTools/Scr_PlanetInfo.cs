using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Scr_PlanetInfo
{
    public string m_name;
    public float m_temperature;
    public bool m_oxygen;
    public float m_gravity;
}

public class Scr_SystemData : ScriptableObject
{
    public List<Scr_PlanetData> SystemList;
}

[System.Serializable]
public class Scr_PlanetData
{
    public string m_name;
    public List<Scr_PlanetInfo> PlanetList;
}
