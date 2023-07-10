using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState : MonoBehaviour
{
    [Header("A.I. Settings - Movement")]
    [SerializeField] protected float _distanceFromTarget;
    [SerializeField] protected float _viewableAngle;

    public abstract EnemyState Tick(EnemyManager manager);

    public void UpdateVision(EnemyManager manager)
    {
        if (manager.currentTarget == null)
            return;
        Vector3 direction = manager.currentTarget.transform.position - manager.transform.position;
        _distanceFromTarget = Vector3.Distance(manager.currentTarget.transform.position, manager.transform.position);
        _viewableAngle = Vector3.Angle(direction, manager.transform.forward);
    }
}
