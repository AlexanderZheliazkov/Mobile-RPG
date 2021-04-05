using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Food Item", menuName = "InventorySystem/Items/Food")]
public class FoodItem : ItemObject
{
    public int restoreHealthValue;
    
    public override void Use(GameObject obj)
    {

    }
}
