using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAnimation : MonoBehaviour
{
    private const float SMOOTH_TIME = .1f;

    private Animator anim;
    private AnimatorOverrideController overrideController;

    private CharacterEquipment equipment;

    private int animationAttackIndex;

    protected NavMeshAgent agent;

    protected float speedModifier = 1f;

    public AnimationClip baseIdelAnim;
    public AnimationClip baseRunAnim;

    void Start()
    {
        #region SetUpAnimator
        if (GetComponent<Animator>())
        {
            anim = GetComponent<Animator>();
        }
        else
        {
            Debug.Log(transform.name + " is missing animator!");
        }
        #endregion

        #region SetUpNavMeshAgent
        if (GetComponent<NavMeshAgent>())
        {
            agent = GetComponent<NavMeshAgent>();
        }
        else
        {
            Debug.Log(transform.name + " is missing NavMeshAgent!");
        }
        #endregion

        #region SetUpOverrideController
        overrideController = new AnimatorOverrideController(anim.runtimeAnimatorController);    //saving current animator controller as new controller
        anim.runtimeAnimatorController = overrideController;                                    //then setting up our controller to be the new controller
        #endregion

        #region SetCharacterEquipment
        if (GetComponent<CharacterEquipment>())
        {
            equipment = GetComponent<CharacterEquipment>();
            equipment.onWeaponChanged += ChangeAnimationsSet;
        }
        else
        {
            Debug.Log(transform.name + " is missing CharacterEquipment!");
        } 
        #endregion

        animationAttackIndex = 1;
    }

    void Update()
    {
        float curSpeed = agent.velocity.magnitude / agent.speed;
        anim.SetFloat("speedPercent", curSpeed * speedModifier, SMOOTH_TIME, Time.deltaTime);
    }

    public virtual void OnAttack(AnimationClip animation)
    {
        if (animation == null) return;

        if (anim.GetInteger("AttackIndex") == 1)
        {
            overrideController["Attack2"] = animation;
            anim.SetInteger("AttackIndex", 2);
            anim.SetTrigger("Attack");

        }
        else if (anim.GetInteger("AttackIndex") == 2)
        {
            overrideController["Attack1"] = animation;
            anim.SetInteger("AttackIndex", 1);
            anim.SetTrigger("Attack");
        }
    }

    public void Jump()
    {
        anim.SetTrigger("Roll");
    }

    public void SetAnimationController(AnimatorOverrideController controller)
    {
        anim.runtimeAnimatorController = controller;
    }

    public void ChangeAnimationsSet(WeaponItem weapon)
    {
        if (weapon == null)
        {
            if (baseIdelAnim != null)
                overrideController["Idle_basic"] = baseIdelAnim;
            if (baseRunAnim != null)
                overrideController["Run_basic"] = baseRunAnim;

            return;
        }

        overrideController["Idle_basic"] = weapon.idleAnim;
        overrideController["Run_basic"] = weapon.runAnim;
    }

    public void SetMoveInput(bool input)
    {
        anim.SetBool("MoveInput", input);
    }

    public void BeingHit()
    {
        anim.SetTrigger("Hit");
    }

    public void IsKilled()
    {
        anim.SetTrigger("Die");
    }
}
