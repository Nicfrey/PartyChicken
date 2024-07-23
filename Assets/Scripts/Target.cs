using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDamageable
{
    public delegate void OnDeath();
    public event OnDeath onDeath;

    public delegate void OnHealthChanged(int health);
    public event OnHealthChanged onHealthChanged;

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
        {
            onDeath?.Invoke();
            return;
        }

        health -= damage;
        onHealthChanged?.Invoke(health);
    }

    public void AddHealth(int heal)
    {
        health += heal;
        if (health > maxHealth)
            health = maxHealth;
        onHealthChanged?.Invoke(health);
    }

    private bool IsDead()
    {
        return health <= 0;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public int GetHealth()
    {
        return health;
    }
}
