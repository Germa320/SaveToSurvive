using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Quest : MonoBehaviour, BaseQuest
{
    public string title
    {
        get { return questTtile; }
        set { questTtile = value; }
    }
    public string description
    {
        get { return questDescription; }
        set { questDescription = value; }
    }
    public int xp
    {
        get { return questXP; }
        set { questXP = value; }
    }

    public string questTtile;
    public string questDescription;
    public int questXP;
    public bool questCompleted;
    public int index;

    public void SignEvent()
    {
        QuestManager.instance.onQuestChange += ChangeQuest;
    }

    public void ChangeQuest()
    {
        QuestManager.instance.questTitle.text = questTtile;
        QuestManager.instance.questInstruction.text = questDescription;
        QuestManager.instance.onQuestChange -= ChangeQuest;

        if (index == 2)
        {
            FindObjectOfType<PlaceObject>().ActivateRocks();
        }

        if(index == 3)
        {
            FindObjectOfType<PlaceObject>().ActivateWoodPile();
        }
    }

}

