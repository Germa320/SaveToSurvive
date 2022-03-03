using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollWheelHandler : MonoBehaviour
{
    public Color selected;
    public Color unselected;

    Inventory inventory;

    public List<GameObject> slots = new List<GameObject>();
    GameObject currentSlot;
    [HideInInspector] public int index;
    public Transform itemHandler;

    private static ScrollWheelHandler instance;

    public static ScrollWheelHandler Instance { get { return instance; } }
    GameObject itemToHold;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        currentSlot = slots[0];
        index = 0;
        currentSlot.transform.GetComponent<Image>().color = selected;
    }

    void Update()
    {

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if(index == slots.Count - 1)
            {
                Debug.Log("Dont change");
            }
            else
            {
                currentSlot.transform.GetComponent<Image>().color = unselected;
                index++;
                currentSlot = slots[index];
                currentSlot.transform.GetComponent<Image>().color = selected;
                if (inventory.mItems.Count > index)
                {
                    if(itemToHold != null)
                    {
                        Destroy(itemToHold);
                    }
                    itemToHold = inventory.mItems[index].OnHold();
                }
                
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if(index == 0)
            {
                Debug.Log("Dont change");
            }
            else
            {
                currentSlot.transform.GetComponent<Image>().color = unselected;
                index--;
                currentSlot = slots[index];
                currentSlot.transform.GetComponent<Image>().color = selected;
                if (inventory.mItems.Count > index)
                {
                    if (itemToHold != null)
                    {
                        Destroy(itemToHold);
                    }
                    itemToHold = inventory.mItems[index].OnHold();
                }
            }
        }
    }
}
