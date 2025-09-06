using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Range(5, 100)]
    public int totalSlots = 20;
    public List<InventorySlot> slots = new List<InventorySlot>();


    public event Action OnInventoryValuesChanged;

    [SerializeField] Transform itemPoolParent;

    public Dictionary<Item, GameObject> itemPool = new Dictionary<Item, GameObject>();

    [SerializeField] LayerMask armsMask;


    private void Awake()
    {
        for (int i = 0; i < totalSlots; i++)
        {
            slots.Add(new InventorySlot { item = null });
        }

    }

    public bool AddItem(Item item)
    {

        int remainingAmoutn = item.itemQuantity;

        foreach (var slot in slots)
        {
            if(slot.item != null && slot.item.itemData == item.itemData && slot.item.itemQuantity < item.itemData.itemMaxStack)
            {
                int spaceLeft = item.itemData.itemMaxStack - slot.item.itemQuantity;
                int toAdd = Mathf.Min(spaceLeft, remainingAmoutn);


                slot.item.itemQuantity += toAdd;
                remainingAmoutn -= toAdd;

                OnInventoryValuesChanged?.Invoke();

                if (remainingAmoutn <= 0) break;
            }
        }

        foreach (var slot in slots)
        {
            if(slot.item == null && remainingAmoutn > 0)
            {
                int toAdd = Mathf.Min(item.itemData.itemMaxStack, remainingAmoutn);


                GameObject itemStack = Instantiate(item.itemData.itemWorldPrefab, itemPoolParent);
                Item itemInstance = itemStack.GetComponent<Item>();
                itemInstance.itemData = item.itemData;
                itemInstance.itemQuantity = toAdd;
                itemStack.SetActive(false);

                int armsLayerIndex = Mathf.RoundToInt(Mathf.Log(armsMask.value, 2));
                itemStack.layer = armsLayerIndex;

                if (!itemPool.ContainsKey(itemInstance))
                {
                    itemPool.Add(itemInstance, itemStack);
                }

                slot.item = itemInstance;

                remainingAmoutn -= toAdd;

                slot.item.isInInventory = true;

                OnInventoryValuesChanged?.Invoke();

                if (remainingAmoutn <= 0) break;
            }
        }

        foreach (KeyValuePair<Item, GameObject> pooledItems in itemPool)
        {
            Debug.Log(pooledItems.Key + " " + pooledItems.Value);
            pooledItems.Value?.GetComponent<Item>().UpdatePhysics();
        }

        item.itemQuantity = remainingAmoutn;
        return remainingAmoutn <= 0;
    }

    public GameObject GetItem(Item item)
    {
        if (item == null) return null;

        if(itemPool.ContainsKey(item))
        {
            return itemPool[item];
        }
        return null;
    }


    //Removes the item completly from the inventory even if more than one
    public void RemoveItem(Item item)
    {
        foreach(var slot in slots)
        {
            if(slot.item == item)
            {
                slot.item = null;

                itemPool.Remove(item);
            }
        }
    }

    //Removes a spesific amount from a spesefic item.. not done [x]
    public void SubtractItem(Item item, int amount)
    {
        foreach (var slot in slots)
        {
            if(slot.item == item)
            {
                slot.item.itemQuantity -= amount;
            }
        }
    }

    //Removes an item based on a spisfic type from all the inventory 
    public void SubtractItemOfType(Item item, int amount)
    {
        int remainingAmount = amount;

        foreach (var slot in slots)
        {
            if(slot.item != null && slot.item.itemData == item.itemData)
            {
                if(slot.item.itemQuantity >= remainingAmount)
                {
                    slot.item.itemQuantity -= remainingAmount;
                    remainingAmount = 0;
                    break;
                }
                else
                {
                    remainingAmount -= slot.item.itemQuantity;
                    slot.item.itemQuantity = 0;

                    slot.item = null;
                }
            }

        }

        OnInventoryValuesChanged?.Invoke();
    }
}
