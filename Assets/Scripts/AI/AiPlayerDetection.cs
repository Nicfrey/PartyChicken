using AI;
using UnityEngine;

public class AiPlayerDetection : MonoBehaviour
{
    [SerializeField] private float defaultDetectionRadius = 6.5f;
    private float detectionRadius;
    [SerializeField] private LayerMask playerLayer;
    private AIPlayerMovement playerMovement;
    private PlayerWeaponHandling weaponHandling;
    
    private Target otherPlayerTarget;

    private void Start()
    {
        playerMovement = GetComponent<AIPlayerMovement>();
        weaponHandling = GetComponent<PlayerWeaponHandling>();
        playerLayer &= ~(1 << gameObject.layer);
        detectionRadius = defaultDetectionRadius;
    }

    private void FixedUpdate()
    {
        if (!weaponHandling.HasWeapon())
            return;
        
        if (otherPlayerTarget)
        {
            detectionRadius = defaultDetectionRadius;
            transform.LookAt(otherPlayerTarget.transform.position);
            if (otherPlayerTarget.IsDead())
            {
                ResetShoot();
            } 
            else if (!weaponHandling.HasAmmo())
            {
                ResetShoot();
                weaponHandling.Throw();
            }
            else if(!otherPlayerTarget.IsDead())
            {
                playerMovement.MoveToPosition(otherPlayerTarget.transform.position, AIMovementState.Attacking, 3f);
                // Detect if angle to other player is less than some threshold
                if (Vector3.Distance(transform.position, otherPlayerTarget.transform.position) <=
                    weaponHandling.GetRange())
                {
                    Vector3 directionToOther = (otherPlayerTarget.transform.position - transform.position).normalized;
                    float angle = Vector3.Angle(transform.forward, directionToOther);
                    if (angle < 45f)
                        weaponHandling.Shoot(true);
                    else
                        weaponHandling.Shoot(false);
                }
            }
        }
        else
        {
            if (!weaponHandling.HasAmmo())
            {
                weaponHandling.Throw();
            }
            detectionRadius += Time.deltaTime;
            Collider[] results = new Collider[10];
            var size = Physics.OverlapSphereNonAlloc(transform.position, detectionRadius, results, playerLayer);
            float closestDistance = float.MaxValue;
            Collider closestCollider = null;
            for (int i = 0; i < size; i++)
            {
                float distance = Vector3.Distance(transform.position, results[i].transform.position);
                if (distance < closestDistance)
                {
                    if (results[i].GetComponent<Target>().IsDead())
                        continue;
                    closestDistance = distance;
                    closestCollider = results[i];
                }
            }
            if (closestCollider)
            {
                otherPlayerTarget = closestCollider.GetComponent<Target>();
                playerMovement.MoveToPosition(otherPlayerTarget.transform.position, AIMovementState.Attacking,3f);
            }
        }
    }

    private void ResetShoot()
    {
        otherPlayerTarget = null;
        weaponHandling.Shoot(false);
        playerMovement.MoveToPosition(transform.position, AIMovementState.Idle);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
