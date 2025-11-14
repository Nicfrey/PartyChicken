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
        Attacking,
        MovingToObjective
    }
    
    [RequireComponent(typeof(NavMeshAgent))]
    public class AIPlayerMovement : MonoBehaviour
    {
        private NavMeshAgent agent;
        private AIMovementState state = AIMovementState.Idle;
        public AIMovementState State => state;
        
        
        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        private void FixedUpdate()
        {
            if(state != AIMovementState.Idle && agent.remainingDistance <= agent.stoppingDistance)
            {
                state = AIMovementState.Idle;
                return;
            }
                
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

        public void MoveToPosition(Vector3 position, AIMovementState newState)
        {
            agent.SetDestination(position);
            this.state = newState;
        }
        
    }
}
