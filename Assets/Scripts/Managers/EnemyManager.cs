using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    Enemy enemy;
    EnemyAnimatorManager enemyAnimatorManager;
    EnemyStats enemyStats;
    public NavMeshAgent navMeshAgent;
    public Rigidbody enemyRigidBody;

    public State currentState;
    public CharacterStats currentTarget;

    public bool isPerformingAction;
    public bool isInteracting;
    public float rotationSpeed = 15;
    public float maximumAttackRange = 1.5f;

    public int damage = 25;

    public float detectionRadius = 20;

    //enemies field of view
    public float maximumDetectionAngle = 50;
    public float minimumDetectionAngle = -50;
    public float currentRecoveryTime = 0;


    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        enemyStats = GetComponent<EnemyStats>();
        enemyRigidBody = GetComponent<Rigidbody>();
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        navMeshAgent.enabled = false;
    }

    private void Start()
    {
        enemyRigidBody.isKinematic = false;
    }

    private void Update()
    {
        HandleRecoveryTimer();

        isInteracting = enemyAnimatorManager.animator.GetBool("isInteracting");
    }

    private void FixedUpdate()
    {
        HandleStateMachine();
    }

    private void HandleStateMachine()
    {
        if (currentState != null)
        {
            State nextState = currentState.Tick(this, enemyStats, enemyAnimatorManager);

            if (nextState != null)
            {
                SwitchToNextState(nextState);
            }
        }
    }

    private void SwitchToNextState(State state)
    {
        currentState = state;
    }

    private void HandleRecoveryTimer()
    {
        if (currentRecoveryTime > 0)
        {
            currentRecoveryTime -= Time.deltaTime;
        }

        if (isPerformingAction)
        {
            if (currentRecoveryTime <= 0)
            {
                isPerformingAction = false;
            }

        }
    }


}
