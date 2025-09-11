using System;
using System.Collections.Generic;
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
            if (slot.item != null && slot.item.itemData == item.itemData && slot.item.itemQuantity < item.itemData.itemMaxStack)
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
            if (slot.item == null && remainingAmoutn > 0)
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
            pooledItems.Value?.GetComponent<Item>().UpdatePhysics();
        }

        item.itemQuantity = remainingAmoutn;
        return remainingAmoutn <= 0;
    }

    public GameObject GetItem(Item item)
    {
        if (item == null) return null;

        if (itemPool.ContainsKey(item))
        {
            return itemPool[item];
        }
        return null;
    }

    

    public void RemoveItem(Item item)
    {
        if (itemPool.ContainsKey(item))
        {
            Destroy(itemPool[item]);
            itemPool.Remove(item);
        }
    }

    public void RemoveItemFromSlot(InventorySlot slot)
    {
        int indexOfSlot = slots.IndexOf(slot);

        if (indexOfSlot != -1 && slots[indexOfSlot].item != null)
        {
            RemoveItem(slots[indexOfSlot].item);
            slots[indexOfSlot].item = null;

            OnInventoryValuesChanged?.Invoke();
        }
    }

    public bool SubtractItemFromSlot(InventorySlot inventorySlot, int amount)
    {
        int remainingAmoung = amount;
        int indexOfSlot = slots.IndexOf(inventorySlot);

        if (indexOfSlot != -1 && slots[indexOfSlot].item != null)
        {
            if (slots[indexOfSlot].item.itemQuantity >= amount)
            {
                slots[indexOfSlot].item.itemQuantity -= amount;

                if (slots[indexOfSlot].item.itemQuantity == 0)
                {
                    RemoveItemFromSlot(inventorySlot);
                }

                OnInventoryValuesChanged?.Invoke();

                return true;
            }
        }
        return false;
    }

    public bool SubtractItemOfType(Item item, int amount)
    {
        int remainingAmount = amount;

        foreach (var slot in slots)
        {
            if (item != null && slot.item != null && slot.item.itemData == item.itemData)
            {
                if (slot.item.itemQuantity >= remainingAmount)
                {
                    slot.item.itemQuantity -= remainingAmount;

                    if (slot.item.itemQuantity == 0)
                    {

                        RemoveItem(slot.item);
                        slot.item = null;
                    }

                    OnInventoryValuesChanged?.Invoke();

                    return true;
                }
                else
                {
                    remainingAmount -= slot.item.itemQuantity;

                    RemoveItem(slot.item);
                    slot.item = null;

                }
            }
        }

        OnInventoryValuesChanged?.Invoke();
        return remainingAmount == 0;
    }

    public void DropItem(InventorySlot slot, bool dropAll, float dropForce, int amount)
    {
        GameObject itemStack = Instantiate(slot.item.itemData.itemWorldPrefab);
        itemStack.layer = 0;


        itemStack.transform.position = itemPoolParent.transform.position;
        itemStack.transform.rotation = itemPoolParent.transform.rotation;
        Item itemInstance = itemStack.GetComponent<Item>();
        itemInstance.itemData = slot.item.itemData;


        Rigidbody itemRb = itemStack.GetComponent<Rigidbody>();
        Collider itemCol = itemStack.GetComponent<Collider>();

        itemInstance.isInInventory = false;

        itemRb.AddForce(Camera.main.transform.forward * dropForce, ForceMode.Impulse);


        if (dropAll)
        {
            itemInstance.itemQuantity = slot.item.itemQuantity;
            RemoveItemFromSlot(slot);
        }
        else
        {
            itemInstance.itemQuantity = amount;
            SubtractItemFromSlot(slot, amount);
        }

        itemInstance.UpdatePhysics();

        OnInventoryValuesChanged?.Invoke();

    }

}

