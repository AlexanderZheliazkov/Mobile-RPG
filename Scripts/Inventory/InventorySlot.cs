using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public ItemType[] AllowedItems = new ItemType[0];
    [System.NonSerialized] public ItemContainerUI parentUI;
    [System.NonSerialized] public GameObject slotUI;
    [System.NonSerialized] public SlotUpdated OnAfterUpdate;
    [System.NonSerialized] public SlotUpdated OnBeforeUpdate;
    public int amount;
    public Item item = new Item();

    public InventorySlot()
    {
        UpdateSlot(new Item(), 0);
    }
    public InventorySlot(Item _item, int _amount)
    {
        UpdateSlot(_item, _amount);
    }
    public void UpdateSlot(Item _item, int _amount)
    {
        if (OnBeforeUpdate != null)
            OnBeforeUpdate.Invoke(this);

        if (_item == null)
        {
            item = new Item();
            amount = 0;
            return;
        }

        item = _item;
        amount = _amount;

        if (OnAfterUpdate != null)
            OnAfterUpdate.Invoke(this);
    }
    public void AddAmount(int value)
    {
        UpdateSlot(item, amount + value);
    }

    public void RemoveItem()
    {
        UpdateSlot(new Item(), 0);
    }

    public ItemObject GetItemObject()
    {
        return item.Id >= 0 ? parentUI.inventory.database.ItemObjects[item.Id] : null;
    }

    public void UseItem()
    {
        if (amount <= 0 || item.Id < 0) return;
        parentUI.inventory.database.ItemObjects[item.Id].Use(parentUI.player);
        amount--;
        if (amount <= 0)
        {
            this.UpdateSlot(new Item(), 0);
        }
    }

    public bool CanPlaceInSlot(ItemObject _itemObject)
    {
        if (AllowedItems.Length <= 0 || _itemObject == null || _itemObject.data.Id < 0)
            return true;
        for (int i = 0; i < AllowedItems.Length; i++)
        {
            if (_itemObject.type == AllowedItems[i])
                return true;
        }
        return false;
    }

    public ItemObject ItemObject
    {
        get
        {
            if (item.Id >= 0)
            {
                return parentUI.inventory.database.ItemObjects[item.Id];
            }
            return null;
        }
    }
}

public delegate void SlotUpdated(InventorySlot _slot);