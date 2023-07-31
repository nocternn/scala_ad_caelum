using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Character Action/Block")]
public class BlockAction : CharacterAction
{
    [Header("Specific Properties")]
    [SerializeField] private float _scaleDef;
    
    protected override void Awake()
    {
        type = Enums.CharacterActionType.Block;
    }
    
    public override void PerformAction(EnemyManager enemy, bool playAnimation = true)
    {
        if (enemy.isInteracting || enemy.isBlocking)
            return;

        enemy.stats.scaleDefense += _scaleDef;
        
        enemy.animatorHandler.PlayTargetAnimation(_animation, true);
        enemy.animatorHandler.SetBool("isBlocking", true);
        
        Task.Delay((int)Mathf.Round(1000 * _duration)).ContinueWith(t =>
        {
            enemy.stats.scaleDefense -= _scaleDef;
            enemy.animatorHandler.SetBool("isBlocking", false);
        }, TaskScheduler.FromCurrentSynchronizationContext());

        base.PerformAction(enemy, playAnimation);
    }
}