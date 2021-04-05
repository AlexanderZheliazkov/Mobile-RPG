using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment Item", menuName = "InventorySystem/Items/Equipment/BaseEquipment")]
public class EquipmentItem : ItemObject
{
    public SkinnedMeshRenderer renderer;

    public override void Use(GameObject obj)
    {

    }
}