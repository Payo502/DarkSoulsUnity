using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorManager : AnimatorManager
{
    EnemyManager enemyManager;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyManager = GetComponentInParent<EnemyManager>();
    }

    private void OnEnable()
    {
        CountdownTimer.OnTimerEnd += DisableEnemyMovement;
    }

    private void OnDisable()
    {
        CountdownTimer.OnTimerEnd -= DisableEnemyMovement;
    }

    private void Start()
    {
        EnableEnemyMovement();
    }

    private void OnAnimatorMove()
    {
        float delta = Time.deltaTime;
        enemyManager.enemyRigidBody.drag = 0;
        Vector3 deltaPosition = animator.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        enemyManager.enemyRigidBody.velocity = velocity;
    }

    private void DisableEnemyMovement()
    {
        animator.enabled = false;
        enemyManager.enemyRigidBody.constraints = RigidbodyConstraints.FreezeAll;
    }

    private void EnableEnemyMovement()
    {
        animator.enabled = true;
        enemyManager.enemyRigidBody.constraints = RigidbodyConstraints.FreezeRotation;
    }
}
