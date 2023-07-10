﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EnemyAnimatorHandler : CharacterAnimatorHandler
{
    private void OnAnimatorMove()
    {
        float delta = Time.deltaTime;

        EnemyManager.Instance.rigidbody.drag = 0;

        Vector3 deltaPosition = GetDeltaPosition();
        deltaPosition.y = 0;

        EnemyManager.Instance.rigidbody.velocity = deltaPosition / delta;
    }
    
    public override void Initialize()
    {
        base.Initialize();

        _rigBuilder = GetComponent<RigBuilder>();
    }
    
    public override void SetHandIK(LeftHandIKTarget leftHandTarget, RightHandIKTarget rightHandTarget, bool isTwoHanding)
    {
        base.SetHandIK(leftHandTarget, rightHandTarget, isTwoHanding);
        EnemyManager.Instance.rigBuilder.Build();
    }
    
    public void SetEnemyType(string enemyName)
    {
        foreach(KeyValuePair<Enums.EnemyType, string> type in Dictionaries.EnemyType)
        {
            SetBool(String.Format("is{0}", type.Value), false);
        }
        SetBool(String.Format("is{0}", enemyName), true);
    }
}