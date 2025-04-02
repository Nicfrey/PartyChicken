using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private Target health;
    private Rigidbody rbCopy;
    
    // Start is called before the first frame update
    void Start()
    {
        rbCopy = rb;
        health = GetComponentInParent<Target>();
        health.onDeath.AddListener(OnDeath);
        health.onRevive.AddListener(OnRevive);
        ragdolColliders = GetComponentsInChildren<Collider>();
        if(!ragdollOnStart)
        {
            foreach (Collider collider in ragdolColliders)
            {
                collider.isTrigger = true;
            }
        }
        else
        {
            mainCollider.enabled = false;
            animator.enabled = false;
            rb.isKinematic = true;
        }
    }

    private void OnRevive()
    {
        foreach (Collider collider in ragdolColliders)
        {
            collider.isTrigger = true;
        }
        animator.enabled = true;
        mainCollider.enabled = true;
        rb.isKinematic = false;
    }

    private void OnDeath(PlayerStatistics arg0)
    {
        foreach (Collider collider in ragdolColliders)
        {
            collider.isTrigger = false;
        }
        animator.enabled = false;
        mainCollider.enabled = false;
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
    }
}
