using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Target : MonoBehaviour, IDamageable
{
    public UnityEvent<PlayerStatistics> onDeath;
    public UnityEvent<int> onHealthChanged;
    public UnityEvent onRevive;

    [SerializeField]
    private int maxHealth = 100;

    private int health;

    void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(int damage, PlayerStatistics shootingPlayer)
    {
        if (IsDead())
            return;
        
        health -= damage;
        if (IsDead())
        {
            onDeath?.Invoke(shootingPlayer);
            return;
        }
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

    public void Revive()
    {
        health = maxHealth;
        onRevive?.Invoke();
    }
}
