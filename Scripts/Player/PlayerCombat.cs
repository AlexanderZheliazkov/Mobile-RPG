using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerStats))]
public class PlayerCombat : MonoBehaviour
{
    [HideInInspector] public WeaponItem weapon;

    private int attackEnergyCost;
    public int jumpEnergyCost;

    public float resAttacksDelay = 10f;
    private float resAttacksTimer;

    private bool inAction;

    private int attackIndex = 0;

    private PlayerAnimation animator;
    private PlayerStats stats;
    private CharacterEquipment equipment;

    void Start()
    {
        stats = GetComponent<PlayerStats>();
        animator = GetComponent<PlayerAnimation>();
        resAttacksTimer = resAttacksDelay;
        attackIndex = 0;

        equipment = GetComponent<CharacterEquipment>();
        equipment.onWeaponChanged += SetWeapon;
    }

    protected void Update()
    {
        if (resAttacksTimer > 0)
        {
            resAttacksTimer -= Time.deltaTime;
        }
        else
        {
            attackIndex = 0;
        }
    }

    public void Attack()
    {
        if (inAction) return;
        if (weapon == null) return;
        if (weapon.attacks.Count <= 0) return;

        if (attackIndex >= weapon.attacks.Count || attackIndex < 0)
            attackIndex = 0;

        attackEnergyCost = weapon.attacks[attackIndex].energyCost;
        if (!stats.UseEnergy(attackEnergyCost)) return;

        animator.OnAttack(weapon.attacks[attackIndex].attackAnimation);
        attackIndex++;

        resAttacksTimer = resAttacksDelay;
    }

    public void Jump()
    {
        if (inAction) return;
        if (!stats.UseEnergy(jumpEnergyCost)) return;

        animator.Jump();
    }

    public void InAction()
    {
        inAction = true;
    }

    public void NotInAction()
    {
        inAction = false;
    }

    public void SetWeapon(WeaponItem _weapon)
    {
        weapon = _weapon;
    }

    public void SpawnAttackProjectile()
    {
        //if (weapon == null) return;
        //if (weapon.attacks.Count <= 0) return;

        if (attackIndex >= weapon.attacks.Count || attackIndex < 0)
            attackIndex = 0;

        weapon.attacks[attackIndex].InstantiateAttack(gameObject.transform);
    }
}
