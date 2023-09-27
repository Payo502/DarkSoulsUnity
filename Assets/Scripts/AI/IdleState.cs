using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public FollowTargetState targetState;
    public LayerMask detectionLayer;
    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
    {
        //look for potential target
        //if target detected switch to followplayer state

        Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();

            if (characterStats != null)
            {
                Vector3 targetDirection = characterStats.transform.position - transform.position;
                float viewAngle = Vector3.Angle(targetDirection, transform.forward);

                if (viewAngle > enemyManager.minimumDetectionAngle && viewAngle < enemyManager.maximumDetectionAngle)
                {
                    enemyManager.currentTarget = characterStats;
                }
            }
        }

        if (enemyManager.currentTarget != null)
        {
            return targetState;
        }
        else
        {
            return this;
        }
    }
}
