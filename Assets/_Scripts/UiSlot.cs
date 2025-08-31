using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiSlot : MonoBehaviour
{
    public Image icon;
    public TMP_Text quanitytText;
    public GameObject slotSelectHighlight;
    public bool isSelected = false;

    public void UpdateSlotUi(ItemSO itemData, int quantity)
    {

        if (isSelected)
        {
            slotSelectHighlight.SetActive(true);
        }
        else
        {
            slotSelectHighlight.SetActive(false);
        }
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
