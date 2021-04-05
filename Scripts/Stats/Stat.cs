using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Attributes
{
    Health,
    Energy,
    Damage,
    Armor,
    Vamp
}

[System.Serializable]
public class Stat
{
    [SerializeField]
    private int baseValue;

    public Attributes atribute;
    public Dictionary<Item, int> modifiers = new Dictionary<Item, int>();

    public Stat()
    {
        baseValue = 0;
        modifiers = new Dictionary<Item, int>();
    }

    public Stat(int baseValue)
    {
        this.baseValue = baseValue;
        modifiers = new Dictionary<Item, int>();
    }

    public int GetValue()
    {
        int finalValue = baseValue;
        foreach (KeyValuePair<Item, int> mod in modifiers)
        {
            finalValue += mod.Value;
        }
        return finalValue;
    }

    public void SetBaseValue(int _baseValue)
    {
        baseValue = _baseValue;
    }

    public void AddModifier(Item item, int modifier)
    {
        modifiers.Add(item, modifier);
    }

    public void RemoveModifier(Item item, int modifier)
    {
        modifiers.Remove(item);
    }
}
