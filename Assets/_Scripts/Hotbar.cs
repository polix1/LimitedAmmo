using System;
using System.Collections.Generic;
using UnityEngine;


public class Hotbar : MonoBehaviour
{
    private Actions inputActions;

    [Range(5, 100)]
    public int hotbarSize;

    private Inventory playerInventory;

    public int selectedHotbarIndex { get; private set; } = 0;

    public List<InventorySlot> hotbarSlots { get; private set; } = new List<InventorySlot>();

    public event Action OnInventoryValuesUpdatedPass;
    public event Action OnHotbarReady;

    private void Awake()
    {
        inputActions = new();
        playerInventory = GetComponent<Inventory>();
    }

    private void Start()
    {
        SetHotbarSlots();
        OnHotbarReady?.Invoke();
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
            if(selectedHotbarIndex++ >= hotbarSize) 
            {
                selectedHotbarIndex = 0;
            }
        }
        else if(scrollWheelYAxis < 0)
        {
            if (selectedHotbarIndex-- <= 0)
            {
                selectedHotbarIndex = hotbarSize;
            }
        }
    }

    private void OnMainInventoryValuesChanged()
    {
        OnInventoryValuesUpdatedPass?.Invoke();
    }

    public InventorySlot GetSelectedSlot()
    {
        return playerInventory.slots[selectedHotbarIndex];
    }

    public void SetHotbarSlots()
    {
        for (int i = playerInventory.slots.Count - hotbarSize; i < playerInventory.slots.Count; i++)
        {
            hotbarSlots.Add(playerInventory.slots[i]);
        }
    }
}
