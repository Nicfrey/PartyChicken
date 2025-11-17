using System;
using System.Collections.Generic;
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
        
        [SerializeField] private GameObject avatar;
        
        private Target target;
        private Animator animator;
        private NavMeshAgent agent;
        private FiniteStateMachine finiteStateMachine;
        private Blackboard blackboard;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            target = GetComponent<Target>();
            animator = avatar.GetComponent<Animator>();
        }

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

            PatrolState patrolState = new PatrolState(stoppingDistanceDefault, GetMovePatrols());
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
            finiteStateMachine.Update();
        }

        private void LateUpdate()
        {
            Vector3 currentVelocity = agent.velocity;
            currentVelocity.y = 0f;
            animator.SetFloat("Speed", currentVelocity.magnitude);
            animator.SetBool("IsGrounded", true);
            Vector3 velocityWithAvatarRotation = avatar.transform.InverseTransformDirection(currentVelocity);
            animator.SetFloat("Right",velocityWithAvatarRotation.x);
            animator.SetFloat("Forward", velocityWithAvatarRotation.z);
            animator.SetBool("IsDead",target.IsDead());
        }

        public void SetPlayerPositionAndRotation(Vector3 transformPosition)
        {
            GetComponent<NavMeshAgent>().Warp(transformPosition);
        }
        
        public void RemovePlayerLayerMask(int layerMask)
        {
            LayerMask newMask = playerMask & ~(1 << layerMask);
            playerMask = newMask;
            finiteStateMachine?.GetCondition<IsEnemyInRange>().SetLayerMask(playerMask);
        }
        private GameObject[] GetMovePatrols()
        {
            GameObject[] patrols = GameObject.FindGameObjectsWithTag("PatrolPosition");
            return patrols;
        }

        public void ResetPath()
        {
            GetComponent<NavMeshAgent>().ResetPath();
        }
    }
}