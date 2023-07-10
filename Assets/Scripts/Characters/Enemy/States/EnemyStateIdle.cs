using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateIdle : EnemyState
{
    [Header("A.I. Settings - Components")]
    [SerializeField] private LayerMask detectionLayer;
    [SerializeField] private LayerMask lineOfSightBlockingLayer;
    [SerializeField] private EnemyStatePursueTarget pursueTargetState;
    
    [Header("A.I. Settings - Detection")]
    public float detectionRadius = 15;
    public float minDetectionAngle = -50;
    public float maxDetectionAngle = 50;
    
    public override EnemyState Tick(EnemyManager manager)
    {
        // Look for a potential target
        HandleDetection(manager);

        // Switch to pursue the new target
        if (manager.currentTarget != null)
            return pursueTargetState;

        return this;
    }

    public bool IsTargetVisible(EnemyManager manager, CharacterManager target, float viewableAngle)
    {
        bool isTargetInFOV = (viewableAngle >= minDetectionAngle) && (viewableAngle <= maxDetectionAngle);
        bool isTargetObstructed = Physics.Linecast(
            manager.lockOnTransform.position,
            target.lockOnTransform.position,
            lineOfSightBlockingLayer
        );
        return isTargetInFOV && !isTargetObstructed;
    }
    
    private void HandleDetection(EnemyManager manager)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, detectionLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterManager target = colliders[i].transform.GetComponent<CharacterManager>();

            // Proceed to next step if target is not of the same type as A.I.
            if (target != null && target.characterType != manager.characterType)
            {
                Vector3 targetDirection = target.transform.position - manager.transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, manager.transform.forward);

                // Assign target as current target if the target is visible to the A.I.
                if (IsTargetVisible(manager, target, viewableAngle))
                {
                    manager.currentTarget = target;
                    return;
                }
            }
        }
    }
}
