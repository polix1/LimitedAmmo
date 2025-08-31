using System;
using System.Collections.Generic;
using UnityEngine;


public class Hotbar : MonoBehaviour
{
    private Actions inputActions;

    [Range(4, 100)]
    public int hotbarSizeMaxIndex;

    private Inventory playerInventory;

    public int selectedHotbarIndex { get; private set; } = 1;

    public List<InventorySlot> hotbarSlots { get; private set; } = new List<InventorySlot>();

    public event Action<int> OnInventoryValuesUpdatedPass;
    public event Action<int> OnHotbarReady;

    private void Awake()
    {
        inputActions = new();
        playerInventory = GetComponent<Inventory>();
    }

    private void Start()
    {
        SetHotbarSlots();
        OnHotbarReady?.Invoke(selectedHotbarIndex);
    }

    private void OnEnable()
    {
        inputActions?.Ui.Enable();

        if (playerInventory != null)
            playerInventory.OnInventoryValuesChanged += OnMainInventoryValuesChanged;
    }

    private void OnDisable()
    {
        inputActions?.Ui.Disable();

        if (playerInventory != null)
            playerInventory.OnInventoryValuesChanged -= OnMainInventoryValuesChanged;
    }

    private void Update()
    {
        float scrollWheelYAxis = inputActions.Ui.ScrollWheel.ReadValue<float>();
        
        if(scrollWheelYAxis > 0)
        {          
            if(selectedHotbarIndex++ >= hotbarSizeMaxIndex) 
            {
                selectedHotbarIndex = 1;
            }
        }
        else if(scrollWheelYAxis < 0)
        {
            if (selectedHotbarIndex-- <= 1)
            {
                selectedHotbarIndex = hotbarSizeMaxIndex;
            }
        }
        OnInventoryValuesUpdatedPass?.Invoke(selectedHotbarIndex-1);
    }

    private void OnMainInventoryValuesChanged()
    {
        OnInventoryValuesUpdatedPass?.Invoke(selectedHotbarIndex-1);
    }

    public InventorySlot GetSelectedSlot()
    {
        return playerInventory.slots[selectedHotbarIndex];
    }

    public void SetHotbarSlots()
    {
        for (int i = playerInventory.slots.Count - hotbarSizeMaxIndex; i < playerInventory.slots.Count; i++)
        {
            hotbarSlots.Add(playerInventory.slots[i]);
        }
    }
}
