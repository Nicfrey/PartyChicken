using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.AI;
using Utils.AI;

namespace AI
{
    public class HasDetectedProps : ICondition
    {
        private float detectionRadius;
        private LayerMask propsLayer;

        public HasDetectedProps(float detectionRadius, LayerMask propsLayer)
        {
            this.detectionRadius = detectionRadius;
            this.propsLayer = propsLayer;
        }

        public bool Evaluate(Blackboard blackboard)
        {
            blackboard.GetData("Transform", out Transform transform);
            blackboard.GetData("Health", out Target health);
            blackboard.GetData("PlayerWeaponHandler", out PlayerWeaponHandling playerWeaponHandling);
            bool needsWeapon = NeedsWeapon(health.GetHealth(), playerWeaponHandling.HasWeapon());
            bool needsHealth = NeedsHealth(health.GetHealth());

            Collider[] results = new Collider[10];
            var size = Physics.OverlapSphereNonAlloc(transform.position, detectionRadius, results, propsLayer);
            
            float closestDistance = float.MaxValue;
            Collider closestCollider = null;
            
            for (int i = 0; i < size; i++)
            {
                Collider collider = results[i];
                if (!collider)
                    continue;
                
                float distance = Vector3.Distance(transform.position, collider.transform.position);
                bool isValid = false;

                if (needsWeapon && collider.TryGetComponent<WeaponHolder>(out WeaponHolder weaponHolder))
                {
                    if (weaponHolder.HasWeapon())
                    {
                        isValid = true;
                    }
                }
                else if (needsHealth && collider.TryGetComponent<HeartBehavior>(out HeartBehavior heartBehavior))
                {
                    if (heartBehavior.CanHeal())
                    {
                        isValid = true;
                    }
                }

                if (isValid && distance < closestDistance)
                {
                    closestDistance = distance;
                    closestCollider = collider;
                }
            }

            if (closestCollider)
            {
                blackboard.ChangeData("TargetPropTransform", closestCollider.transform);
                return true;
            }

            return false;
        }

        private bool NeedsWeapon(int health, bool hasWeapon)
        {
            return health > 50 && !hasWeapon;
        }

        private bool NeedsHealth(int health)
        {
            return health <= 30;
        }
    }

    public class EnemyIsDead : ICondition
    {
        public bool Evaluate(Blackboard blackboard)
        {
            blackboard.GetData("TargetEnemy", out Target targetEnemy);
            if (targetEnemy == null)
                return true;
            return targetEnemy.IsDead();
        }
    }

    public class IsEnemyInRange : ICondition
    {
        private LayerMask playerLayer;
        private readonly float defaultDetectionRadius;

        public IsEnemyInRange(LayerMask playerLayer, float defaultDetectionRadius)
        {
            this.playerLayer = playerLayer;
            this.defaultDetectionRadius = defaultDetectionRadius;
        }
        
        public void SetLayerMask(LayerMask playerLayer)
        {
            this.playerLayer = playerLayer;
        }

        public bool Evaluate(Blackboard blackboard)
        {
            blackboard.GetData("PlayerWeaponHandler", out PlayerWeaponHandling playerWeaponHandling);
            if (!playerWeaponHandling.HasWeapon())
            {
                return false;
            }
            
            blackboard.GetData("DetectionRadiusPlayer", out float detectionRadius);
            blackboard.GetData("Transform", out Transform transform);
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
                blackboard.ChangeData("TargetEnemy", closestCollider.GetComponent<Target>());
                blackboard.ChangeData("DetectionRadiusPlayer", defaultDetectionRadius);
                return true;
            }

            blackboard.ChangeData("DetectionRadiusPlayer", detectionRadius);
            return false;
        }
    }

    public class HasNoAmmo : ICondition
    {
        public bool Evaluate(Blackboard blackboard)
        {
            blackboard.GetData("PlayerWeaponHandler", out PlayerWeaponHandling playerWeaponHandling);
            if (!playerWeaponHandling.HasWeapon())
                return true;
            
            return !playerWeaponHandling.HasAmmo();
        }
    }

    public class IsPlayerDead : ICondition
    {
        public bool Evaluate(Blackboard blackboard)
        {
            blackboard.GetData("Health", out Target health);
            return health.IsDead();
        }
    }

    public class HasTakenProp : ICondition
    {
        public bool Evaluate(Blackboard blackboard)
        {
            blackboard.GetData("TargetPropTransform", out Transform transform);
            return !transform;
        }
    }

    public class IsPlayerAlive : ICondition
    {
        public bool Evaluate(Blackboard blackboard)
        {
            blackboard.GetData("Health", out Target health);
            return !health.IsDead();
        }
    }
}