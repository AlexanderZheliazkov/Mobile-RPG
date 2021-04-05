using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : NPCMovement
{
    private Vector3 startPosition;

    protected override void Start()
    {
        base.Start();
        startPosition = transform.position;
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void Moving()
    {
        if (!canMove)
        {
            return;
        }
        agent.isStopped = false;
        //if (mainTarget.position != mainTargetLastPosition && aggresionMode != AggresionMode.Aggressive)
        //{
        //    //Debug.Log("Main is moving!");
        //    agent.isStopped = false;
        //    agent.SetDestination(mainTarget.position);
        //    target = null;
        //    return;
        //}


        if (target != null)
        {
            if (Vector3.Distance(target.position, transform.position) > maxDistanceAway)
            {
                agent.SetDestination(startPosition);
                target = null;
                //Debug.Log("Going to Main!");
                return;
            }

            //Debug.Log(target.name + " is target");
            if (Vector3.Distance(transform.position, target.transform.position) > attackRange)
            {
                agent.SetDestination(target.transform.position);
                //Debug.Log("Going to target!");
                return;

            }
            else //if distance between enemy and champion is <= attackRange
            {
                FaceTarget(target.transform);
                agent.SetDestination(transform.position);
                if (combat != null)
                {
                    combat.Attack();
                }

                return;
            }
        }

        agent.SetDestination(startPosition);
        return;
    }
}
