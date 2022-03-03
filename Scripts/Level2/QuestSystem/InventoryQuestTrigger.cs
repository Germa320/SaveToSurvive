using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryQuestTrigger : MonoBehaviour
{
    Inventory inventory;
    public Transform nextMapPoint;
    bool done = false;

    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
    }

    void Update()
    {
        if(inventory.CheckIfContains("Rock", 13))
        {
            if (!done)
            {
                MissionPointer.instance.ChangeTarget(nextMapPoint);
                done = true;
            }
            
            QuestManager.instance.QuestChange(GetComponent<Quest>().index);
        }
    }
}
