using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int totalSlots = 20;
    public List<InventorySlot> slots = new List<InventorySlot>();

    [SerializeField] Item debugItem;

    public event Action OnInventoryValuesChanged;

    private void Awake()
    {
        for (int i = 0; i < totalSlots; i++)
        {
            slots.Add(new InventorySlot { item = null, quantity = 0 });
        }
    }

    private void Start()
    {
        AddItem(debugItem.itemData, debugItem.itemQuantity);

        for (int i = 0; i < slots.Count; i++)
        {
            Debug.Log($"Slot index in inventory: {i} \n Slot contains item:{slots[i]?.item} \n Item in slot quantity: {slots[i]?.quantity} \n");
        }
    }


    public bool AddItem(ItemSO item, int amount)
    {
        foreach (var slots in slots)
        {
            if(slots.item == item && slots.quantity < item.itemMaxStack)
            {
                int spaceLeftInSlot = item.itemMaxStack - slots.quantity;
                int quantityToAddToSlot = Mathf.Min(spaceLeftInSlot, amount);

                slots.quantity += quantityToAddToSlot;
                amount -= quantityToAddToSlot;

                OnInventoryValuesChanged?.Invoke();

                if (amount <= 0) return true; 
            }
        }

        foreach (var slot in slots)
        {
            if(slot.item == null && amount > 0)
            {
                int quantityToAddToSlot = Mathf.Min(item.itemMaxStack, amount);

                slot.item = item;
                slot.quantity = quantityToAddToSlot;
                amount -= quantityToAddToSlot;

                OnInventoryValuesChanged?.Invoke();

                if (amount <= 0) return true;
            }
        }

        return false;
    }
}
