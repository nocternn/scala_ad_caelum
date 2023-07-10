using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateAttack : EnemyState
{
    [Header("A.I. Settings - Components")]
    public WeaponAction currentAttack;
    [SerializeField] private EnemyStatePursueTarget pursueTargetState;
    [SerializeField] private EnemyStateCombatStance combatStanceState;
    
    [Header("A.I. Settings - Attack")]
    public bool hasPerformedAttack;
    
    public override EnemyState Tick(EnemyManager manager)
    {
        pursueTargetState.HandleRotation(manager);

        if (_distanceFromTarget > manager.stats.maxAttackRange)
        {
            hasPerformedAttack = false;
            return pursueTargetState;
        }

        if (!hasPerformedAttack)
        {
            AttackTarget(manager);
            hasPerformedAttack = true;
        }

        return pursueTargetState;
    }

    private void AttackTarget(EnemyManager manager)
    {
        manager.isInteracting = true;
        manager.animatorHandler.UpdateAnimatorValues(0, 0);
        currentAttack.PerformAction(manager);
        currentAttack = null;
    }
}
