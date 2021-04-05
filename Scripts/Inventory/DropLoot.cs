using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropLoot : MonoBehaviour
{
    public Transform spawnItemPoint;
    public ItemToDrop[] dropItems;

    private void Start()
    {
        if (spawnItemPoint == null)
            spawnItemPoint = transform;
    }

    public void SpawnItem(ItemToDrop _dropItem)
    {
        if (_dropItem.items.Length <= 0) return;
        int randomItemIndex = Random.Range(0, _dropItem.items.Length);

        float _chanse = Random.Range(0, 100);
        if (_chanse <= _dropItem.dropChancePercent)
        {
            ItemObject _item = _dropItem.items[randomItemIndex];
            GroundItem gi = Instantiate(_item.prefab, spawnItemPoint.position, spawnItemPoint.rotation);
            gi.item = _item;
            if (_item.stackable)
                gi.amount = Random.Range((int)_dropItem.count.x, (int)_dropItem.count.y);
            else
                gi.amount = 1;
        }
    }

    public void SpawnForAllItems()
    {
        for (int i = 0; i < dropItems.Length; i++)
        {
            SpawnItem(dropItems[i]);
        }
    }
}

[System.Serializable]
public struct ItemToDrop
{
    public ItemObject[] items;
    public Vector2 count;
    public float dropChancePercent;
}
