using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
public class DropLootOnDeath : DropLoot
{
    private CharacterStats stats;

    private void Start()
    {
        stats = GetComponent<CharacterStats>();
        stats.healthBelowZero += SpawnForAllItems;
    }
}
