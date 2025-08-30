using UnityEngine;

public interface IInteractable
{
    public void OnInteract();
}

public interface IItemInteractable
{
    public void OnItemInteract(Inventory playerInventory);
}
