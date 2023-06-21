using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CardEffectDeliverance : CardItemEffect
{
    public override void Apply(CardItem card, PlayerManager player = null, EnemyManager enemy = null)
    {
        switch (card.id)
        {
            case 1:
                if (canBeApplied_01) GobletOfTheGiver(card, player);
                break;
            case 2:
                if (canBeApplied_02) RochetOfThePilgrim(card, player);
                break;
            case 3:
                if (canBeApplied_03) MaskOfThePredator(card, player, enemy);
                break;
            default:
                UnityEngine.Debug.Log("Invalid Deliverance card ID");
                break;
        }
    }

    private void GobletOfTheGiver(CardItem card, PlayerManager player)
    {
        float duration = card.duration[card.level];
        float scalar = card.scalar[card.level];

        if (player.inputHandler.attackUltimateInput)
        {
            canBeApplied_01 = false;

            player.stats.scaleAttack += scalar / 100.0f;

            Task.Delay((int)Mathf.Round(1000 * duration)).ContinueWith(t => 
            {
                player.stats.scaleAttack -= scalar / 100.0f;
                canBeApplied_01 = true;
            });
        }
    }
    
    private void RochetOfThePilgrim(CardItem card, PlayerManager player)
    {
        float count = card.count[card.level];
        float duration = card.scalar[card.level];
        
        if (player.inputHandler.attackUltimateInput)
        {
            canBeApplied_02 = false;

            IEnumerator addSP = AddSkillPoints(player, count, (int)Mathf.Round(duration));
            StartCoroutine(addSP);

            Task.Delay((int)Mathf.Round(1000 * duration)).ContinueWith(t =>
            {
                StopCoroutine(addSP);
                canBeApplied_02 = true;
            });
        }
    }
    
    private void MaskOfThePredator(CardItem card, PlayerManager player, EnemyManager enemy)
    {
        float duration = card.duration[card.level];
        float scalar = card.scalar[card.level];
        
        if (player.inputHandler.attackUltimateInput)
        {
            canBeApplied_03 = false;

            enemy.stats.scaleDefense -= scalar / 100.0f;

            Task.Delay((int)Mathf.Round(1000 * duration)).ContinueWith(t => 
            {
                canBeApplied_03 = true;
            });
        }
    }
}
