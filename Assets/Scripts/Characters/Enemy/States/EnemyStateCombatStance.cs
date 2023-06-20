using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateCombatStance : EnemyState
{
    [Header("A.I. Settings - Components")]
    [SerializeField] private EnemyStateIdle idleState;
    [SerializeField] private EnemyStatePursueTarget pursueTargetState;
    [SerializeField] private EnemyStateAttack attackState;

    [Header("A.I. Settings - Combat Stance")]
    private float distanceFromTarget;
    private float viewableAngle;
    
    public override EnemyState Tick(EnemyManager manager)
    {
        Vector3 direction = manager.currentTarget.transform.position - manager.transform.position;
        distanceFromTarget = Vector3.Distance(manager.currentTarget.transform.position, manager.transform.position);
        viewableAngle = Vector3.Angle(direction, manager.transform.forward);

        if (manager.isInteracting)
        {
            return pursueTargetState;
        }

        if (manager.stats.currentRecoveryTime <= 0)
        {
            // Switch to attack state if in attack range
            if (distanceFromTarget < manager.stats.maxAttackRange &&
                (viewableAngle >= idleState.minDetectionAngle && viewableAngle <= idleState.maxDetectionAngle))
            {
                return attackState;
            }
            // Switch to pursue state if out of attack range
            return pursueTargetState;
        }

        return this;
    }
}
