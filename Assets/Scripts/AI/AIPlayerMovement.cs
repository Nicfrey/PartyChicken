using System;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public enum AIMovementState
    {
        Idle,
        Moving,
        PickingUpItem,
        MovingToObjective,
        Attacking,
    }
    
    [RequireComponent(typeof(NavMeshAgent))]
    public class AIPlayerMovement : MonoBehaviour
    {
        private NavMeshAgent agent;
        private AIMovementState state = AIMovementState.Idle;
        public AIMovementState State => state;

        private Target target;
        
        
        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            target = GetComponent<Target>();
        }
        

        private void FixedUpdate()
        {
            agent.updateRotation = state != AIMovementState.Attacking;
            if(target.IsDead() || Vector3.Distance(transform.position, agent.destination) < agent.stoppingDistance)
            {
                MoveToPosition(transform.position, AIMovementState.Idle);
                agent.ResetPath();
            }

            if (target.IsDead())
                return;
                
            // Set random destination if idle
            if (state == AIMovementState.Idle)
            {
                Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * 10f;
                randomDirection += transform.position;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomDirection, out hit, 10f, NavMesh.AllAreas))
                {
                    MoveToPosition(randomDirection, AIMovementState.Moving);
                }
            }
        }

        public void MoveToPosition(Vector3 position, AIMovementState newState, float stoppingDistance = 0.2f)
        {
            if(target.IsDead())
                return;
            agent.SetDestination(position);
            agent.stoppingDistance = stoppingDistance;
            state = newState;
        }

        public void SetPlayerPositionAndRotation(Vector3 transformPosition)
        {
            agent.Warp(transformPosition);
        }
    }
}
