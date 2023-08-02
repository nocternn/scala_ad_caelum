using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatePursueTarget : EnemyState
{
    [Header("A.I. Settings - Components")]
    [SerializeField] private EnemyStateIdle idleState;
    [SerializeField] private EnemyStateCombatStance combatStanceState;
    
    protected override void Awake()
    {
        idleState = GetComponent<EnemyStateIdle>();
        combatStanceState = GetComponent<EnemyStateCombatStance>();
    }
    
    public override EnemyState Tick()
    {
        // Chase the target
        if (!EnemyManager.Instance.isInteracting)
        {
            HandleMovement();
        }
        HandleRotation();
        
        // Switch to combat stance state if within attack range
        if (!EnemyManager.Instance.isInteracting && _distanceFromTarget <= EnemyManager.Instance.stats.maxAttackRange)
            return combatStanceState;
        
        // Continue to chase if out of attack range
        return this;
    }
    
    public void HandleMovement()
    {
        if (_distanceFromTarget > EnemyManager.Instance.stats.maxAttackRange)
        {
            EnemyManager.Instance.animatorHandler.UpdateAnimatorValues(0, 1);
        }
        else
        {
            EnemyManager.Instance.animatorHandler.UpdateAnimatorValues(0, 0);
        }
    }
    
    public void HandleRotation()
    {
        if (EnemyManager.Instance.isInteracting) // If the enemy is performing an action then rotate manually
        {
            Vector3 direction = 
                EnemyManager.Instance.currentTarget.transform.position - EnemyManager.Instance.transform.position;
            direction.y = 0;
            direction.Normalize();

            if (direction == Vector3.zero)
                direction = EnemyManager.Instance.transform.forward;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            EnemyManager.Instance.transform.rotation = Quaternion.Slerp(
                EnemyManager.Instance.transform.rotation, targetRotation,
                EnemyManager.Instance.locomotion.rotationSpeed / Time.deltaTime);
        }
        else // If the enemy is not performing an action then rotate with pathfinding
        {
            Vector3 relativeDirection = EnemyManager.Instance.transform.InverseTransformDirection(
                EnemyManager.Instance.navMeshAgent.desiredVelocity);
            Vector3 targetVelocity = EnemyManager.Instance.rigidbody.velocity;

            EnemyManager.Instance.navMeshAgent.enabled = true;
            EnemyManager.Instance.navMeshAgent.SetDestination(EnemyManager.Instance.currentTarget.transform.position);

            EnemyManager.Instance.rigidbody.velocity = targetVelocity;
            EnemyManager.Instance.transform.rotation = Quaternion.Slerp(
                EnemyManager.Instance.transform.rotation, EnemyManager.Instance.navMeshAgent.transform.rotation,
                EnemyManager.Instance.locomotion.rotationSpeed / Time.deltaTime);
        }
        EnemyManager.Instance.ResetNavMeshAgent();
    }
}
