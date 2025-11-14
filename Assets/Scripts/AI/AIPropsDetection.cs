using System;
using UnityEngine;

namespace AI
{
    public class AIPropsDetection : MonoBehaviour
    {
        [SerializeField]
        private float detectionRadius = 5f;
        [SerializeField]
        private LayerMask layerMask;

        private Target target;
        private PlayerWeaponHandling weaponHandling;
        private AIPlayerMovement playerMovement;
        
        private void Start()
        {
            target = GetComponent<Target>();
            weaponHandling = GetComponent<PlayerWeaponHandling>();
            playerMovement = GetComponent<AIPlayerMovement>();
        }

        private void FixedUpdate()
        {
            if (playerMovement.State > AIMovementState.PickingUpItem || (!NeedsWeapon() && !NeedsHealth()))
                return;
            
            Collider[] results = new Collider[10];
            var size = Physics.OverlapSphereNonAlloc(transform.position, detectionRadius, results, layerMask);
            float closestDistance = float.MaxValue;
            Collider closestCollider = null;
            for (int i = 0; i < size; i++)
            {
                float distance = Vector3.Distance(transform.position, results[i].transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    if(NeedsWeapon() && results[i].TryGetComponent<WeaponHolder>(out _))
                    {
                        closestCollider = results[i];
                        continue;
                    }
                    if(NeedsHealth() && results[i].TryGetComponent<HeartBehavior>(out _))
                    {
                        closestCollider = results[i];
                        continue;
                    }
                }
            }
            if (closestCollider)
            {
                playerMovement.MoveToPosition(closestCollider.transform.position,AIMovementState.PickingUpItem);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
        }

        private bool NeedsWeapon()
        {
            return target.GetHealth() > 30 && !weaponHandling.HasWeapon();
        }
        
        private bool NeedsHealth()
        {
            return target.GetHealth() <= 30;
        }
        
        
    }
}
