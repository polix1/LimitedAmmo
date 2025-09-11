using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class InventoryUi : MonoBehaviour
{
    public static InventoryUi Instance;

    [SerializeField] Inventory playerInventory;
    [SerializeField] Transform uiSlotHolder;
    [SerializeField] GameObject uiSlot;
    public Transform moverTransform;
    public Image moverImage;
    public TMP_Text moverCount;

    Vector2 mousePosition;


    public List<UiSlot> uiSlots = new List<UiSlot>();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }

        for (int i = 0; i < playerInventory.totalSlots; i++)
        {
            GameObject uiSlotInstance = Instantiate(uiSlot, uiSlotHolder);
            uiSlots.Add(uiSlotInstance.GetComponent<UiSlot>());
            uiSlots[i].GetComponent<UiSlot>().slotType = SlotType.MainInventory;
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

    private void Update()
    {
        if(Mouse.current != null)
        {
            mousePosition = Mouse.current.position.ReadValue();
        }
    }

    private void LateUpdate()
    {
        moverTransform.GetComponent<RectTransform>().position = mousePosition;
    }
}
