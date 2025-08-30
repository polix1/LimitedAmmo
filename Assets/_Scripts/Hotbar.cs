using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Hotbar : MonoBehaviour
{
    private Actions inputActions;

    [Range(5, 100)]
    [SerializeField] int hotbarSize;

    private Inventory playerInventory;

    public int selectedHotbarIndex { get; private set; } = 0;

    private List<InventorySlot> hotbarSlots = new List<InventorySlot>();

    private void Awake()
    {
        inputActions = new();
        playerInventory = GetComponent<Inventory>();
    }

    private void Start()
    {
        SetHotbarSlots();
    }

    private void OnEnable()
    {
        inputActions?.Ui.Enable();
    }

    private void OnDisable()
    {
        inputActions?.Ui.Disable();
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
