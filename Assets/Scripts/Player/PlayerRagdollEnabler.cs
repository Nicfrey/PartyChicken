using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class PlayerRagdollEnabler : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField] 
    private CapsuleCollider mainCollider;
    [SerializeField] 
    private Rigidbody rb;
    [SerializeField] 
    private bool ragdollOnStart;
    private Collider[] ragdolColliders;
    private Rigidbody[] ragdolRigidbodies;
    private Target health;
    
    private void Awake()
    {
        health = GetComponentInParent<Target>();
        health.onDeath.AddListener(OnDeath);
        health.onRevive.AddListener(OnRevive);
        ragdolColliders = GetComponentsInChildren<Collider>();
        ragdolRigidbodies = GetComponentsInChildren<Rigidbody>();
        
        RemovePunchColliders();
    }
    
    private void RemovePunchColliders()
    {
        List<Collider> colliderList = new List<Collider>(ragdolColliders);
        PlayerPunch[] punches = GetComponentsInChildren<PlayerPunch>();
        foreach (var punch in punches)
        {
            colliderList.Remove(punch.GetComponent<Collider>());
        }
        ragdolColliders = colliderList.ToArray();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        health.onDeath.AddListener(OnDeath);
        health.onRevive.AddListener(OnRevive);
        
        SetRagdollEnabled(ragdollOnStart);
    }

    private void SetRagdollEnabled(bool enabled)
    {
        animator.enabled = !enabled;

        foreach (Rigidbody rb in ragdolRigidbodies)
        {
            rb.isKinematic = !enabled;
            rb.useGravity = enabled;
        }

        foreach (Collider col in ragdolColliders)
        {
            col.enabled = enabled;
        }
    }

    public void EnableRagdoll(Vector3 force, Vector3 hitPoint)
    {
        SetRagdollEnabled(true);
        
        Rigidbody nearestRb = GetNearestRigidbody(hitPoint);
        if (nearestRb != null)
        {
            nearestRb.AddForce(force, ForceMode.Impulse);
        }
    }
    
    private Rigidbody GetNearestRigidbody(Vector3 point)
    {
        Rigidbody nearestRb = null;
        float minDistance = float.MaxValue;
    
        foreach (Rigidbody rb in ragdolRigidbodies)
        {
            float distance = Vector3.Distance(rb.position, point);
        
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestRb = rb;
            }
        }
    
        return nearestRb;
    }
    
    private void OnRevive()
    {
        SetRagdollEnabled(false);
    }

    private void OnDeath(PlayerStatistics arg0)
    {
        var direction = Random.insideUnitSphere;
        if(arg0 != null) 
            direction = (transform.position - arg0.transform.position).normalized;
        EnableRagdoll(direction * 5f, transform.position);
    }
}
