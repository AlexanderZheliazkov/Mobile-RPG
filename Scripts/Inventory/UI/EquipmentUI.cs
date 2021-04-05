using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentUI : ItemContainerUI
{
    public GameObject[] slots = new GameObject[8];

    public override void CreateSlots()
    {
        itemsDisplayed = new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < inventory.GetSlots.Length; i++)
        {
            var temp = slots[i];

            AddEvent(temp, EventTriggerType.PointerEnter, delegate { OnEnter(temp); });
            AddEvent(temp, EventTriggerType.PointerExit, delegate { OnExit(temp); });
            AddEvent(temp, EventTriggerType.BeginDrag, delegate { OnDragStart(temp); });
            AddEvent(temp, EventTriggerType.EndDrag, delegate { OnDragEnd(temp); });
            AddEvent(temp, EventTriggerType.Drag, delegate { OnDrag(temp); });
            AddEvent(temp, EventTriggerType.PointerClick, delegate { OnClick(temp); });

            inventory.GetSlots[i].slotUI = temp;

            itemsDisplayed.Add(temp, inventory.GetSlots[i]);
        }
    }
}