using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float itemDistance;
    Inventory inventory;
    ScrollWheelHandler scrollWheelHandler;
    public AudioSource audioSource;
    public AudioClip pickUpItem;


    void PickUpItem()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, itemDistance))
        {
            IInventoryItem item = hit.transform.GetComponent<IInventoryItem>();
            if (item != null)
            {
                audioSource.PlayOneShot(pickUpItem);
                inventory.AddItem(item);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        scrollWheelHandler = FindObjectOfType<ScrollWheelHandler>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            PickUpItem();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if(inventory.mItems.Count > scrollWheelHandler.index)
            {
                inventory.RemoveItem(inventory.mItems[scrollWheelHandler.index], true);
            }
        }
    }
}
