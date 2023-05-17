using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    public EnemyActionAttack[] enemyAttacks;

    void Start()
    {
        maxHealth = ScaleStat(baseHealth, scaleHealth);
        currentHealth = maxHealth;
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
        }
    }
}
