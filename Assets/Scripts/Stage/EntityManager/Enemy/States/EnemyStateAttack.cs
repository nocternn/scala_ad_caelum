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
    
    protected override void Awake()
    {
        pursueTargetState = GetComponent<EnemyStatePursueTarget>();
        combatStanceState = GetComponent<EnemyStateCombatStance>();
    }
    
    public override EnemyState Tick()
    {
        pursueTargetState.HandleRotation();

        if (_distanceFromTarget > EnemyManager.Instance.stats.maxAttackRange)
        {
            hasPerformedAttack = false;
            return pursueTargetState;
        }

        if (!hasPerformedAttack)
        {
            AttackTarget();
            hasPerformedAttack = true;
        }

        return pursueTargetState;
    }

    private void AttackTarget()
    {
        EnemyManager.Instance.isInteracting = true;
        EnemyManager.Instance.animatorHandler.UpdateAnimatorValues(0, 0);
        currentAttack.PerformAction(EnemyManager.Instance);
        currentAttack = null;
    }
}
