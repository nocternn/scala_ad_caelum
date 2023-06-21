using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffectVicissitude : CardItemEffect
{
    private int MaxStacks = 40;
    private int ReducedStacks = 10;
    
    private int _stacks = 0;

    private bool _isRunning = false;

    private float lastBoostPlayerATK = 0;
    private float lastBoostPlayerDEF = 0;
    private float lastBoostEnemy = 0;
    
    public override void Apply(CardItem card, PlayerManager player = null, EnemyManager enemy = null)
    {
        if (!_isRunning)
        {
            _isRunning = true;
            StartCoroutine(AddStacks());
        }
        
        if (player.isHit)
        {
            player.isHit = false;
            ReduceStacks();
        }

        switch (card.id)
        {
            case 1:
                if (canBeApplied_01) LongTrip(card, player);
                break;
            case 2:
                if (canBeApplied_02) NoOneToShare(card, enemy);
                break;
            case 3:
                if (canBeApplied_03) LostAndFound(card, player);
                break;
            default:
                UnityEngine.Debug.Log("Invalid Vicissitude card ID");
                break;
        }
    }
    
    private void LongTrip(CardItem card, PlayerManager player)
    {
        float count = card.count[card.level];
        float duration = card.duration[card.level];
        float scalar = card.scalar[card.level];

        player.stats.scaleAttack -= lastBoostPlayerATK;

        lastBoostPlayerATK = _stacks * (scalar / 100.0f);
        
        player.stats.scaleAttack += lastBoostPlayerATK;
    }
    
    private void NoOneToShare(CardItem card, EnemyManager enemy)
    {
        float count = card.count[card.level];
        float duration = card.duration[card.level];
        float scalar = card.scalar[card.level];
        
        enemy.stats.scaleDefense += lastBoostEnemy;

        lastBoostEnemy = _stacks * (scalar / 100.0f);
        
        enemy.stats.scaleDefense -= lastBoostEnemy;
    }
    
    private void LostAndFound(CardItem card, PlayerManager player)
    {
        float count = card.count[card.level];
        float duration = card.duration[card.level];
        float scalar = card.scalar[card.level];
        
        player.stats.scaleDefense -= lastBoostPlayerDEF;

        lastBoostPlayerDEF = _stacks * (scalar / 100.0f);
        
        player.stats.scaleDefense += lastBoostPlayerDEF;
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
