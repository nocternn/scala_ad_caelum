using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CardEffectDeliverance : CardItemEffect
{
    public override void Apply(CardItem card)
    {
        switch (card.id)
        {
            case 1:
                if (canBeApplied_01) GobletOfTheGiver(card);
                break;
            case 2:
                if (canBeApplied_02) RochetOfThePilgrim(card);
                break;
            case 3:
                if (canBeApplied_03) MaskOfThePredator(card);
                break;
            default:
                UnityEngine.Debug.Log("Invalid Deliverance card ID");
                break;
        }
    }

    private void GobletOfTheGiver(CardItem card)
    {
        float duration = card.duration[card.level];
        float scalar = card.scalar[card.level];

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
    
    private void RochetOfThePilgrim(CardItem card)
    {
        float count = card.count[card.level];
        float duration = card.scalar[card.level];
        
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
    
    private void MaskOfThePredator(CardItem card)
    {
        float duration = card.duration[card.level];
        float scalar = card.scalar[card.level];
        
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
