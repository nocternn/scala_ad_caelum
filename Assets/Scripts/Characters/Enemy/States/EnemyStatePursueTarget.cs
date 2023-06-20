using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatePursueTarget : EnemyState
{
    [Header("A.I. Settings - Components")]
    [SerializeField] private EnemyStateIdle idleState;
    [SerializeField] private EnemyStateCombatStance combatStanceState;
    
    [Header("A.I. Settings - Movement")]
    private float distanceFromTarget;
    private float viewableAngle;
    
    public override EnemyState Tick(EnemyManager manager)
    {
        Vector3 direction = manager.currentTarget.transform.position - manager.transform.position;
        distanceFromTarget = Vector3.Distance(manager.currentTarget.transform.position, manager.transform.position);
        viewableAngle = Vector3.Angle(direction, manager.transform.forward);
        
        // Chase the target
        if (!manager.isInteracting)
        {
            HandleMovement(manager);
        }
        HandleRotation(manager);
        manager.ResetNavMeshAgent();
        
        // Switch to combat stance state if within attack range
        if (!manager.isInteracting && distanceFromTarget <= manager.stats.maxAttackRange)
            return combatStanceState;
        
        // Continue to chase if out of attack range
        return this;
    }
    
    public void HandleMovement(EnemyManager manager)
    {
        if (distanceFromTarget > manager.stats.maxAttackRange)
        {
            manager.animatorHandler.UpdateAnimatorValues(0, 1);
        }
        else
        {
            manager.animatorHandler.UpdateAnimatorValues(0, 0);
        }
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
                manager.locomotion.rotationSpeed / Time.deltaTime);
        }
        else // If the enemy is not performing an action then rotate with pathfinding
        {
            Vector3 relativeDirection = manager.transform.InverseTransformDirection(manager.navMeshAgent.desiredVelocity);
            Vector3 targetVelocity = manager.rigidbody.velocity;

            manager.navMeshAgent.enabled = true;
            manager.navMeshAgent.SetDestination(manager.currentTarget.transform.position);

            manager.rigidbody.velocity = targetVelocity;
            manager.transform.rotation = Quaternion.Slerp(manager.transform.rotation, manager.navMeshAgent.transform.rotation,
                manager.locomotion.rotationSpeed / Time.deltaTime);
        }
    }
}
