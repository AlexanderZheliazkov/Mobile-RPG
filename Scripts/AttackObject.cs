using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "Attacks/Attack")]
public class AttackObject : ScriptableObject
{
    public AnimationClip attackAnimation;

    public GameObject attackVFX;

    public AttackProjectile projectile;
    public LayerMask targetMask;
    public float projectileLivingTime;
    public GameObject projectileDieVFX;

    [Min(0)] public float damageModifier;
    [Min(0)] public int energyCost;

    public void InstantiateAttack(Transform obj)
    {
        if (obj == null) return;
        if (projectile == null) return;

        AttackProjectile attack = Instantiate(projectile, obj.position, obj.rotation);
        CharacterStats stats = obj.GetComponent<CharacterStats>();
        if (stats != null)
        {
            attack.myStats = stats;
            attack.targetMask = targetMask;

        }
        attack.livingTime = projectileLivingTime;
        attack.dieVFX = projectileDieVFX;

        if(attackVFX!= null)
        {
            GameObject vfx = Instantiate(attackVFX, obj.position, obj.rotation);
            Destroy(vfx, 5);
        }
    }
}
