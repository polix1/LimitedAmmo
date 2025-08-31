using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiSlot : MonoBehaviour
{
    public Image icon;
    public TMP_Text quanitytText;
    public GameObject slotHighlight;

    public void UpdateSlotUi(ItemSO itemData, int quantity)
    {
        if (itemData != null)
        {
            icon.gameObject.SetActive(true);
            quanitytText.gameObject.SetActive(true);
            icon.sprite = itemData.itemIcon;
            quanitytText.text = quantity.ToString();
        }
        else if(itemData == null)
        {
            icon.gameObject.SetActive(false);
            quanitytText.gameObject.SetActive(false);
        }
    }
}
