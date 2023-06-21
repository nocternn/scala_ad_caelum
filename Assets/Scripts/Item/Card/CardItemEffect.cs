using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardItemEffect : MonoBehaviour
{
    public bool canBeApplied_01 = true;
    public bool canBeApplied_02 = true;
    public bool canBeApplied_03 = true;

    public virtual void Apply(CardItem card, PlayerManager player = null, EnemyManager enemy = null)
    {
        
    }
    
    protected IEnumerator AddSkillPoints(PlayerManager player, float count, int duration = 0)
    {
        if (duration > 0)
        {
            for (int i = 1; i <= duration; i++)
            {
                AddSkillPoints(player, count);
                yield return new WaitForSeconds(1.0f);
            }
        }
        else
        {
            for (;;)
            {
                AddSkillPoints(player, count);
                yield return new WaitForSeconds(1.0f);
            }
        }
    }

    private void AddSkillPoints(PlayerManager player, float count)
    {
        player.stats.currentSkillPoints += count;
        if (player.stats.currentSkillPoints >= player.stats.maxSkillPoints)
        {
            player.stats.currentSkillPoints = player.stats.maxSkillPoints;
        }
    }
}