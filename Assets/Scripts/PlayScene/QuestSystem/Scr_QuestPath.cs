using System.Collections;
using UnityEngine;

public class Scr_QuestPath 
{
    public Scr_QuestEvent startEvent;
    public Scr_QuestEvent endEvent;

    public Scr_QuestPath(Scr_QuestEvent from, Scr_QuestEvent to)
    {
        startEvent = from;
        endEvent = to;
    }
}
