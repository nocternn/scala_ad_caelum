using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CardEffectGold : CardItemEffect
{
    private float deltaSPPlayer = 0;

    private float lastBoostATK = 0;
    private float lastBoostDEF = 0;

    public override void Apply(CardItem card, PlayerManager player = null, EnemyManager enemy = null)
    {
        switch (card.id)
        {
            case 1:
                if (canBeApplied_01) RecitatifOfEden(card, player);
                break;
            case 2:
                if (canBeApplied_02) RecitatifOfBirds(card, player);
                break;
            case 3:
                if (canBeApplied_03) RecitatifOfGoodWine(card, player);
                break;
            default:
                UnityEngine.Debug.Log("Invalid Gold card ID");
                break;
        }
    }
    
    private void RecitatifOfEden(CardItem card, PlayerManager player)
    {
        float count = card.count[card.level];
        float scalar = card.scalar[card.level];
        
        int deltaSP = player.stats.maxSkillPoints - player.stats.maxSkillPoints;
        if (deltaSP != deltaSPPlayer)
        {
            float boost = (deltaSP / count) * (scalar / 100.0f);
            player.stats.scaleAttack -= lastBoostATK;
            player.stats.scaleAttack += boost;
            
            deltaSPPlayer = deltaSP;
            lastBoostATK = boost;
        }
    }
    
    private void RecitatifOfBirds(CardItem card, PlayerManager player)
    {
        float count = card.count[card.level];
        float scalar = card.scalar[card.level];
        
        int deltaSP = player.stats.maxSkillPoints - player.stats.maxSkillPoints;
        if (deltaSP != deltaSPPlayer)
        {
            float boost = (deltaSP / count) * (scalar / 100.0f);
            player.stats.scaleDefense -= lastBoostDEF;
            player.stats.scaleDefense += boost;
            
            deltaSPPlayer = deltaSP;
            lastBoostDEF = boost;
        }
    }
    
    private void RecitatifOfGoodWine(CardItem card, PlayerManager player)
    {
        float count = card.count[card.level];

        canBeApplied_03 = false;
        
        StartCoroutine(AddSkillPoints(player, count));
    }
}
