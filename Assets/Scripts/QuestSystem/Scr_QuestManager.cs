using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scr_QuestManager : MonoBehaviour
{
    public List<Scr_Quest> quest = new List<Scr_Quest>();

    private void Start()
    {/*
        //create each event
        Scr_QuestEvent a = quest.AddQuestEvent("test1", "description 1");
        Scr_QuestEvent b = quest.AddQuestEvent("test2", "description 2");
        Scr_QuestEvent c = quest.AddQuestEvent("test3", "description 3");
        Scr_QuestEvent d = quest.AddQuestEvent("test4", "description 4");
        Scr_QuestEvent e = quest.AddQuestEvent("test5", "description 5");

        //define the paths between the events
        quest.AddPath(a.GetId(), c.GetId());
        quest.AddPath(b.GetId(), c.GetId());
        quest.AddPath(b.GetId(), e.GetId());
        quest.AddPath(c.GetId(), e.GetId());
        quest.AddPath(d.GetId(), e.GetId());

        quest.BFS(a.GetId());

        quest.PrintPath();*/
    }

    private void CreateQuest()
    {
        Scr_Quest mission = new Scr_Quest();
        quest.Add(mission);

    }
}
