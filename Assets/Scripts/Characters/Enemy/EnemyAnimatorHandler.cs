using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorHandler : CharacterAnimatorHandler
{
    private EnemyManager _manager;

    private void OnAnimatorMove()
    {
        float delta = Time.deltaTime;

        _manager.locomotion.rigidbody.drag = 0;

        Vector3 deltaPosition = GetDeltaPosition();
        deltaPosition.y = 0;

        _manager.locomotion.rigidbody.velocity = deltaPosition / delta;
    }

    public void SetManager(EnemyManager manager)
    {
        _manager = manager;
    }
    
    public void SetEnemyType(String enemyName)
    {
        foreach (string enemyType in EnemyManager.Types)
            SetBool(String.Format("is{0}", enemyType), false);
        SetBool(String.Format("is{0}", enemyName), true);
    }
}