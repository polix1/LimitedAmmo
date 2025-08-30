using UnityEngine;

public class Item : MonoBehaviour, IItemInteractable
{
    public ItemSO itemData;
    public int itemQuantity;


    public void OnItemInteract(Inventory playerInventory)
    {
        PickUp(playerInventory);
    }

    public void PickUp(Inventory inventory)
    {
        inventory.AddItem(this);

        if (itemQuantity <= 0) 
        {
            Destroy(gameObject);
        }

    }
}
