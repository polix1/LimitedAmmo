using UnityEngine;

[RequireComponent (typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Item : MonoBehaviour, IItemInteractable
{
    public ItemSO itemData;
    public int itemQuantity;

    public bool isInInventory;

    private Rigidbody itemRb;
    private Collider itemCollider;


    private void Awake()
    {
        itemRb = GetComponent<Rigidbody>();

        itemCollider = GetComponent<Collider>();
    }

    public void OnItemInteract(Inventory playerInventory)
    {
        PickUp(playerInventory);
    }

    public void PickUp(Inventory inventory)
    {
        inventory.AddItem(this);

        if(itemQuantity <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void UpdatePhysics()
    {
        if(isInInventory)
        {
            itemRb.isKinematic = true;
            itemCollider.isTrigger = true;
        }
        else
        {
            itemRb.isKinematic = false;
            itemCollider.isTrigger = false;
        }
    }
}
