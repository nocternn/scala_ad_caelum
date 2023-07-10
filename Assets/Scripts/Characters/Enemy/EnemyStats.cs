using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    [Header("A.I. Settings")]
    public float currentRecoveryTime = 0;
    public float maxAttackRange = 10;

    [Header("Properties")]
    public Enums.EnemyType enemyType;
    [SerializeField] private Action[] _actions;

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

    public Action[] GetActions()
    {
        return _actions;
    }
}
