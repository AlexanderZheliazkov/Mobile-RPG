using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponTyper
{
    TwoHanded,
    SwordAndShield,
    DualSwords,
    Ranged
}

[CreateAssetMenu(fileName = "New Weapon Item", menuName = "InventorySystem/Items/Equipment/Weapon")]
public class WeaponItem : ItemObject
{
    [Header("Animations")]
    [SerializeField] public List<AttackObject> attacks;
    public AnimationClip idleAnim;
    public AnimationClip runAnim;

    public WeaponPrefab leftHandPrefab;
    public WeaponPrefab rightHandPrefab;

    public override void Use(GameObject obj)
    {

    }
}
