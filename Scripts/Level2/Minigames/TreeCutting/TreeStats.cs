using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeStats : MonoBehaviour
{
    public int cutsCount = 4;
    Inventory inventory;
    public GameObject pileOfWood;
    public GameObject anotherPileOfWood;

    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        anotherPileOfWood = Instantiate(pileOfWood, pileOfWood.transform);
        anotherPileOfWood.transform.parent = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<Item>() != null)
        {
            if (other.transform.GetComponent<Item>().Name == "Axe")
            {
                cutsCount -= 1;
                if (cutsCount == 0)
                {
                    GetComponent<Rigidbody>().isKinematic = false;
                    GetComponent<Rigidbody>().useGravity = true;
                    anotherPileOfWood.SetActive(true);
                    inventory.AddItem(anotherPileOfWood.GetComponent<Item>());
                    
                }
            }
        }
    }
}
