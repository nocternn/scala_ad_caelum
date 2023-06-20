using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Random;

public class EnemyStateAttack : EnemyState
{
    [Header("A.I. Settings - Components")]
    [SerializeField] private EnemyStatePursueTarget pursueTargetState;
    [SerializeField] private EnemyStateCombatStance combatStanceState;
    [SerializeField] private EnemyActionAttack currentAttack;
    
    [Header("A.I. Settings - Attack")]
    private float distanceFromTarget;
    private float viewableAngle;
    
    public override EnemyState Tick(EnemyManager manager)
    {   
        // Return to combat stance
        if (manager.isInteracting)
            return combatStanceState;

        // Select an attack based on attack scores
        if (currentAttack != null)
        {   
            Vector3 direction = manager.currentTarget.transform.position - manager.transform.position;
            distanceFromTarget = Vector3.Distance(manager.currentTarget.transform.position, manager.transform.position);
            viewableAngle = Vector3.Angle(direction, manager.transform.forward);

            // If attack is out of range/angle, select a new attack
            if (distanceFromTarget < currentAttack.minDistance)
                return this;
            // If attack is in range
            if (distanceFromTarget < currentAttack.maxDistance)
            {
                // If target is within viewable angle then attack
                if (viewableAngle >= currentAttack.minAngle && viewableAngle <= currentAttack.maxAngle)
                {
                    // If not recovering then stop movement and attack target
                    if (manager.stats.currentRecoveryTime <= 0 && !manager.isInteracting)
                    {
                        AttackTarget(manager);
                        return combatStanceState;
                    }
                }
            }
        }
        else
        {
            GetNewAttack(manager);
        }

        return combatStanceState;
    }
    
    private void GetNewAttack(EnemyManager manager)
    {
        int maxScore = 0;
        foreach (EnemyActionAttack attack in manager.stats.enemyAttacks)
        {
            if ((distanceFromTarget >= attack.minDistance)
                && (distanceFromTarget <= attack.maxDistance)
                && (viewableAngle >= attack.minAngle && viewableAngle <= attack.maxAngle))
            {
                maxScore += attack.score;
            }
        }

        int randomScore = Range(0, maxScore);
        int tempScore = 0;
        
        foreach (EnemyActionAttack attack in manager.stats.enemyAttacks)
        {
            if ((distanceFromTarget >= attack.minDistance)
                && (distanceFromTarget <= attack.maxDistance)
                && (viewableAngle >= attack.minAngle && viewableAngle <= attack.maxAngle))
            {
                if (currentAttack != null)
                    return;

                tempScore += attack.score;
                if (tempScore > randomScore)
                    currentAttack = attack;
            }
        }
    }

    private void AttackTarget(EnemyManager manager)
    {
        manager.isInteracting = true;
        manager.animatorHandler.UpdateAnimatorValues(0, 0);
        manager.animatorHandler.PlayTargetAnimation(currentAttack.animation, true);
        manager.stats.currentRecoveryTime = currentAttack.recoveryTime;
        currentAttack = null;
    }
}
