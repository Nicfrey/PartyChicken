using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExplosiveBarrelBehavior : MonoBehaviour
{
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private float explosionRadius;
    [SerializeField] private int explosionDamage;

    [SerializeField] 
    private GameObject explodedBarrel;
    [SerializeField]
    private GameObject unexplodedBarrel;
    [SerializeField]
    private ParticleSystem fire;
    [SerializeField]
    private float forceImpulse;

    private AnimationCurve explosionCurve;

    private Target target;

    void Awake()
    {
        explodedBarrel.SetActive(false);
        unexplodedBarrel.SetActive(true);
        explosionCurve = new AnimationCurve();
        explosionCurve.AddKey(0f, explosionDamage);
        explosionCurve.AddKey(explosionRadius, 20f);
    }

    // Start is called before the first frame update
    void Start()
    {
        target = GetComponent<Target>();
        target.onDeath += Explode;
        target.onHealthChanged += ActivateFire;
    }

    private void ActivateFire(int health)
    {
        if (target.GetHealth() <= target.GetMaxHealth() / 2)
        {
            fire.Play();
            target.onHealthChanged -= ActivateFire;
        }
    }

    private void Explode()
    {
        target.onDeath -= Explode;
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        bool hasHitPlayer = false;

        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent<Target>(out var target))
            {
                if (target == this.target)
                {
                    continue;
                }

                float distance = Vector3.Distance(transform.position, target.transform.position);
                target.TakeDamage((int) explosionCurve.Evaluate(distance));
                if (collider.TryGetComponent<PlayerMovement>(out var playerMovement))
                {
                    Vector3 direction = (collider.transform.position - transform.position) + new Vector3(0, 5, 0);
                    direction.Normalize();
                    playerMovement.AddImpact(direction, forceImpulse);
                }

                hasHitPlayer = true;
            }
        }

        GameObject effect = Instantiate(explosionEffect, transform.position,
            hasHitPlayer ? Quaternion.identity : Quaternion.Inverse(transform.rotation));
        effect.transform.localScale = Vector3.one * explosionRadius;
        Destroy(effect, 5f);
        unexplodedBarrel.SetActive(false);
        explodedBarrel.SetActive(true);
    }

    void OnDestroy()
    {
        target.onDeath -= Explode;
        target.onHealthChanged -= ActivateFire;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}

