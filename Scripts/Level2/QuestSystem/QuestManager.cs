using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public interface BaseQuest
{
    public string title { get; set; }
    public string description { get; set; }
    public int xp { get; set; }
}

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    public int index = 0;
    public List<Quest> quests = new List<Quest>();
    public TMP_Text questTitle;
    public TMP_Text questInstruction;

    public bool questsComplited = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        questTitle.text = "Look around";
        questInstruction.text = "Go down the track";
        quests[index].SignEvent();
    }

    public event Action onQuestChange;
    public void QuestChange(int questIndex)
    {
        if (questIndex == index)
        {
            onQuestChange?.Invoke();
            index++;
            quests[index].SignEvent();
            if(index == quests.Count - 1)
            {
                questsComplited = true;
            }
        }   
    }
}
