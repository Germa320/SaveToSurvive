using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    public Inventory Inventory;
    ScrollWheelHandler scrollWheelHandler;

    void Start()
    {
        Inventory.ItemAdded += InventoryScript_ItemAdded;
        Inventory.ItemRemoved += InventoryScript_ItemRemoved;
        scrollWheelHandler = FindObjectOfType<ScrollWheelHandler>();
    }

    private void InventoryScript_ItemAdded(object sender, InventoryEventArgs e)
    {
        RuntimePreviewGenerator.BackgroundColor = new Color(0, 0, 0, 0.5f);

        Transform inventoryPanel = transform.Find("Inventory_UI");
        if (!e.alreadyIs)
        {
            foreach (Transform slot in inventoryPanel)
            {
                RawImage image = slot.transform.GetChild(0).GetComponent<RawImage>();
                TMP_Text number = image.transform.GetChild(0).GetComponent<TMP_Text>();

                if (!image.enabled)
                {
                    image.enabled = true;
                    image.texture = RuntimePreviewGenerator.GenerateModelPreview(e.itemObject.transform);
                    number.enabled = true;
                    number.text = Inventory.mItemsCount[e.index].ToString();

                    break;
                }
            }
        }
        else
        {
            TMP_Text number = scrollWheelHandler.slots[e.index].transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
            number.text = Inventory.mItemsCount[e.index].ToString();
        }
        
    }

    private void InventoryScript_ItemRemoved(object sender, InventoryEventArgs e)
    {
        Transform inventoryPanel = transform.Find("Inventory_UI");

        GameObject slot = inventoryPanel.GetChild(e.index).gameObject;
        RawImage image = slot.transform.GetChild(0).GetComponent<RawImage>();
        TMP_Text number = image.transform.GetChild(0).GetComponent<TMP_Text>();

        if (e.alreadyIs)
        {
            number.text = Inventory.mItemsCount[e.index].ToString();
        }
        else
        {
            if (image.enabled)
            {
                image.enabled = false;
                image.texture = null;
                number.enabled = false;
            }

            MoveImagesIfEmpyInBetween(inventoryPanel);
        }
    }


    private void MoveImagesIfEmpyInBetween(Transform inventoryPanel)
    {
        for(int i = scrollWheelHandler.index; i < Inventory.mItems.Count; i++)
        {
            GameObject currentSlot = inventoryPanel.GetChild(i).gameObject;
            RawImage currentImage = currentSlot.transform.GetChild(0).GetComponent<RawImage>();
            TMP_Text currentText = currentImage.transform.GetChild(0).GetComponent<TMP_Text>();
            GameObject nextSlot = inventoryPanel.GetChild(i + 1).gameObject;
            RawImage nextImage = nextSlot.transform.GetChild(0).GetComponent<RawImage>();
            TMP_Text nextText = nextImage.transform.GetChild(0).GetComponent<TMP_Text>();
            if (nextImage.enabled)
            {
                currentImage.texture = nextImage.texture;
                currentText.text = nextText.text;
                currentImage.enabled = true;
                currentText.enabled = true;
                nextImage.texture = null;
                nextImage.enabled = false;
                nextText.enabled = false;
            }
        }
    }

    private void UpdateSlotNumber()
    {
        Transform inventoryPanel = transform.Find("Inventory_UI");

        GameObject slot = inventoryPanel.GetChild(scrollWheelHandler.index).gameObject;
        RawImage image = slot.transform.GetChild(0).GetComponent<RawImage>();
    }
}
