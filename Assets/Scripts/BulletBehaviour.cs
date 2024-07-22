using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public float Speed { private get; set; } = 10f;
    public float Range { private get; set; } = 10f;
    private float distanceTravelled = 0f;
    private Vector3 startPosition;
    public bool StartMove { private get; set; } = false;
    public int Damage { private get; set; } = 10;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        HandlingLifetime();
    }

    void FixedUpdate()
    {
        if (StartMove)
            rb.velocity = transform.forward * Speed;
    }

    private void HandlingLifetime()
    {
        distanceTravelled = Vector3.Distance(startPosition, transform.position);

        if (distanceTravelled >= Range)
        {
            Destroy(gameObject);
        }
    }

    public void SetDirection(Vector3 direction)
    {
        transform.forward = direction;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Target>(out var target))
        {
            target.TakeDamage(Damage);
            Destroy(gameObject);
        }
    }

    public void SetupBullet(float speed, float range, int damage)
    {
        Speed = speed;
        Range = range;
        Damage = damage;
    }
}
