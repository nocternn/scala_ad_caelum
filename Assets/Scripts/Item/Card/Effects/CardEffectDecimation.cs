using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffectDecimation : CardItemEffect
{
    private int deltaHPPlayer = 0;

    private float lastBoostPlayer = 0;
    private float lastBoostEnemy = 0;
    
    public override void Apply(CardItem card, PlayerManager player = null, EnemyManager enemy = null)
    {
        switch (card.id)
        {
            case 1:
                if (canBeApplied_01) BladeGraveAndScar(card, player);
                break;
            case 2:
                if (canBeApplied_02) PathMisfortuneAndWrittenFate(card, player);
                break;
            case 3:
                if (canBeApplied_03) DesirelessMindlessAndHomeless(card, player, enemy);
                break;
            default:
                UnityEngine.Debug.Log("Invalid Decimation card ID");
                break;
        }
    }

    private void BladeGraveAndScar(CardItem card, PlayerManager player)
    {
        float count = card.count[card.level];
        float scalar = card.scalar[card.level];

        int deltaHP = player.stats.maxHealth - player.stats.currentHealth;
        if (deltaHP != deltaHPPlayer)
        {
            float boostPlayer = (deltaHP / count) * (scalar / 100.0f);
            player.stats.scaleHealth -= lastBoostPlayer;
            player.stats.scaleHealth += boostPlayer;
            
            deltaHPPlayer = deltaHP;
            lastBoostPlayer = boostPlayer;
        }
    }
    
    private void PathMisfortuneAndWrittenFate(CardItem card, PlayerManager player)
    {
        float scalar = card.scalar[card.level];

        canBeApplied_02 = false;

        player.stats.scaleHealth = 1 + (scalar / 100.0f);
        player.stats.UpdateMaxHealth();
    }
    
    private void DesirelessMindlessAndHomeless(CardItem card, PlayerManager player, EnemyManager enemy)
    {
        float count = card.count[card.level];
        float scalar = card.scalar[card.level];
        
        int deltaHP = player.stats.maxHealth - player.stats.currentHealth;
        if (deltaHP != deltaHPPlayer)
        {
            float boostEnemy = (deltaHP / count) * (scalar / 100.0f);
            player.stats.scaleHealth += lastBoostEnemy;
            player.stats.scaleHealth -= boostEnemy;
            
            deltaHPPlayer = deltaHP;
            lastBoostEnemy = boostEnemy;
        }
    }
}
