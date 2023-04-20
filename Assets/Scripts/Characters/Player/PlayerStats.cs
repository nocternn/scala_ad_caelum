using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private PlayerManager _playerManager;
    
    [Header("Fixed Stats")]
    public const int MaxCharge = 100;
    public const int StepCharge = 25;
    
    [Header("Base Stats")]
    public int baseSkillPoints;

    [Header("Stats Scale")]
    public int scaleSkillPoints = 1;

    [Header("Skill Points")]
    public int maxSkillPoints;
    public int currentSkillPoints;

    [Header("Attack Charge")]
    public int currentCharge;

    void Start()
    {
        maxHealth = baseHealth * scaleHealth;
        maxSkillPoints = baseSkillPoints * scaleSkillPoints;

        currentHealth = maxHealth;
        currentSkillPoints = maxSkillPoints;

        _playerManager.UpdateUI("player_health", currentHealth, maxHealth);
        _playerManager.UpdateUI("skill", currentSkillPoints, maxSkillPoints);
        _playerManager.UpdateUI("charge", currentCharge, MaxCharge);
    }
    
    public void SetManager(PlayerManager manager)
    {
        _playerManager = manager;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        _playerManager.UpdateUI("player_health", currentHealth, maxHealth);
        _playerManager.PlayTargetAnimation("damage", true, true);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            _playerManager.PlayTargetAnimation("death", true, true);
        }
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
        _playerManager.UpdateUI("charge", currentCharge, MaxCharge);
    }

    public bool IsFullCharge()
    {
        return currentCharge >= MaxCharge;
    }
}
