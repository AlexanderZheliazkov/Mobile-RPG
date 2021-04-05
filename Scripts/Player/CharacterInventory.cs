using DitzeGames.MobileJoystick;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInventory : MonoBehaviour
{
    public ItemContainerObject inventory;

    void Start()
    {
        //inventory.Load();
    }

    void Update()
    {
        
    }

    private void OnApplicationQuit()
    {
        inventory.Clear();
    }
}
