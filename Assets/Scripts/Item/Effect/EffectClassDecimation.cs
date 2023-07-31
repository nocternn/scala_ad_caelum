using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectClassDecimation : EffectClass
{
    private int deltaHPPlayer = 0;

    private float lastBoostPlayer = 0;
    private float lastBoostEnemy = 0;
    
    public override void Apply(EffectItem effect)
    {
        switch (effect.id)
        {
            case 1:
                if (canBeApplied_01) BladeGraveAndScar(effect);
                break;
            case 2:
                if (canBeApplied_02) PathMisfortuneAndWrittenFate(effect);
                break;
            case 3:
                if (canBeApplied_03) DesirelessMindlessAndHomeless(effect);
                break;
            default:
                UnityEngine.Debug.Log("Invalid Decimation effect ID");
                break;
        }
    }

    private void BladeGraveAndScar(EffectItem effect)
    {
        float count = effect.count[effect.level];
        float scalar = effect.scalar[effect.level];

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
    
    private void PathMisfortuneAndWrittenFate(EffectItem effect)
    {
        float scalar = effect.scalar[effect.level];

        canBeApplied_02 = false;
        
        PlayerManager.Instance.stats.scaleHealth = 1 + (scalar / 100.0f);
        PlayerManager.Instance.stats.UpdateMaxHealth();
    }
    
    private void DesirelessMindlessAndHomeless(EffectItem effect)
    {
        float count = effect.count[effect.level];
        float scalar = effect.scalar[effect.level];
        
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
