using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Quest 
{
    public List<Scr_QuestEvent> questEvents = new List<Scr_QuestEvent>();

    public Scr_Quest() { }
    
    public Scr_QuestEvent AddQuestEvent(string n_name, string d_description)
    {
        Scr_QuestEvent questEvent = new Scr_QuestEvent(n_name, d_description);
        questEvents.Add(questEvent);
        return questEvent;
    }

    public void AddPath(string fromQuestEvent, string toQuestEvent)
    {
        Scr_QuestEvent from = FindQuestEvent(fromQuestEvent);
        Scr_QuestEvent to = FindQuestEvent(toQuestEvent);

        if(from != null && to != null)
        {
            Scr_QuestPath p = new Scr_QuestPath(from, to);
            from.pathList.Add(p);
        }
    }

    Scr_QuestEvent FindQuestEvent(string id)
    {
        foreach(Scr_QuestEvent n in questEvents)
        {
            if (n.GetId() == id)
                return n;
        }
        return null;
    }

    public void BFS(string id, int orderNumber = 1)
    {
        Scr_QuestEvent thisEvent = FindQuestEvent(id);
        thisEvent.order = orderNumber;

        foreach(Scr_QuestPath e in thisEvent.pathList)
        {
            if (e.endEvent.order == -1)
                BFS(e.endEvent.GetId(), orderNumber + 1);
        }
    }

    public void PrintPath()
    {
        foreach(Scr_QuestEvent n in questEvents)
        {
            Debug.Log(n.name + " " + n.order);
        }
    }
}
