using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCAnimacion : MonoBehaviour
{
    private const float SMOOTH_TIME = .1f;

    private int animationAttackIndex;
    protected float speedModifier = 1f;
    protected Animator anim;
    protected NavMeshAgent agent;
    protected AnimatorOverrideController overrideController;

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

    public void BeingHit()
    {
        anim.SetTrigger("Hit");
    }

    public void Killed()
    {
        anim.SetTrigger("Die");
    }
}
