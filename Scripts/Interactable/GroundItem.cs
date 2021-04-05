using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundItem : Interactable
{
    public ItemObject item;

    private void Awake()
    {
        countable = true;
    }

    protected override void Start()
    {
        icon = item.icon;
        name = item.name;
    }

    public override void Interact(GameObject obj)
    {
        if (obj.GetComponent<CharacterInventory>())
        {
            if(obj.GetComponent<CharacterInventory>().inventory.AddItem(item.CreateItem(), amount))
            {
                AplayVFX();
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Your inventory is full!");
            }

        }
    }
}
