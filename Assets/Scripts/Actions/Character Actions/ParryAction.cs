using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Character Action/Parry")]
public class ParryAction : CharacterAction
{
    protected override void Awake()
    {
        type = Enums.ActionType.Parry;
    }
    
    public override void PerformAction(EnemyManager enemy, bool playAnimation = true)
    {
        if (enemy.isInteracting)
            return;

        enemy.animatorHandler.PlayTargetAnimation(_animation, true);
        
        if (!enemy.isHit)
            return;
        
        Task.Delay((int)Mathf.Round(1000 * _duration)).ContinueWith(t =>
        {
            if (enemy.weaponSlotManager.GetCurrentWeapon().combatType == Enums.WeaponCombatType.Ranged)
            {
                enemy.PerformAction(Enums.ActionType.Shoot);
            }
            else
            {
                enemy.PerformAction(Enums.ActionType.Basic);
            }
        });
        
        base.PerformAction(enemy, playAnimation);
    }
}
