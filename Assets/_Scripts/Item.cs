using UnityEngine;

public class Item : MonoBehaviour, IItemInteractable
{
    public ItemSO itemData;
    public int itemQuantity;


    public void OnItemInteract(Inventory playerInventory)
    {
        playerInventory.AddItem(itemData, itemQuantity);
    }

    public void PickUp(Inventory inventory)
    {
        inventory.AddItem(itemData, itemQuantity);
        Destroy(gameObject);
    }
}
