using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class SelectedItemUI : MonoBehaviour
{
    public InventorySlot selectedSlot;

    public Image itemIcon;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;
    //public TextMeshProUGUI itemAtributes;

    public Button closeBtn;
    public Button deleteItem;

    public void UpdateSelectedItem(InventorySlot _slot)
    {
        if (_slot == null || _slot.item.Id < 0) return;

        selectedSlot = _slot;

        ItemObject item = _slot.parentUI.inventory.database.ItemObjects[_slot.item.Id];
        itemIcon.sprite = item.icon;
        itemName.text = _slot.item.Name;
        itemDescription.text = item.description;
        itemDescription.text += "\n";
        for (int i = 0; i < _slot.item.buffs.Length; i++)
        {
            itemDescription.text += _slot.item.buffs[i].attribute.ToString() + ": " + _slot.item.buffs[i].value + "\n";
        }
    }

    public void CloseSelectedItemUI()
    {
        gameObject.SetActive(false);
    }

    public void DeleteItem()
    {
        selectedSlot.parentUI.inventory.RemoveItem(selectedSlot.item, selectedSlot.amount);
        CloseSelectedItemUI();
    }
}
