using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EffectClassGold : EffectClass
{
    private float deltaSPPlayer = 0;

    private float lastBoostATK = 0;
    private float lastBoostDEF = 0;

    public override void Apply(EffectItem card)
    {
        switch (card.id)
        {
            case 1:
                if (canBeApplied_01) RecitatifOfEden(card);
                break;
            case 2:
                if (canBeApplied_02) RecitatifOfBirds(card);
                break;
            case 3:
                if (canBeApplied_03) RecitatifOfGoodWine(card);
                break;
            default:
                UnityEngine.Debug.Log("Invalid Gold card ID");
                break;
        }
    }
    
    private void RecitatifOfEden(EffectItem card)
    {
        float count = card.count[card.level];
        float scalar = card.scalar[card.level];
        
        int deltaSP = PlayerManager.Instance.stats.maxSkillPoints - PlayerManager.Instance.stats.maxSkillPoints;
        if (deltaSP != deltaSPPlayer)
        {
            float boost = (deltaSP / count) * (scalar / 100.0f);
            PlayerManager.Instance.stats.scaleAttack -= lastBoostATK;
            PlayerManager.Instance.stats.scaleAttack += boost;
            
            deltaSPPlayer = deltaSP;
            lastBoostATK = boost;
        }
    }
    
    private void RecitatifOfBirds(EffectItem card)
    {
        float count = card.count[card.level];
        float scalar = card.scalar[card.level];
        
        int deltaSP = PlayerManager.Instance.stats.maxSkillPoints - PlayerManager.Instance.stats.maxSkillPoints;
        if (deltaSP != deltaSPPlayer)
        {
            float boost = (deltaSP / count) * (scalar / 100.0f);
            PlayerManager.Instance.stats.scaleDefense -= lastBoostDEF;
            PlayerManager.Instance.stats.scaleDefense += boost;
            
            deltaSPPlayer = deltaSP;
            lastBoostDEF = boost;
        }
    }
    
    private void RecitatifOfGoodWine(EffectItem card)
    {
        float count = card.count[card.level];

        canBeApplied_03 = false;
        
        StartCoroutine(AddSkillPoints(count));
    }
}
