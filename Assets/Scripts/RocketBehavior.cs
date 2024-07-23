using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RocketBehavior : BulletBehaviour
{
    public float ExplosionRadius { private get; set; } = 5f;
    public int ExplosionDamage { private get; set; } = 20;
    [SerializeField]
    private GameObject explosionEffect;
    [SerializeField]
    private ParticleSystem trailEffect;
    [SerializeField]
    private float forceImpulse;

    private GameObject explosionEffectCreated;
    private bool hasExploded = false;
    void Start()
    {
        StartSetup();
        trailEffect.Play();
    }

    // Update is called once per frame
    void Update()
    {
        HandlingLifetime();
    }

    void FixedUpdate()
    {
        if(!hasExploded)
            Move();
    }

    void OnCollisionEnter(Collision collision)
    {
        Explode();
        if (collision.gameObject.TryGetComponent<Target>(out var target))
        {
            HandleCollisionTarget(target);
        }
        Destroy(gameObject);
    }

    private void Explode()
    {
        hasExploded = true;
        Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosionRadius);
        bool hasHitPlayer = false;

        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent<Target>(out var target))
            {
                target.TakeDamage(ExplosionDamage);
                if (collider.TryGetComponent<PlayerMovement>(out var playerMovement))
                {
                    Vector3 direction = (collider.transform.position - transform.position) + new Vector3(0,5,0);
                    direction.Normalize();
                    playerMovement.AddImpact(direction,forceImpulse);
                }
                hasHitPlayer = true;
            }
        }

        GameObject effect = Instantiate(explosionEffect, transform.position,
            hasHitPlayer ? Quaternion.identity : Quaternion.Inverse(transform.rotation));
        Destroy(effect,5f);
    }
}
