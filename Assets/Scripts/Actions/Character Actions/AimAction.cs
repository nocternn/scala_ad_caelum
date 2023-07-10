using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Character Action/Aim")]
public class AimAction : CharacterAction
{
    protected override void Awake()
    {
        type = Enums.ActionType.Aim;
    }
    
    public override void PerformAction(PlayerManager player, bool playAnimation = true)
    {
        // If is currently aiming then stop, else start aiming
        player.isAiming = !player.isAiming;
        
        player.aimTarget.gameObject.SetActive(player.isAiming);
        player.rigLayer.enabled = player.isAiming;
        player.rigLayer.gameObject.SetActive(player.isAiming);

        if (player.isAiming)
        {
            player.rigBuilder.Build();
        }
        else
        {
            player.rigBuilder.Clear();
        }
        
        HUDManager.Instance.hudCombat.ToggleCrosshair(player.isAiming);

        base.PerformAction(player, playAnimation);
    }
    
    public override void PerformAction(EnemyManager enemy, bool playAnimation = true)
    {
        // If is currently aiming then stop, else start aiming
        enemy.isAiming = !enemy.isAiming;
        
        enemy.rigLayer.enabled = enemy.isAiming;
        enemy.rigLayer.gameObject.SetActive(enemy.isAiming);
        enemy.rigBuilder.Build();

        base.PerformAction(enemy, playAnimation);
    }
}
