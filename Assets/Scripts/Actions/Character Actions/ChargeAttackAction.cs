using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Weapon Actions/Charged Attack")]
public class ChargeAttackAction : WeaponAction
{
    protected override void Awake()
    {
        type = Enums.ActionType.Charged;
    }

    public override void PerformAction(PlayerManager player, bool playAnimation = true)
    {
        if (playAnimation)
			player.PlayTargetAnimation(_animation, true);
        
        base.PerformAction(player, playAnimation);
    }
}
