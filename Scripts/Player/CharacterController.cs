using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DitzeGames.MobileJoystick;

[RequireComponent(typeof(CharacterMovement), typeof(PlayerCombat), typeof(PlayerAnimation))]
public class CharacterController : MonoBehaviour
{
    #region Input

    [Header("Input")]
    public Joystick moveJoystick;
    public Button attackBtn;
    public Button jumpBtn;

    #endregion


    private int attackIndex = 1;
    private float timerAtc;

    private bool canAttack = true;

    public float attackTimer = 10f;

    private CharacterMovement movement;
    private PlayerCombat combat;
    private CharacterEquipment equipment;
    private CharacterInventory inventory;
    private PlayerStats stats;
    private PlayerAnimation animator;

    private void Start()
    {
        movement = GetComponent<CharacterMovement>();
        combat = GetComponent<PlayerCombat>();
        equipment = GetComponent<CharacterEquipment>();
        inventory = GetComponent<CharacterInventory>();
        animator = GetComponent<PlayerAnimation>();

        stats = GetComponent<PlayerStats>();
        stats.healthBelowZero += PlayerKilled;
        stats.onBeingHit += animator.BeingHit;

        timerAtc = attackTimer;

        attackBtn.onButtonDown += combat.Attack;
        jumpBtn.onButtonDown += combat.Jump;

        equipment.equipment.Load();
        inventory.inventory.Load();
    }

    private void Update()
    {
        movement.moveInput = moveJoystick.InputVector;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            inventory.inventory.Save();
            equipment.equipment.Save();
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            inventory.inventory.Load();
            equipment.equipment.Load();
        }

        if (Input.GetKeyDown(KeyCode.K))
            stats.TakeDamage(50);
    }

    public void PlayerKilled()
    {
        animator.IsKilled();
        animator.enabled = false;
        movement.enabled = false;
        combat.InAction();
        combat.enabled = false;
        inventory.inventory.Clear();
        equipment.equipment.Clear();
    }

    public void Die()
    {
        inventory.inventory.Clear();
        equipment.equipment.Clear();

        Destroy(gameObject);
    }

    private void OnApplicationQuit()
    {
        inventory.inventory.Clear();
        equipment.equipment.Clear();
    }
}
