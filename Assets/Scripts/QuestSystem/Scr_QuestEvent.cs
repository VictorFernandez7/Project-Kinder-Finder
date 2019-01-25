using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Scr_QuestEvent
{
    public enum EventStatus { WAITING, CURRENT, DONE };
    //WAITING - not yet completed but can't be worked on cause there's a prerequisite event
    //CURRENT - the one the player should be trying to achieve
    //DONE - has been achieved

    public string name;
    public string description;
    public string id;
    public int order = -1;
    public EventStatus status;

    public List<Scr_QuestPath> pathList = new List<Scr_QuestPath>();

    public Scr_QuestEvent(string n_name, string d_description)
    {
        id = Guid.NewGuid().ToString();
        name = n_name;
        description = d_description;
        status = EventStatus.WAITING;
    }

    public void UpdateQuestEvent(EventStatus es)
    {
        status = es;
    }

    public string GetId()
    {
        return id;
    }
}
