using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CardEffectDeliverance : CardItemEffect
{
    public override void Apply(CardItem card, PlayerManager player = null, EnemyManager enemy = null)
    {
        if (!canBeApplied)
            return;

        switch (card.id)
        {
            case 1:
                GobletOfTheGiver(card, player);
                break;
            case 2:
                RochetOfThePilgrim(card, player);
                break;
            case 3:
                MaskOfThePredator(card, player, enemy);
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
            canBeApplied = false;

            player.stats.scaleAttack += scalar / 100.0f;

            Task.Delay((int)Mathf.Round(1000 * duration)).ContinueWith(t => 
            {
                player.stats.scaleAttack -= scalar / 100.0f;
                canBeApplied = true;
            });
        }
    }
    
    private void RochetOfThePilgrim(CardItem card, PlayerManager player)
    {
        float count = card.count[card.level];
        float duration = card.scalar[card.level];
        
        if (player.inputHandler.attackUltimateInput)
        {
            canBeApplied = false;

            IEnumerator addSP = AddSkillPoints(player, count, (int)Mathf.Round(duration));
            StartCoroutine(addSP);

            Task.Delay((int)Mathf.Round(1000 * duration)).ContinueWith(t =>
            {
                StopCoroutine(addSP);
                canBeApplied = true;
            });
        }
    }
    
    private void MaskOfThePredator(CardItem card, PlayerManager player, EnemyManager enemy)
    {
        float duration = card.duration[card.level];
        float scalar = card.scalar[card.level];
        
        if (player.inputHandler.attackUltimateInput)
        {
            canBeApplied = false;

            enemy.stats.scaleDefense -= scalar / 100.0f;

            Task.Delay((int)Mathf.Round(1000 * duration)).ContinueWith(t => 
            {
                canBeApplied = true;
            });
        }
    }
}
