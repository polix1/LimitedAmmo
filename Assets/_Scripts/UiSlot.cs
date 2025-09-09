using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class UiSlot : MonoBehaviour, IPointerClickHandler
{
    public Image icon;
    public TMP_Text quanitytText;
    public GameObject slotSelectHighlight;
    public bool isSelected = false;
    public InventorySlot InventorySlot;

    public event Action<InventorySlot> OnSlotValuesChanged;



    public void UpdateSlotUi(ItemSO itemData, int quantity)
    {

        if (isSelected)
        {
            slotSelectHighlight.SetActive(true);
        }
        else
        {
            slotSelectHighlight.SetActive(false);
        }
        if (itemData != null)
        {
            icon.gameObject.SetActive(true);
            quanitytText.gameObject.SetActive(true);
            icon.sprite = itemData.itemIcon;
            quanitytText.text = quantity.ToString();

        }
        else if(itemData == null)
        {
            icon.gameObject.SetActive(false);
            quanitytText.gameObject.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //PickUp
        if (MoverSlot.item == null && InventorySlot.item != null)
        {
            MoverSlot.item = InventorySlot.item;
            InventorySlot.item = null;
        }

        //Put in slot 
        else if (MoverSlot.item != null && InventorySlot.item == null)
        {
            InventorySlot.item = MoverSlot.item;
            MoverSlot.item = null;
        }

        //swap
        else if (MoverSlot.item != null && InventorySlot.item != null)
        {
            if (MoverSlot.item.itemData == InventorySlot.item.itemData)
            {
                int spaceLeftInSlot = InventorySlot.item.itemData.itemMaxStack - InventorySlot.item.itemQuantity;

                if (spaceLeftInSlot > 0)
                {
                    int toAdd = Mathf.Min(spaceLeftInSlot, MoverSlot.item.itemQuantity);
                    InventorySlot.item.itemQuantity += toAdd;

                    MoverSlot.item.itemQuantity -= toAdd;

                    if (MoverSlot.item.itemQuantity == 0)
                    {
                        MoverSlot.item = null;
                    }

                    return;
                }
            }
            Item temp = InventorySlot.item;
            InventorySlot.item = MoverSlot.item;
            MoverSlot.item = temp;
        }


        if (InventorySlot.item != null)
            UpdateSlotUi(InventorySlot.item.itemData, InventorySlot.item.itemQuantity);
        else
            UpdateSlotUi(null, 0); // clear the slot UI
    }
}
