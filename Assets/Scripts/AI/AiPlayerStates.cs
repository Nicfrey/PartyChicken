using UnityEngine;
using UnityEngine.AI;
using Utils.AI;

namespace AI
{
    public class PatrolState : IState
    {
        private readonly float stoppingDistance;
        private readonly float patrolRadius = 10f;

        private NavMeshAgent cachedAgent;
        private Transform cachedTransform;
        
        public PatrolState(float stoppingDistance, float patrolRadius = 10f)
        {
            this.patrolRadius = patrolRadius;
            this.stoppingDistance = stoppingDistance;
        }
        
        public void OnEnter(Blackboard blackboard)
        {
            blackboard.GetData("NavMeshAgent", out cachedAgent);
            blackboard.GetData("Transform", out cachedTransform);
            
            cachedAgent.stoppingDistance = stoppingDistance;
            cachedAgent.isStopped = false;
            FindNewDestination();
        }

        public void OnExit(Blackboard blackboard)
        {
            cachedAgent.ResetPath();
        }

        public void Update(Blackboard blackboard)
        {
            if (!cachedAgent.pathPending && cachedAgent.remainingDistance <= cachedAgent.stoppingDistance)
            {
                if (!cachedAgent.hasPath || cachedAgent.velocity.sqrMagnitude < 0.1f)
                    FindNewDestination();
            }
        }

        private void FindNewDestination()
        {
            Vector2 randomCircle = Random.insideUnitCircle * patrolRadius;
            Vector3 randomDirection = new Vector3(randomCircle.x, 0f, randomCircle.y);
            Vector3 targetPosition = cachedTransform.position + randomDirection;
            
            if (TryFindValidNavMeshPosition(targetPosition, out Vector3 validPosition))
            {
                Debug.Log($"Found new patrol destination at {validPosition}");
                cachedAgent.SetDestination(validPosition);
            }
            else
            {
                Debug.LogWarning("[PatrolState] Impossible de trouver une position valide sur le NavMesh");
            }
        }
        
        private bool TryFindValidNavMeshPosition(Vector3 targetPosition, out Vector3 validPosition, int maxAttempts = 5)
        {
            validPosition = Vector3.zero;
        
            for (int i = 0; i < maxAttempts; i++)
            {
                if (NavMesh.SamplePosition(targetPosition, out NavMeshHit hit, patrolRadius, NavMesh.AllAreas))
                {
                    validPosition = hit.position;
                    return true;
                }
            
                Vector2 randomCircle = Random.insideUnitCircle * patrolRadius;
                targetPosition = cachedTransform.position + new Vector3(randomCircle.x, 0f, randomCircle.y);
            }
        
            return false;
        }
    }

    public class MoveToProps : IState
    {
        private readonly float stoppingDistance;
        public MoveToProps(float stoppingDistance)
        {
            this.stoppingDistance = stoppingDistance;
        }
        
        public void OnEnter(Blackboard blackboard)
        {
            blackboard.GetData("NavMeshAgent", out NavMeshAgent navMeshAgent);
            navMeshAgent.stoppingDistance = stoppingDistance;
            
            blackboard.GetData("TargetPropTransform", out Transform targetPropTransform);
            navMeshAgent.SetDestination(targetPropTransform.position);
        }

        public void OnExit(Blackboard blackboard)
        {
            blackboard.GetData("NavMeshAgent", out NavMeshAgent navMeshAgent);
            navMeshAgent.ResetPath();
            blackboard.ChangeData("TargetPropTransform", default(Transform));
        }

        public void Update(Blackboard blackboard)
        {
            blackboard.GetData("NavMeshAgent", out NavMeshAgent navMeshAgent);
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                blackboard.ChangeData("TargetPropTransform", default(Transform));
            }
        }
    }

    public class AttackEnemy : IState
    {
        public void OnEnter(Blackboard blackboard)
        {
            blackboard.GetData("NavMeshAgent", out NavMeshAgent navMeshAgent);
            blackboard.GetData("TargetEnemy", out Target targetEnemy);
            Debug.Log($"[AttackEnemy] Targeting enemy: {targetEnemy.gameObject.name}");
            navMeshAgent.SetDestination(targetEnemy.transform.position);
        }

        public void OnExit(Blackboard blackboard)
        {
            blackboard.GetData("PlayerWeaponHandler", out PlayerWeaponHandling playerWeaponHandling);
            playerWeaponHandling.Shoot(false);
            playerWeaponHandling.Throw();
            blackboard.ChangeData("TargetEnemy", default(Target));
        }

        public void Update(Blackboard blackboard)
        {
            blackboard.GetData("PlayerWeaponHandler", out PlayerWeaponHandling playerWeaponHandling);
            if (!playerWeaponHandling.HasWeapon())
                return;
            
            blackboard.GetData("TargetEnemy", out Target targetEnemy);
            blackboard.GetData("NavMeshAgent", out NavMeshAgent navMeshAgent);
            blackboard.GetData("Transform", out Transform transform);
            navMeshAgent.SetDestination(targetEnemy.transform.position);
            if (Vector3.Distance(transform.position, targetEnemy.transform.position) <= playerWeaponHandling.GetRange())
            {
                Vector3 directionToEnemy = (targetEnemy.transform.position - transform.position).normalized;
                float angle = Vector3.Angle(transform.forward, directionToEnemy);
                playerWeaponHandling.Shoot(angle is < 45f and > -45f);
            }
        }
    }
    
    public class DeadState : IState
    {
        public void OnEnter(Blackboard blackboard)
        {
            blackboard.GetData("NavMeshAgent", out NavMeshAgent navMeshAgent);
            navMeshAgent.ResetPath();
        }

        public void OnExit(Blackboard blackboard)
        {
            
        }

        public void Update(Blackboard blackboard)
        {
            
        }
    }
    
}