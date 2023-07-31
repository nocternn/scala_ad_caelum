using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectClassVicissitude : EffectClass
{
    private int MaxStacks = 40;
    private int ReducedStacks = 10;
    
    private int _stacks = 0;

    private bool _isRunning = false;

    private float lastBoostPlayerATK = 0;
    private float lastBoostPlayerDEF = 0;
    private float lastBoostEnemy = 0;
    
    public override void Apply(EffectItem card)
    {
        if (!_isRunning)
        {
            _isRunning = true;
            StartCoroutine(AddStacks());
        }
        
        if (PlayerManager.Instance.isHit)
        {
            PlayerManager.Instance.isHit = false;
            ReduceStacks();
        }

        switch (card.id)
        {
            case 1:
                if (canBeApplied_01) LongTrip(card);
                break;
            case 2:
                if (canBeApplied_02) NoOneToShare(card);
                break;
            case 3:
                if (canBeApplied_03) LostAndFound(card);
                break;
            default:
                UnityEngine.Debug.Log("Invalid Vicissitude card ID");
                break;
        }
    }
    
    private void LongTrip(EffectItem card)
    {
        float count = card.count[card.level];
        float duration = card.duration[card.level];
        float scalar = card.scalar[card.level];

        PlayerManager.Instance.stats.scaleAttack -= lastBoostPlayerATK;

        lastBoostPlayerATK = _stacks * (scalar / 100.0f);
        
        PlayerManager.Instance.stats.scaleAttack += lastBoostPlayerATK;
    }
    
    private void NoOneToShare(EffectItem card)
    {
        float count = card.count[card.level];
        float duration = card.duration[card.level];
        float scalar = card.scalar[card.level];
        
        EnemyManager.Instance.stats.scaleDefense += lastBoostEnemy;

        lastBoostEnemy = _stacks * (scalar / 100.0f);
        
        EnemyManager.Instance.stats.scaleDefense -= lastBoostEnemy;
    }
    
    private void LostAndFound(EffectItem card)
    {
        float count = card.count[card.level];
        float duration = card.duration[card.level];
        float scalar = card.scalar[card.level];
        
        PlayerManager.Instance.stats.scaleDefense -= lastBoostPlayerDEF;

        lastBoostPlayerDEF = _stacks * (scalar / 100.0f);
        
        PlayerManager.Instance.stats.scaleDefense += lastBoostPlayerDEF;
    }

    public void ReduceStacks()
    {
        _stacks -= ReducedStacks;
        if (_stacks < 0)
            _stacks = 0;
    }
    
    protected IEnumerator AddStacks()
    {
        while (true)
        {
            _stacks++;
            if (_stacks > MaxStacks)
                _stacks = MaxStacks;
            yield return new WaitForSeconds(1.0f);
        }
    }
}
