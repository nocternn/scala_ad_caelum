using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private PlayerManager _manager;
    
    [Header("Fixed Stats")]
    public const int MaxCharge = 100;
    public const int StepCharge = 25;
    
    [Header("Base Stats")]
    public int baseSkillPoints;

    [Header("Stats Scale")]
    public float scaleSkillPoints = 1;

    [Header("Skill Points")]
    public int maxSkillPoints;
    public float currentSkillPoints;

    [Header("Attack")]
    public int currentCharge;
    public int hitCount;

    void Start()
    {
        maxHealth = ScaleStat(baseHealth, scaleHealth);
        maxSkillPoints = ScaleStat(baseSkillPoints, scaleSkillPoints);

        currentHealth = maxHealth;
        currentSkillPoints = maxSkillPoints / 3;
    }
    
    public void SetManager(PlayerManager manager)
    {
        _manager = manager;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        _manager.PlayTargetAnimation("damage", true, true);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            _manager.PlayTargetAnimation("death", true, true);
            _manager.Lose();
        }

        _manager.isHit = true;
    }

    public void UpdateAttackCharge(bool isGain)
    {
        if (isGain)
        {
            currentCharge += StepCharge;
            if (IsFullCharge()) // in case currentCharge overflows
                currentCharge = MaxCharge;
        }
        else
        {
            currentCharge = 0;
        }
    }

    public bool IsFullCharge()
    {
        return currentCharge >= MaxCharge;
    }

	public void UpdateMaxHealth()
	{
		maxHealth = ScaleStat(baseHealth, scaleHealth);
	}
}
