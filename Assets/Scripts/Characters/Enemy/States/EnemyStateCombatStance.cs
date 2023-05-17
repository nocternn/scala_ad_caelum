using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateCombatStance : EnemyState
{
    [Header("A.I. Settings - Components")]
    public EnemyStatePursueTarget pursueTargetState;
    public EnemyStateAttack attackState;
    
    public override EnemyState Tick(EnemyManager manager)
    {
        manager.UpdateAISettings();

        if (manager.currentRecoveryTime > 0)
        {
            return this;
        }
        
        // If player is out of range then return to pursue state
        if (!manager.IsInAttackRange())
            return pursueTargetState;
        
        // Switch to attack state if in attack range
        return attackState;
    }
}
