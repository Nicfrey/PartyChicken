using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBehavior : BulletBehaviour
{
    public float ExplosionRadius { private get; set; } = 5f;
    public int ExplosionDamage { private get; set; } = 20;
    [SerializeField]
    private GameObject explosionEffect;
    [SerializeField]
    private ParticleSystem trailEffect;

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
        Instantiate(explosionEffect,transform.position,transform.rotation);
        Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosionRadius);

        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent<Target>(out var target))
            {
                target.TakeDamage(ExplosionDamage);
            }
        }
    }
}
