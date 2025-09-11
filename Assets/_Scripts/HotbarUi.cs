using UnityEngine;
using System.Collections.Generic;
using System;

public class HotbarUi : MonoBehaviour
{
    [SerializeField] Hotbar playerHotbar;
    [SerializeField] Transform uiSlotHolder;
    [SerializeField] GameObject uiSlot;

    private List<UiSlot> uiSlots = new List<UiSlot>();

    private void Awake()
    {
        for (int i = 0; i < playerHotbar.hotbarSizeMaxIndex; i++)
        {
            GameObject uiSlotInstance = Instantiate(uiSlot, uiSlotHolder);
            uiSlots.Add(uiSlotInstance.GetComponent<UiSlot>());
            uiSlots[i].GetComponent<UiSlot>().slotType = SlotType.Hotbar;
        }
    }
    private void Start()
    {
    }

    private void OnEnable()
    {
        if (playerHotbar != null)
        {
            playerHotbar.OnInventoryValuesUpdatedPass += UpdateUi;
            playerHotbar.OnHotbarReady += UpdateUi;
        }
    }

    private void OnDisable()
    {
        if (playerHotbar != null)
        {
            playerHotbar.OnInventoryValuesUpdatedPass -= UpdateUi;
            playerHotbar.OnHotbarReady -= UpdateUi;
        }
    }

    private void UpdateUi(int selectedIndex)
    {
        for (int i = 0; i < playerHotbar.hotbarSlots.Count; i++)
        {
            uiSlots[i].isSelected = false;
            uiSlots[selectedIndex].isSelected = true;
            if (playerHotbar.hotbarSlots[i].item != null)
            {
                UiSlot slot = uiSlots[i];

                slot.UpdateSlotUi(playerHotbar.hotbarSlots[i].item.itemData, playerHotbar.hotbarSlots[i].item.itemQuantity);
            }
            else
            {
                UiSlot slot = uiSlots[i];

                slot.UpdateSlotUi(null, 0);
            }
        }
    }
}
