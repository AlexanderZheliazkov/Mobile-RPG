using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "InventorySystem/Inventory")]
public class ItemContainerObject : ScriptableObject
{
    [Header("Data")]
    public string savePath;
    public ItemDatabaseObject database;
    [Header("Others")]
    public int maxSpace;
    public Inventory container;
    public InventorySlot[] GetSlots { get { return container.slots; } }

    public bool AddItem(Item _item, int _amount)
    {
        if (_item == null) return false;

        if (database.ItemObjects[_item.Id].stackable)
        {
            for (int i = 0; i < GetSlots.Length; i++)
            {
                if (_item.Id == GetSlots[i].item.Id)
                {
                    GetSlots[i].AddAmount(_amount);
                    return true;
                }
            }
        }

        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].item.Id == -1)
            {
                GetSlots[i].UpdateSlot(_item, _amount);
                return true;
            }
        }

        return false;
    }

    public bool RemoveItem(Item _item, int _amount)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (_item == GetSlots[i].item)
            {
                if (GetSlots[i].amount >= _amount)
                {
                    if (GetSlots[i].amount - _amount <= 0)
                    {
                        GetSlots[i].UpdateSlot(new Item(), 0);
                    }
                    else
                    {
                        GetSlots[i].AddAmount(-_amount);
                    }
                    return true;
                }
                else
                {
                    Debug.Log("You want to remove " + _amount + " " + _item.Name
                        + "from inventory, but you have only " + GetSlots[i].amount + ".");
                    return false;
                }
            }
        }
        Debug.Log("You want to remove " + _amount + " " + _item.Name
                        + "from inventory, but you do not have " + _item.Name + ".");
        return false;
    }

    public static void SwapItems(InventorySlot item1, InventorySlot item2)
    {
        if (item1.item.Id < 0) return;
        if (!(item2.CanPlaceInSlot(item1.ItemObject) && item1.CanPlaceInSlot(item2.ItemObject)))
        {
            return;
        }

        //if items are the same and are stackable
        if (item1.item.Id == item2.item.Id && item1.parentUI.inventory.database.ItemObjects[item1.item.Id].stackable && item2.parentUI.inventory.database.ItemObjects[item2.item.Id].stackable)
        {
            item2.UpdateSlot(item2.item, item2.amount + item1.amount);
            item1.RemoveItem();
            return;
        }
        InventorySlot temp = new InventorySlot(item2.item, item2.amount);
        item2.UpdateSlot(item1.item, item1.amount);
        item1.UpdateSlot(temp.item, temp.amount);

    }

    [ContextMenu("Save")]
    public void Save()
    {
        //string saveData = JsonUtility.ToJson(this, true);
        //BinaryFormatter bf = new BinaryFormatter();
        //FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        //bf.Serialize(file, saveData);
        //file.Close();

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, container);
        stream.Close();
    }
    [ContextMenu("Load")]
    public void Load()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            //BinaryFormatter bf = new BinaryFormatter();
            //FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            //JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            //file.Close();

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            Inventory newContainer = (Inventory)formatter.Deserialize(stream);
            for (int i = 0; i < GetSlots.Length; i++)
            {
                GetSlots[i].UpdateSlot(newContainer.slots[i].item, newContainer.slots[i].amount);
            }
            stream.Close();
        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        container.Clear();
    }

    [ContextMenu("Res")]
    public void Reset()
    {
        container.slots = new InventorySlot[maxSpace];
    }
}

[System.Serializable]
public class Inventory
{
    public InventorySlot[] slots;
    public Inventory(int maxSpace)
    {
        slots = new InventorySlot[maxSpace];
    }
    public void Clear()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].RemoveItem();
        }
    }
}