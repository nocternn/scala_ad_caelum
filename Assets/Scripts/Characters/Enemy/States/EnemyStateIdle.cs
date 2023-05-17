using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateIdle : EnemyState
{
    [Header("A.I. Settings - Components")]
    public LayerMask detectionLayer;
    public EnemyStatePursueTarget pursueTargetState;
    
    [Header("A.I. Settings - Detection")]
    public float detectionRadius = 15;
    
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
                Vector3 targetDirection = character.transform.position - transform.position;
                manager.viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                if ((character.transform.root != transform.root)
                    && (manager.viewableAngle >= manager.minDetectionAngle && manager.viewableAngle <= manager.maxDetectionAngle))
                {
                    manager.currentTarget = character;
                }
            }
        }
    }
}
