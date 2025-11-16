using System;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.AI;
using Utils.AI;

namespace AI
{
    public class AiPlayerManager : MonoBehaviour
    {
        [SerializeField] private float stoppingDistanceDefault = 0.2f;
        [SerializeField] private float attackRange = 2f;
        [SerializeField] private LayerMask playerMask;
        [SerializeField] private LayerMask propsMask;
        [SerializeField] private float defaultDetectionRadiusPlayer = 6.5f;
        [SerializeField] private float defaultDetectionRadiusProps = 5f;

        private FiniteStateMachine finiteStateMachine;
        private Blackboard blackboard;

        private void Start()
        {
            blackboard = new Blackboard();
            blackboard.AddData("Health", GetComponent<Target>());
            blackboard.AddData("PlayerWeaponHandler", GetComponent<PlayerWeaponHandling>());
            blackboard.AddData("Transform", transform);
            blackboard.AddData("NavMeshAgent", GetComponent<NavMeshAgent>());
            blackboard.AddData("TargetEnemy", default(Target));
            blackboard.AddData("TargetPropTransform", default(Transform));
            blackboard.AddData("DetectionRadiusPlayer", 5f);

            PatrolState patrolState = new PatrolState(stoppingDistanceDefault);
            MoveToProps moveToPropsState = new MoveToProps(stoppingDistanceDefault);
            AttackEnemy attackEnemyState = new AttackEnemy();
            DeadState deadState = new DeadState();

            HasDetectedProps hasDetectedPropsCondition = new HasDetectedProps(defaultDetectionRadiusProps, propsMask);
            EnemyIsDead enemyIsDeadCondition = new EnemyIsDead();
            HasNoAmmo noAmmoCondition = new HasNoAmmo();
            IsPlayerDead isPlayerDeadCondition = new IsPlayerDead();
            IsEnemyInRange isEnemyInRangeCondition = new IsEnemyInRange(playerMask, 6.5f);
            HasTakenProp takenPropCondition = new HasTakenProp();
            IsPlayerAlive isPlayerAliveCondition = new IsPlayerAlive();

            finiteStateMachine = new FiniteStateMachine(patrolState, blackboard);
            finiteStateMachine.AddTransition(patrolState, moveToPropsState, hasDetectedPropsCondition);
            finiteStateMachine.AddTransition(patrolState, attackEnemyState, isEnemyInRangeCondition);
            finiteStateMachine.AddTransition(attackEnemyState, patrolState, enemyIsDeadCondition);
            finiteStateMachine.AddTransition(attackEnemyState, patrolState, noAmmoCondition);
            finiteStateMachine.AddTransition(attackEnemyState, deadState, isPlayerDeadCondition);
            finiteStateMachine.AddTransition(moveToPropsState, patrolState, takenPropCondition);
            finiteStateMachine.AddTransition(moveToPropsState, deadState, isPlayerDeadCondition);
            finiteStateMachine.AddTransition(patrolState, deadState, isPlayerDeadCondition);
            finiteStateMachine.AddTransition(deadState, patrolState, isPlayerAliveCondition);
        }
        
        private void Update()
        {
            Debug.Log($"{gameObject.name} Current State: {finiteStateMachine.GetCurrentState().GetType().Name}");
            finiteStateMachine.Update();
        }

        public void SetPlayerPositionAndRotation(Vector3 transformPosition)
        {
            GetComponent<NavMeshAgent>().Warp(transformPosition);
        }
    }
}