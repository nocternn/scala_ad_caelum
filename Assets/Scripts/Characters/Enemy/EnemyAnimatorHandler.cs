using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EnemyAnimatorHandler : CharacterAnimatorHandler
{
    private EnemyManager _manager;

    private void OnAnimatorMove()
    {
        float delta = Time.deltaTime;

        _manager.rigidbody.drag = 0;

        Vector3 deltaPosition = GetDeltaPosition();
        deltaPosition.y = 0;

        _manager.rigidbody.velocity = deltaPosition / delta;
    }
    
    public override void Initialize()
    {
        base.Initialize();

        _rigBuilder = GetComponent<RigBuilder>();
    }
    
    public override void SetHandIK(LeftHandIKTarget leftHandTarget, RightHandIKTarget rightHandTarget, bool isTwoHanding)
    {
        base.SetHandIK(leftHandTarget, rightHandTarget, isTwoHanding);
        _manager.rigBuilder.Build();
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