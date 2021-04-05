using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class ItemContainerUI : MonoBehaviour
{
    public ItemContainerObject inventory;
    public GameObject slotsHolder;

    public GameObject player;

    protected UIManager uiManager;

    protected Dictionary<GameObject, InventorySlot> itemsDisplayed = new Dictionary<GameObject, InventorySlot>();

    public OnClickItem onClickItem;

    void Start()
    {
        for (int i = 0; i < inventory.GetSlots.Length; i++)
        {
            inventory.GetSlots[i].parentUI = this;
            inventory.GetSlots[i].OnAfterUpdate += OnSlotUpdate;
        }
        CreateSlots();

        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        UpdateDiplay();
    }

    void Update()
    {

    }

    public void UpdateDiplay()
    {
        foreach (KeyValuePair<GameObject, InventorySlot> _slot in itemsDisplayed)
        {
            if (_slot.Value.item.Id >= 0)
            {
                _slot.Key.transform.GetChild(1).GetComponentInChildren<Image>().sprite = inventory.database.ItemObjects[_slot.Value.item.Id].icon;
                _slot.Key.transform.GetChild(1).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = _slot.Value.amount == 1 ? "" : _slot.Value.amount.ToString("n0");
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().gameObject.SetActive(false);
            }
            else
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().gameObject.SetActive(true);
                _slot.Key.transform.GetChild(1).GetComponentInChildren<Image>().sprite = null;
                _slot.Key.transform.GetChild(1).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }
    }

    public abstract void CreateSlots();

    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnEnter(GameObject obj)
    {
        MouseData.slotHoveredOver = obj;
        MouseData.interfaceMouseIsOver = itemsDisplayed[obj].parentUI;
    }
    public void OnExit(GameObject obj)
    {
        MouseData.slotHoveredOver = null;
        MouseData.interfaceMouseIsOver = null;
    }
    public void OnDragStart(GameObject obj)
    {
        MouseData.tempItemBeingDragged = CreateTempItem(obj);
    }
    public GameObject CreateTempItem(GameObject obj)
    {
        GameObject tempItem = null;
        if (itemsDisplayed[obj].item.Id >= 0)
        {
            tempItem = new GameObject();
            var rt = tempItem.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(50, 50);
            tempItem.transform.SetParent(transform.parent);
            var img = tempItem.AddComponent<Image>();
            img.sprite = itemsDisplayed[obj].GetItemObject().icon;
            img.raycastTarget = false;
        }

        return tempItem;
    }
    public void OnDragEnd(GameObject obj)
    {
        Destroy(MouseData.tempItemBeingDragged);
        if (MouseData.slotHoveredOver)
        {
            InventorySlot mouseHoverSlotData = MouseData.interfaceMouseIsOver.itemsDisplayed[MouseData.slotHoveredOver];
            ItemContainerObject.SwapItems(itemsDisplayed[obj], mouseHoverSlotData);
        }
    }
    public void OnDrag(GameObject obj)
    {
        if (MouseData.tempItemBeingDragged != null)
            MouseData.tempItemBeingDragged.GetComponent<RectTransform>().position = Input.mousePosition;
    }

    public void OnClick(GameObject obj)
    {
        if (onClickItem != null)
            onClickItem.Invoke(itemsDisplayed[obj]);

        Debug.Log(obj.name + " was clicked!");
    }

    private void OnSlotUpdate(InventorySlot _slot)
    {
        if (_slot.slotUI == null) return;
        if (_slot.item.Id >= 0)
        {
            _slot.slotUI.transform.GetChild(1).GetComponentInChildren<Image>().sprite = inventory.database.ItemObjects[_slot.item.Id].icon;
            _slot.slotUI.transform.GetChild(1).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
            _slot.slotUI.GetComponentInChildren<TextMeshProUGUI>().text = _slot.amount == 1 ? "" : _slot.amount.ToString("n0");
            _slot.slotUI.transform.GetChild(0).GetComponentInChildren<Image>().gameObject.SetActive(false);
        }
        else
        {
            _slot.slotUI.transform.GetChild(0).GetComponentInChildren<Image>().gameObject.SetActive(true);
            _slot.slotUI.transform.GetChild(1).GetComponentInChildren<Image>().sprite = null;
            _slot.slotUI.transform.GetChild(1).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
            _slot.slotUI.GetComponentInChildren<TextMeshProUGUI>().text = "";
        }
    }
}

public static class MouseData
{
    public static ItemContainerUI interfaceMouseIsOver;
    public static GameObject tempItemBeingDragged;
    public static GameObject slotHoveredOver;
}


public delegate void OnClickItem(InventorySlot _slot);