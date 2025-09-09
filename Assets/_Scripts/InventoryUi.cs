using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class InventoryUi : MonoBehaviour
{
    [SerializeField] Inventory playerInventory;
    [SerializeField] Transform uiSlotHolder;
    [SerializeField] GameObject uiSlot;


    public List<UiSlot> uiSlots = new List<UiSlot>();

    private void Awake()
    {
        for (int i = 0; i < playerInventory.totalSlots; i++)
        {
            GameObject uiSlotInstance = Instantiate(uiSlot, uiSlotHolder);
            uiSlots.Add(uiSlotInstance.GetComponent<UiSlot>());
        }
    } 

    private void Start()
    {
        for (int i = 0; i < playerInventory.slots.Count; i++)
        {
            uiSlots[i].InventorySlot = playerInventory.slots[i];
        }
        UpdateUi();
    }

    private void OnEnable()
    {
        if (playerInventory != null)
            playerInventory.OnInventoryValuesChanged += UpdateUi;
    }

    private void OnDisable()
    {
        if (playerInventory != null)
            playerInventory.OnInventoryValuesChanged -= UpdateUi;
    }

    private void UpdateUi()
    {
        for (int i = 0; i < playerInventory.slots.Count; i++)
        {
            if (playerInventory.slots[i].item != null)
            {
                UiSlot slot = uiSlots[i];

                slot.UpdateSlotUi(playerInventory.slots[i].item.itemData, playerInventory.slots[i].item.itemQuantity);
            }
            else
            {
                UiSlot slot = uiSlots[i];

                slot.UpdateSlotUi(null, 0);
            }
        }
    }
}
