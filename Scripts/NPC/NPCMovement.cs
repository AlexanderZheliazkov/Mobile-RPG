using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class NPCMovement : MonoBehaviour
{
    public float attackRange;
    public float lookRange;

    public LayerMask targetingMask;
    //public Transform mainTarget;
    //protected Vector3 mainTargetLastPosition;
    protected Transform target;

    [HideInInspector] public bool canMove;
    protected float maxDistanceAway;
    protected NavMeshAgent agent;
    protected NPCCombat combat;

    protected virtual void Start()
    {
        //if (mainTarget == null)
        //    mainTarget = gameObject.transform;

        //mainTargetLastPosition = mainTarget.position;

        maxDistanceAway = lookRange + attackRange;
        agent = GetComponent<NavMeshAgent>();

        if (GetComponent<NPCCombat>())
        {
            combat = GetComponent<NPCCombat>();
        }

    }

    protected virtual void Update()
    {
        if (target == null)
        {
            canMove = true;
            target = GetTarget(targetingMask);
        }

        if (canMove)
            Moving();
        else
            agent.isStopped = true;

        if (Input.GetKey(KeyCode.A))
        {
            combat.Attack();
        }

    }

    protected Transform GetTarget(LayerMask targetMaks)
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, lookRange, targetMaks);
        if (cols.Length == 0) return null;
        GameObject closestEnemy = cols[0].gameObject;
        foreach (Collider col in cols)
        {
            if (Vector3.Distance(transform.position, col.transform.position) <= Vector3.Distance(transform.position, closestEnemy.transform.position))
            {
                closestEnemy = col.gameObject;
            }
        }
        return closestEnemy.transform;
    }

    protected abstract void Moving();

    protected void FaceTarget(Transform targetToFace)
    {
        if (!canMove) return;
        Vector3 direction = (targetToFace.position - transform.position).normalized;
        Quaternion LookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, LookRotation, Time.deltaTime * 5f);
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, lookRange);
    }

    public void CanMove()
    {
        canMove = true;
    }

    public void CanNotMove()
    {
        canMove = false;
    }
}
