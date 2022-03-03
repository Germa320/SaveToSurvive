using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IInventoryItem
{
    public string itemName;

    public string Name
    {
        get
        {
            return itemName;
        }
    }

    public void OnPickup()
    {
        gameObject.SetActive(false);
    }

    public void OnDrop()
    {
        RaycastHit hit = new RaycastHit();
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 10))
        {
            GameObject objectToSpawn = Instantiate(this.gameObject, transform.position, transform.rotation);
            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = hit.point;
        }
    }

    public GameObject OnHold()
    {
        GameObject objectToSpawn = Instantiate(this.gameObject, transform.position, transform.rotation);
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = ScrollWheelHandler.Instance.itemHandler.transform.position;
        objectToSpawn.transform.parent = ScrollWheelHandler.Instance.itemHandler.transform.parent;
        return objectToSpawn;
    }
}
