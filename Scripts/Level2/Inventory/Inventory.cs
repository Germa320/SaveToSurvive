using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Inventory : MonoBehaviour
{
    private const int SLOTS = 7;

    public List<IInventoryItem> mItems = new List<IInventoryItem>();

    public List<int> mItemsCount = new List<int>();

    public event EventHandler<InventoryEventArgs> ItemAdded;

    public event EventHandler<InventoryEventArgs> ItemRemoved;

    public Transform itemHandler;


    bool ContainsItem(IInventoryItem item)
    {
        for (int i = 0; i < mItems.Count; i++)
        {
            if (mItems[i].Name == item.Name)
            {
                return true;
            }
        }

        return false;
    }

    public void AddItem(IInventoryItem item)
    {
        if(mItems.Count < SLOTS || mItems.Contains(item))
        {
            Collider collider = (item as MonoBehaviour).GetComponent<Collider>();
            if (collider.enabled)
            {
                collider.enabled = false;

                bool condition = ContainsItem(item);
                int index = 0;
                if (!condition)
                {
                    mItems.Add(item);
                    mItemsCount.Add(0);
                }

                for (int i = 0; i < mItems.Count; i++)
                {
                    if (mItems[i].Name == item.Name)
                    {
                        index = i;
                        mItemsCount[i]++;
                        
                    }
                }

                item.OnPickup();

                if (ItemAdded != null)
                {
                    ItemAdded(this, new InventoryEventArgs(item, (item as MonoBehaviour).gameObject, condition, index));
                }
            }
        }
    }

    public void RemoveItem(IInventoryItem item, bool spawn)
    {
        bool condition = true;
        int index = 0;

        for (int i = 0; i < mItems.Count; i++)
        {
            if (mItems[i].Name == item.Name)
            {
                index = i;
                mItemsCount[i]--;
                if(mItemsCount[i] == 0)
                {
                    mItemsCount.RemoveAt(i);
                    mItems.Remove(item);
                    condition = false;
                }
            }
        }

        if(spawn) item.OnDrop();

        Collider collider = (item as MonoBehaviour).GetComponent<Collider>();
        if(collider != null)
        {
            collider.enabled = true;
        }

        if (ItemRemoved != null)
        {
            ItemRemoved(this, new InventoryEventArgs(item, (item as MonoBehaviour).gameObject, condition, index));
        }
    }


    public bool CheckIfContains(string itemName, int number)
    {
        for(int i = 0; i < mItems.Count; i++)
        {
            if(mItems[i].Name == itemName)
            {
                if(mItemsCount[i] >= number)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool CheckIfSelected(string itemName)
    {
        if(mItems.Count > ScrollWheelHandler.Instance.index)
        {
            if (mItems[ScrollWheelHandler.Instance.index] != null)
            {
                if (mItems[ScrollWheelHandler.Instance.index].Name == itemName)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
