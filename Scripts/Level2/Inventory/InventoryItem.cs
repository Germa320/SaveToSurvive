using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public interface IInventoryItem
{
    string Name { get; }

    void OnPickup();

    void OnDrop();

    GameObject OnHold();
}

public class InventoryEventArgs : EventArgs
{
    public InventoryEventArgs(IInventoryItem item, GameObject obj, bool condition, int ind)
    {
        Item = item;
        itemObject = obj;
        alreadyIs = condition;
        index = ind;
    }

    public GameObject itemObject;
    public IInventoryItem Item;
    public bool alreadyIs;
    public int index;
}
