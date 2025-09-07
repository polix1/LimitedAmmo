using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemPool : MonoBehaviour
{
    Dictionary<Item, GameObject> pool = new Dictionary<Item, GameObject>();

    public void AddItem(Item KeyItem, GameObject ValueGameObject, Transform poolParent)
    {
        if (!pool.ContainsKey(KeyItem))
        {
            pool.Add(KeyItem, ValueGameObject);

            GameObject itemStack = Instantiate(KeyItem.itemData.itemWorldPrefab, poolParent);

            Item itemInstance = itemStack.GetComponent<Item>();

            itemInstance.enabled = false;
        }
    }

    public void RemoveItem(Item KeyItem)
    {
        if(pool.TryGetValue(KeyItem, out GameObject itemGameObject))
        {
            Destroy(itemGameObject);
            pool.Remove(KeyItem);
        }
    }

    public GameObject GetItem(Item item)
    {
        if (item == null) return null;

        if (pool.ContainsKey(item))
        {
            return pool[item];
        }
        return null;
    }
}
