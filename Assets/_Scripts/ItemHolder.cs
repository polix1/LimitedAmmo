using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    [SerializeField] Transform itemHolderPosition;
    private void LateUpdate()
    {
        transform.position = itemHolderPosition.position;
    }
}
