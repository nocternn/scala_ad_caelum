using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLocomotion : CharacterLocomotion
{
    private EnemyManager _manager;

    private NavMeshAgent _navMeshAgent;

    public CharacterManager currentTarget;
    public LayerMask detectionLayer;
    public new Rigidbody rigidbody;

    public float distanceFromTarget;

    private void Awake()
    {
        _navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _navMeshAgent.enabled = false;
        
        rigidbody.isKinematic = false;
    }

    public void SetManager(EnemyManager manager)
    {
        _manager = manager;
    }
    
    public void HandleDetection()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _manager.stats.detectionRadius, detectionLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterManager character = colliders[i].transform.GetComponent<CharacterManager>();
            if (character != null)
            {
                Vector3 targetDirection = character.transform.position - transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
                
                if ((character.transform.root != transform.root)
                    && (viewableAngle > _manager.stats.minDetectionAngle && viewableAngle < _manager.stats.maxDetectionAngle))
                {
                    currentTarget = character;
                }
            }
        }
    }

    // Move towards target
    public void HandleMovement()
    {
        if (_manager.isInteracting)
        {
            _manager.animatorHandler.UpdateAnimatorValues(0, 0);
            _navMeshAgent.enabled = false;
            return;
        }
        
        Vector3 targetDirection = currentTarget.transform.position - transform.position;
        float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

        distanceFromTarget = Vector3.Distance(currentTarget.transform.position, transform.position);
        if (distanceFromTarget > _manager.stats.stoppingDistance)
        {
            _manager.animatorHandler.UpdateAnimatorValues(0, 1);
        }
        else
        {
            _manager.animatorHandler.UpdateAnimatorValues(0, 0);
        }
    }

    // Rotate towards target
    public void HandleRotation()
    {
        if (_manager.isInteracting) // If the enemy is performing an action then rotate manually
        {
            Vector3 direction = currentTarget.transform.position - transform.position;
            direction.y = 0;
            direction.Normalize();

            if (direction == Vector3.zero)
                direction = transform.forward;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed / Time.deltaTime);
        }
        else // If the enemy is not performing an action then rotate with pathfinding
        {
            Vector3 relativeDirection = transform.InverseTransformDirection(_navMeshAgent.desiredVelocity);
            Vector3 targetVelocity = rigidbody.velocity;

            _navMeshAgent.enabled = true;
            _navMeshAgent.SetDestination(currentTarget.transform.position);

            rigidbody.velocity = targetVelocity;
            transform.rotation = Quaternion.Slerp(transform.rotation, _navMeshAgent.transform.rotation,
                rotationSpeed / Time.deltaTime);
        }
    }

    public void ResetNavMesh()
    {
        _navMeshAgent.transform.localPosition = Vector3.zero;
        _navMeshAgent.transform.localRotation = Quaternion.identity;
    }
}
