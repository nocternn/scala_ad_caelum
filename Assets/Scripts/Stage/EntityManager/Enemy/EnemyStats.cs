using System;
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
    [SerializeField] private CharacterAction[] _actions;

    void Start()
    {
        maxHealth = ScaleStat(baseHealth, scaleHealth);
        currentHealth = maxHealth;
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
		if (!StageManager.Instance.isLocalBattle)
		{
        	EnemyManager.Instance.PlayTargetAnimation(
				String.Format("{0}_damage", EnemyManager.Instance.currentEnemy.name.ToLower()), true);
		}
		else
		{
			EnemyManager.Instance.PlayTargetAnimation("damage", true, true);
		}
        
        if (currentHealth <= 0)
        {
            currentHealth = 0;

			EnemyManager.Instance.Die();
			if (!StageManager.Instance.isLocalBattle)
			{
        		EnemyManager.Instance.PlayTargetAnimation(
					String.Format("{0}_death", EnemyManager.Instance.currentEnemy.name.ToLower()), true);
			}
			else
			{
				EnemyManager.Instance.PlayTargetAnimation("death", true, true);
			}

            StageManager.Instance.EndStageWin();
        }
        else if (currentHealth <= ScaleStat(maxHealth, 0.3f))
        {
            // If current HP is lower than 30% of max HP, gain
            // 20% more ATK
            scaleAttack += 0.2f;
            // 10% more critical
            scaleCritical += 0.1f;
            // 30% more defense
            scaleDefense += 0.3f;
        }
    }

    public CharacterAction[] GetActions()
    {
        return _actions;
    }
}
