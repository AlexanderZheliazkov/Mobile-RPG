using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DitzeGames.MobileJoystick;

public class UIManager : MonoBehaviour
{
    public GameObject player;

    [Header("Inventory")]
    public GameObject inventoryUIObject;
    public Button openInventoryBtn;
    public Button closeInventoryBtn;
    private ItemContainerUI[] inventoryUI;

    [Header("GameplayUI")]
    public GameObject gameplayUI;

    [Header("SelectedItem")]
    public SelectedItemUI selectedItemUI;

    void Start()
    {
        inventoryUIObject.SetActive(false);
        inventoryUI = inventoryUIObject.GetComponentsInChildren<ItemContainerUI>();
        openInventoryBtn.onButtonDown += OpenInventoryBtnPress;
        closeInventoryBtn.onButtonDown += CloseInventoryBtnPress;
        
        for (int i = 0; i < inventoryUI.Length; i++)
        {
            inventoryUI[i].onClickItem += UpdateSelectedItem;
        }

        selectedItemUI.gameObject.SetActive(false);
        gameplayUI.gameObject.SetActive(true);
    }
    
    void Update()
    {

    }

    void OpenInventoryBtnPress()
    {
        gameplayUI.SetActive(false);
        inventoryUIObject.SetActive(true);
        selectedItemUI.gameObject.SetActive(false);
    }

    void CloseInventoryBtnPress()
    {
        gameplayUI.SetActive(true);
        inventoryUIObject.SetActive(false);
    }

    public void UpdateSelectedItem(InventorySlot _slot)
    {
        if (_slot.item.Id < 0) return;

        if(selectedItemUI.gameObject.active == false)
        {
            selectedItemUI.gameObject.SetActive(true);
        }
        selectedItemUI.UpdateSelectedItem(_slot);
    }
}
