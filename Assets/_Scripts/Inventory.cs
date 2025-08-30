using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Range(5, 100)]
    public int totalSlots = 20;
    public List<InventorySlot> slots = new List<InventorySlot>();


    public event Action OnInventoryValuesChanged;

    private void Awake()
    {
        for (int i = 0; i < totalSlots; i++)
        {
            slots.Add(new InventorySlot { item = null, quantity = 0 });
        }
    }

    public bool AddItem(Item item)
    {

        int remainingAmoutn = item.itemQuantity;

        foreach (var slots in slots)
        {
            if(slots.item == item.itemData && slots.quantity < item.itemData.itemMaxStack)
            {
                int spaceLeftInSlot = item.itemData.itemMaxStack - slots.quantity;
                int quantityToAddToSlot = Mathf.Min(spaceLeftInSlot, remainingAmoutn);

                slots.quantity += quantityToAddToSlot;
                remainingAmoutn -= quantityToAddToSlot;

                OnInventoryValuesChanged?.Invoke();

                if (remainingAmoutn <= 0) break;
            }
        }

        foreach (var slot in slots)
        {
            if(slot.item == null && remainingAmoutn > 0)
            {
                int quantityToAddToSlot = Mathf.Min(item.itemData.itemMaxStack, remainingAmoutn);

                slot.item = item.itemData;
                slot.quantity = quantityToAddToSlot;
                remainingAmoutn -= quantityToAddToSlot;

                OnInventoryValuesChanged?.Invoke();

                if (remainingAmoutn <= 0) break;
            }
        }

        item.itemQuantity = remainingAmoutn;
        return remainingAmoutn <= 0;
    }
}
