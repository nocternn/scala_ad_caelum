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
    public float detectionRadius;
    public float minDetectionAngle;
    public float maxDetectionAngle;

    protected override void Awake()
    {
        pursueTargetState = GetComponent<EnemyStatePursueTarget>();
    }
    
    public override EnemyState Tick()
    {
        // Look for a potential target
        HandleDetection();

        // Switch to pursue the new target
        if (EnemyManager.Instance.currentTarget != null)
            return pursueTargetState;

        return this;
    }

    public bool IsTargetVisible(CharacterManager target, float viewableAngle)
    {
        bool isTargetInFOV = (viewableAngle >= minDetectionAngle) && (viewableAngle <= maxDetectionAngle);
        bool isTargetObstructed = Physics.Linecast(
            EnemyManager.Instance.lockOnTransform.position,
            target.lockOnTransform.position,
            lineOfSightBlockingLayer
        );
        return isTargetInFOV && !isTargetObstructed;
    }
    
    private void HandleDetection()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, detectionLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterManager target = colliders[i].transform.GetComponent<CharacterManager>();
            // Proceed to next step if target is not of the same type as A.I.
            if (target != null && target.characterType != EnemyManager.Instance.characterType)
            {
                Vector3 targetDirection = target.transform.position - EnemyManager.Instance.transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, EnemyManager.Instance.transform.forward);

                // Assign target as current target if the target is visible to the A.I.
                if (IsTargetVisible(target, viewableAngle))
                {
                    EnemyManager.Instance.currentTarget = target;
                    return;
                }
            }
        }
    }
}
