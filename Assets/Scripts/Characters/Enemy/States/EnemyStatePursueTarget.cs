using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatePursueTarget : EnemyState
{
    [Header("A.I. Settings - Components")]
    public EnemyStateCombatStance combatStanceState;
    
    [Header("A.I. Settings - Movement")]
    public float rotationSpeed = 20;
    
    public override EnemyState Tick(EnemyManager manager)
    {
        manager.UpdateAISettings();
        
        // Chase the target
        HandleMovement(manager);
        HandleRotation(manager);
        manager.ResetNavMeshAgent();
        
        // Switch to combat stance state if within attack range
        if (manager.IsInAttackRange())
            return combatStanceState;
        
        // Continue to chase if out of attack range
        return this;
    }
    
    public void HandleMovement(EnemyManager manager)
    {
        if (manager.isInteracting)
        {
            manager.animatorHandler.UpdateAnimatorValues(0, 0);
            manager.navMeshAgent.enabled = false;
            return;
        }
        
        if (!manager.IsInAttackRange())
            manager.animatorHandler.UpdateAnimatorValues(0, 1);
    }
    
    public void HandleRotation(EnemyManager manager)
    {
        if (manager.isInteracting) // If the enemy is performing an action then rotate manually
        {
            Vector3 direction = manager.currentTarget.transform.position - manager.transform.position;
            direction.y = 0;
            direction.Normalize();

            if (direction == Vector3.zero)
                direction = manager.transform.forward;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            
            manager.transform.rotation = Quaternion.Slerp(manager.transform.rotation, targetRotation,
                rotationSpeed / Time.deltaTime);
        }
        else // If the enemy is not performing an action then rotate with pathfinding
        {
            Vector3 relativeDirection = manager.transform.InverseTransformDirection(manager.navMeshAgent.desiredVelocity);
            Vector3 targetVelocity = manager.rigidbody.velocity;

            manager.navMeshAgent.enabled = true;
            manager.navMeshAgent.SetDestination(manager.currentTarget.transform.position);

            manager.rigidbody.velocity = targetVelocity;
            manager.transform.rotation = Quaternion.Slerp(manager.transform.rotation, manager.navMeshAgent.transform.rotation,
                rotationSpeed / Time.deltaTime);
        }
    }
}
