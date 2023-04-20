using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    [Header("Enemy Specific - Movement")]
    public float stoppingDistance;
    
    [Header("Enemy Specific - Detection")]
    public float detectionRadius = 20;
    public float minDetectionAngle = -50;
    public float maxDetectionAngle = 50;

    [Header("Enemy Specific - Attack")]
    public EnemyActionAttack[] enemyAttacks;
    public float currentRecoveryTime = 0;
    

    void Start()
    {
        maxHealth = baseHealth * scaleHealth;
        currentHealth = maxHealth;
        
        _hudManager.UpdateUI("enemy_health", currentHealth, maxHealth);
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        _hudManager.UpdateUI("enemy_health", currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
        }
    }
}
