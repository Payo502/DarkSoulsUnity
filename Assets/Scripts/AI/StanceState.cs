using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StanceState : State
{
    public AttackState attackState;
    public FollowTargetState followTargetState;
    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
    {
        //check for attack range
        //if in attack distance switch to attack state
       float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

        if (enemyManager.isPerformingAction)
        {
            enemyAnimatorManager.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
        }

        if (enemyManager.currentRecoveryTime <= 0 && distanceFromTarget <= enemyManager.maximumAttackRange)
        {
            return attackState;
        }
        else if(distanceFromTarget > enemyManager.maximumAttackRange)
        {
            return followTargetState;
        }
        else
        {
            return this;
        }
    }
}
