using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
public class NPCCombat : MonoBehaviour
{
    public List<AttackObject> attacks = new List<AttackObject>();

    public float attackDelay = 2f;
    private float resAttackTimer;

    private int attackIndex;

    private NPCAnimacion animator;
    private NPCMovement movement;
    public bool inAction;
    public bool canAttack;

    protected CharacterStats stats;

    void Start()
    {
        stats = GetComponent<CharacterStats>();

        if (GetComponent<NPCMovement>())
        {
            movement = GetComponent<NPCMovement>();
        }
        if (GetComponent<NPCAnimacion>())
        {
            animator = GetComponent<NPCAnimacion>();
        }

        stats.healthBelowZero += Kill;
        stats.onBeingHit += animator.BeingHit;

        attackIndex = 0;
    }

    void Update()
    {
        if (resAttackTimer > 0)
        {
            resAttackTimer -= Time.deltaTime;
            canAttack = false;
        }
        else
        {
            canAttack = true;
        }
    }

    public void Attack()
    {
        if (inAction || !canAttack) return;
        if (attacks == null) return;
        if (attacks.Count <= 0) return;

        if (attackIndex >= attacks.Count || attackIndex < 0)
            attackIndex = 0;

        //attackEnergyCost = weapon.attacks[attackIndex].energyCost;
        //if (!stats.UseEnergy(attackEnergyCost)) return;

        animator.OnAttack(attacks[attackIndex].attackAnimation);
        attackIndex++;

        resAttackTimer = attackDelay;
    }

    public void InAction()
    {
        inAction = true;
    }

    public void NotInAction()
    {
        inAction = false;
    }

    public void SpawnAttackProjectile()
    {
        //if (weapon == null) return;
        //if (weapon.attacks.Count <= 0) return;

        if (attackIndex >= attacks.Count || attackIndex < 0)
            attackIndex = 0;

        attacks[attackIndex].InstantiateAttack(gameObject.transform);
    }

    public void Kill()
    {
        animator.Killed();
        stats.enabled = false;
        animator.enabled = false;
        movement.enabled = false;
        InAction();
    }

    public void Die()
    {
        Destroy(gameObject);
    }

}
