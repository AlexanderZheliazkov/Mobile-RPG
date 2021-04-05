using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryUI : ItemContainerUI
{
    public GameObject slotPrefab;

    [Header("Tiles")]
    public int X_START;
    public int Y_START;
    public int X_SPACE_BETWEEN_ITEM;
    public int NUMBER_OF_COLUMN;
    public int Y_SPACE_BETWEEN_ITEMS;

    public override void CreateSlots()
    {
        itemsDisplayed = new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < inventory.GetSlots.Length; i++)
        {
            var temp = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity, slotsHolder.transform);
            temp.GetComponent<RectTransform>().localPosition = GetPosition(i);

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

    public Vector3 GetPosition(int i)
    {
        return new Vector3(X_START + (X_SPACE_BETWEEN_ITEM * (i % NUMBER_OF_COLUMN)), Y_START + (-Y_SPACE_BETWEEN_ITEMS * (i / NUMBER_OF_COLUMN)), 0f);
    }
}
