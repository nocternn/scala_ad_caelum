using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateIdle : EnemyState
{
    [Header("A.I. Settings - Components")]
    [SerializeField] private LayerMask detectionLayer;
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
    
    private void HandleDetection(EnemyManager manager)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, detectionLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterManager character = colliders[i].transform.GetComponent<CharacterManager>();
            if (character != null)
            {
                Vector3 targetDirection = character.transform.position - manager.transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, manager.transform.forward);

                if ((character.transform.root != transform.root)
                    && (viewableAngle >= minDetectionAngle && viewableAngle <= maxDetectionAngle))
                {
                    manager.currentTarget = character;
                    return;
                }
            }
        }
    }
}
