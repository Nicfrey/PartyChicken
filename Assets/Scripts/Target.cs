using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDamageable
{
    public delegate void OnDeath();
    public event OnDeath onDeath;

    [SerializeField]
    private int maxHealth = 100;

    private int health;

    void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (IsDead())
            return;

        health -= damage;
    }

    private bool IsDead()
    {
        return health <= 0;
    }
}
