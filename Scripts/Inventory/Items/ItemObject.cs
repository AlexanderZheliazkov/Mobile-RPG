using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Head,
    Shouders,
    Body,
    Arms,
    Legs,
    Weapon,
    Special,
    Consumable,
    Defaut
}

public abstract class ItemObject : ScriptableObject
{
    //tova e Scriptable Object, po obrazac na kogoto move da se sazdade item kojti move da se izpolzva ot igracha.

    public Sprite icon;
    public GroundItem prefab;
    public bool stackable;
    public ItemType type;
    [TextArea(3, 10)]
    public string description;
    public Item data = new Item();

    public Item CreateItem()
    {
        Item item = new Item(this);
        return item;
    }

    public abstract void Use(GameObject obj);
}

[System.Serializable]
public class Item
{
    //tova e predmeta koito igracha mozhe da izpolzva i da sahranqva v inventara

    public string Name;
    public int Id;
    public ItemBuff[] buffs;

    public Item()
    {
        Id = -1;
        Name = "";
    }

    public Item(ItemObject item)
    {
        Name = item.name;
        Id = item.data.Id;
        buffs = new ItemBuff[item.data.buffs.Length];
        for (int i = 0; i < buffs.Length; i++)
        {
            buffs[i] = new ItemBuff(item.data.buffs[i].min, item.data.buffs[i].max)
            {
                attribute = item.data.buffs[i].attribute
            };
        }
    }
}