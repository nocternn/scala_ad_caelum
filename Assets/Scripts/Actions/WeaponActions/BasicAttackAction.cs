using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Weapon Actions/Basic Attack")]
public class BasicAttackAction : WeaponAction
{
    protected override void Awake()
    {
        type = Enums.WeaponActionType.Basic;
    }
    
    public override void PerformAction(PlayerManager player, bool playAnimation = true)
    {
        if (playAnimation)
            player.PlayTargetAnimation(_animation, true);
        
        base.PerformAction(player, playAnimation);
    }
    
    public override void PerformAction(EnemyManager enemy, bool playAnimation = true)
    {
        if (playAnimation)
            enemy.animatorHandler.PlayTargetAnimation(_animation, true);
        
        base.PerformAction(enemy, playAnimation);
    }
}
