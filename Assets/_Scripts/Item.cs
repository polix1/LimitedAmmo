using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemSO itemData;
    public int itemQuantity;

    public void PickUp(Inventory inventory)
    {
        inventory.AddItem(itemData, itemQuantity);
        Destroy(gameObject);
    }
}
