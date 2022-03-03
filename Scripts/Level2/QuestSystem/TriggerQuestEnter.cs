using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerQuestEnter : MonoBehaviour
{
    public Transform nextMapPoint;
    bool done = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!done)
        {
            MissionPointer.instance.ChangeTarget(nextMapPoint);
            done = true;
        }
        
        QuestManager.instance.QuestChange(GetComponent<Quest>().index);
    }
}
