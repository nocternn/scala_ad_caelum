using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Random;

public class EnemyStateAttack : EnemyState
{
    public EnemyStateCombatStance combatStanceState;
    public EnemyActionAttack currentAttack;
    
    public override EnemyState Tick(EnemyManager manager)
    {        
        manager.UpdateAISettings();
        
        // Return to combat stance
        if (manager.isInteracting || !manager.IsInAttackRange())
            return combatStanceState;
        
        // Select an attack based on attack scores
        GetNewAttack(manager);
        if (currentAttack == null)
            return this;
        
        // If attack is out of range/angle, select a new attack
        if (manager.distanceFromTarget < currentAttack.minDistance || manager.distanceFromTarget > currentAttack.maxDistance)
            return this;
        if (manager.viewableAngle < currentAttack.minAngle || manager.viewableAngle > currentAttack.maxAngle)
            return this;
        
        // Stop movement and attack target
        if (manager.currentRecoveryTime <= 0 && !manager.isInteracting)
        {
            AttackTarget(manager);
            return combatStanceState;
        }

        return this;
    }
    
    private void GetNewAttack(EnemyManager manager)
    {
        int maxScore = 0;
        foreach (EnemyActionAttack attack in manager.stats.enemyAttacks)
        {
            if ((manager.distanceFromTarget >= attack.minDistance)
                && (manager.distanceFromTarget <= attack.maxDistance)
                && (manager.viewableAngle >= attack.minAngle && manager.viewableAngle <= attack.maxAngle))
            {
                maxScore += attack.score;
            }
        }

        int randomScore = Range(0, maxScore);
        int tempScore = 0;
        
        foreach (EnemyActionAttack attack in manager.stats.enemyAttacks)
        {
            if ((manager.distanceFromTarget >= attack.minDistance)
                && (manager.distanceFromTarget <= attack.maxDistance)
                && (manager.viewableAngle >= attack.minAngle && manager.viewableAngle <= attack.maxAngle))
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
        manager.currentRecoveryTime = currentAttack.recoveryTime;
        currentAttack = null;
    }
}
