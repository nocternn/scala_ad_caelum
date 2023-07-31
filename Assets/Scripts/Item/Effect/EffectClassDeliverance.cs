using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EffectClassDeliverance : EffectClass
{
    public override void Apply(EffectItem effect)
    {
        switch (effect.id)
        {
            case 1:
                if (canBeApplied_01) GobletOfTheGiver(effect);
                break;
            case 2:
                if (canBeApplied_02) RochetOfThePilgrim(effect);
                break;
            case 3:
                if (canBeApplied_03) MaskOfThePredator(effect);
                break;
            default:
                UnityEngine.Debug.Log("Invalid Deliverance effect ID");
                break;
        }
    }

    private void GobletOfTheGiver(EffectItem effect)
    {
        float duration = effect.duration[effect.level];
        float scalar = effect.scalar[effect.level];

        if (PlayerManager.Instance.inputHandler.attackUltimateInput)
        {
            canBeApplied_01 = false;

            PlayerManager.Instance.stats.scaleAttack += scalar / 100.0f;

            Task.Delay((int)Mathf.Round(1000 * duration)).ContinueWith(t => 
            {
                PlayerManager.Instance.stats.scaleAttack -= scalar / 100.0f;
                canBeApplied_01 = true;
            });
        }
    }
    
    private void RochetOfThePilgrim(EffectItem effect)
    {
        float count = effect.count[effect.level];
        float duration = effect.duration[effect.level];
        
        if (PlayerManager.Instance.inputHandler.attackUltimateInput)
        {
            canBeApplied_02 = false;

            StartCoroutine(AddSkillPoints(count, (int)Mathf.Round(duration)));

            Task.Delay((int)Mathf.Round(1000 * duration)).ContinueWith(t =>
            {
                canBeApplied_02 = true;
            });
        }
    }
    
    private void MaskOfThePredator(EffectItem effect)
    {
        float duration = effect.duration[effect.level];
        float scalar = effect.scalar[effect.level];
        
        if (PlayerManager.Instance.inputHandler.attackUltimateInput)
        {
            canBeApplied_03 = false;

            EnemyManager.Instance.stats.scaleDefense -= scalar / 100.0f;

            Task.Delay((int)Mathf.Round(1000 * duration)).ContinueWith(t => 
            {
                EnemyManager.Instance.stats.scaleDefense += scalar / 100.0f;
                canBeApplied_03 = true;
            });
        }
    }
}
