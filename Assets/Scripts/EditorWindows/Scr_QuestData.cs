using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Scr_QuestInfo
{
    public string name;
    public string description;

    public List<Scr_EventInfo> EventList = new List<Scr_EventInfo>();
}

[System.Serializable]
public class Scr_EventInfo
{
    public string name;
    public string description;

    public int from;
    public int to;
}

public class Scr_QuestData : ScriptableObject
{
    public List<Scr_QuestInfo> QuestList = new List<Scr_QuestInfo>();
}
