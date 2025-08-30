using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class InventoryUi : MonoBehaviour
{
    [SerializeField] Inventory playerInventory;
    [SerializeField] Transform uiSlotHolder;
    [SerializeField] GameObject uiSlot;


    private List<Transform> uiSlots = new List<Transform>();

    private void Awake()
    {
        for (int i = 0; i < playerInventory.totalSlots; i++)
        {
            GameObject uiSlotInstance = Instantiate(uiSlot, uiSlotHolder);
            uiSlots.Add(uiSlotInstance.transform);
        }
        if (playerInventory != null)
            playerInventory.OnInventoryValuesChanged -= UpdateUI;

        if (playerInventory != null)
            playerInventory.OnInventoryValuesChanged += UpdateUI;

        UpdateUI();
    }

    private void OnEnable()
    {
        if (playerInventory != null)
            playerInventory.OnInventoryValuesChanged += UpdateUI;
    }

    private void OnDisable()
    {
        if (playerInventory != null)
            playerInventory.OnInventoryValuesChanged -= UpdateUI;
    }

    private void UpdateUI()
    {
        for (int i = 0; i < playerInventory.slots.Count; i++)
        {
            if (playerInventory.slots[i].item != null)
            {
                SetUiSlot(i);
            }
            else
            {
                EmptyUiSlot(i);
            }
        }
    }

    private void SetUiSlot(int slotIndex) 
    {
        Transform slot = uiSlots[slotIndex];
        slot.GetChild(3).gameObject.SetActive(true);
        slot.GetChild(1).gameObject.SetActive(true);
        slot.GetComponentInChildren<TMP_Text>().text = playerInventory.slots[slotIndex].quantity.ToString();
        slot.GetChild(1).GetComponent<Image>().sprite = playerInventory.slots[slotIndex].item.itemIcon;
    }

    private void EmptyUiSlot(int slotIndex)
    {
        Transform slot = uiSlots[slotIndex];

        slot.GetChild(3).gameObject.SetActive(false);
        slot.GetChild(1).gameObject.SetActive(false);
    }
}
