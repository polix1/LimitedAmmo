using UnityEngine;

[CreateAssetMenu]
public class ItemSO : ScriptableObject
{
    public string itemName;
    [Space]

    public Sprite itemIcon;
    [Space]

    public int itemMaxStack = 1;
    [Space]

    public ItemType itemType;
    [Space]

    public GameObject itemWorldPrefab;

}
