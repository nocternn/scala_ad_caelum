using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffectDecimation : CardItemEffect
{
    private int deltaHPPlayer = 0;

    private float lastBoostPlayer = 0;
    private float lastBoostEnemy = 0;
    
    public override void Apply(CardItem card)
    {
        switch (card.id)
        {
            case 1:
                if (canBeApplied_01) BladeGraveAndScar(card);
                break;
            case 2:
                if (canBeApplied_02) PathMisfortuneAndWrittenFate(card);
                break;
            case 3:
                if (canBeApplied_03) DesirelessMindlessAndHomeless(card);
                break;
            default:
                UnityEngine.Debug.Log("Invalid Decimation card ID");
                break;
        }
    }

    private void BladeGraveAndScar(CardItem card)
    {
        float count = card.count[card.level];
        float scalar = card.scalar[card.level];

        int deltaHP = PlayerManager.Instance.stats.maxHealth - PlayerManager.Instance.stats.currentHealth;
        if (deltaHP != deltaHPPlayer)
        {
            float boostPlayer = (deltaHP / count) * (scalar / 100.0f);
            PlayerManager.Instance.stats.scaleAttack -= lastBoostPlayer;
            PlayerManager.Instance.stats.scaleAttack += boostPlayer;
            
            deltaHPPlayer = deltaHP;
            lastBoostPlayer = boostPlayer;
        }
    }
    
    private void PathMisfortuneAndWrittenFate(CardItem card)
    {
        float scalar = card.scalar[card.level];

        canBeApplied_02 = false;

        PlayerManager.Instance.stats.scaleHealth = 1 + (scalar / 100.0f);
        PlayerManager.Instance.stats.UpdateMaxHealth();
    }
    
    private void DesirelessMindlessAndHomeless(CardItem card)
    {
        float count = card.count[card.level];
        float scalar = card.scalar[card.level];
        
        int deltaHP = PlayerManager.Instance.stats.maxHealth - PlayerManager.Instance.stats.currentHealth;
        if (deltaHP != deltaHPPlayer)
        {
            float boostEnemy = (deltaHP / count) * (scalar / 100.0f);
            EnemyManager.Instance.stats.scaleDefense += lastBoostEnemy;
            EnemyManager.Instance.stats.scaleDefense -= boostEnemy;
            
            deltaHPPlayer = deltaHP;
            lastBoostEnemy = boostEnemy;
        }
    }
}
