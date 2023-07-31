using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EffectClass : MonoBehaviour
{
    public bool canBeApplied_01 = true;
    public bool canBeApplied_02 = true;
    public bool canBeApplied_03 = true;

    public virtual void Apply(EffectItem card)
    {
        
    }
    
    protected IEnumerator AddSkillPoints(float count, int duration = 0)
    {
        if (duration > 0)
        {
            for (int i = 1; i <= duration; i++)
            {
                AddSkillPoints(count);
                yield return new WaitForSeconds(1.0f);
            }
        }
        else
        {
            for (;;)
            {
                AddSkillPoints(count);
                yield return new WaitForSeconds(1.0f);
            }
        }
    }

    private void AddSkillPoints(float count)
    {
        PlayerManager.Instance.stats.currentSkillPoints += count;
        if (PlayerManager.Instance.stats.currentSkillPoints >= PlayerManager.Instance.stats.maxSkillPoints)
        {
            PlayerManager.Instance.stats.currentSkillPoints = PlayerManager.Instance.stats.maxSkillPoints;
        }
    }
}