using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    public StanceState stanceState;
    public EnemyAttackAction[] enemyAttacks;
    public EnemyAttackAction currentAttack;

    public EnemyStats enemyStats;

    public static event Action OnDamage;

    [SerializeField] public AudioSource enemyAttackSound;



    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
    {
        //select attack
        //if selected attack can be used, stop movement and attack our target
        //return to the stance state
        Vector3 targetDir = enemyManager.currentTarget.transform.position - transform.position;
        float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
        float viewAngle = Vector3.Angle(targetDir, transform.forward);

        if (enemyManager.isPerformingAction)
            return stanceState;

        if (currentAttack != null && enemyStats.isAlive)
        {
            //if we are too close to enemy 
            //get a new attack

            if (distanceFromTarget < currentAttack.minimumDistanceToAttack)
            {
                return this;
            }
            else if(distanceFromTarget < currentAttack.maximumDistanceToAttack)
            {
                if (viewAngle <= currentAttack.maximumAttackAngle &&
                    viewAngle >= currentAttack.minimumAttackAngle)
                {
                    if (enemyManager.currentRecoveryTime <= 0 && enemyManager.isPerformingAction == false)
                    {
                        enemyAnimatorManager.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                        enemyAnimatorManager.animator.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);
                        enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
                        enemyManager.isPerformingAction = true;
                        enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
                        currentAttack = null;
                        enemyAttackSound.Play();
                        OnDamage?.Invoke();
                        return stanceState;
                    }
                }
            }
        }
        else
        {
            GetNewAttack(enemyManager);
        }

        return stanceState;
    }

    private void GetNewAttack(EnemyManager enemyManager)
    {
        Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
        float viewAngle = Vector3.Angle(targetDirection, transform.forward);
        float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position);

        int maxScore = 0;
        for (int i = 0; i < enemyAttacks.Length; i++)
        {
            EnemyAttackAction enemyAttackAction = enemyAttacks[i];

            if (distanceFromTarget <= enemyAttackAction.maximumDistanceToAttack
                && distanceFromTarget >= enemyAttackAction.minimumDistanceToAttack)
            {
                if (viewAngle <= enemyAttackAction.maximumAttackAngle
                    && viewAngle >= enemyAttackAction.minimumAttackAngle)
                {
                    maxScore += enemyAttackAction.attackScore;
                }
            }
        }

        int randomValue = UnityEngine.Random.Range(0, maxScore);
        int tempScore = 0;

        for (int i = 0; i < enemyAttacks.Length; i++)
        {
            EnemyAttackAction enemyAttackAction = enemyAttacks[i];

            if (distanceFromTarget <= enemyAttackAction.maximumDistanceToAttack
                && distanceFromTarget >= enemyAttackAction.minimumDistanceToAttack)
            {
                if (viewAngle <= enemyAttackAction.maximumAttackAngle
                    && viewAngle >= enemyAttackAction.minimumAttackAngle)
                {
                    if (currentAttack != null)
                    {
                        return;
                    }

                    tempScore += enemyAttackAction.attackScore;

                    if (tempScore >= randomValue)
                    {
                        currentAttack = enemyAttackAction;
                    }
                }
            }
        }
    }
}
