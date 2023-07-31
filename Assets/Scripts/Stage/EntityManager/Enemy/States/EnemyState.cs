using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState : MonoBehaviour
{
    [Header("A.I. Settings - Movement")]
    [SerializeField] protected float _distanceFromTarget;
    [SerializeField] protected float _viewableAngle;

    public abstract EnemyState Tick();

    public void UpdateVision()
    {
        if (EnemyManager.Instance.currentTarget == null)
            return;
        Vector3 direction =
            EnemyManager.Instance.currentTarget.transform.position - EnemyManager.Instance.transform.position;
        _distanceFromTarget = Vector3.Distance(
            EnemyManager.Instance.currentTarget.transform.position,
            EnemyManager.Instance.transform.position);
        _viewableAngle = Vector3.Angle(direction, EnemyManager.Instance.transform.forward);
    }
}
