using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLocomotion : CharacterLocomotion
{
    public void Initialize()
    {
        characterCollider = transform.GetComponent<CapsuleCollider>();
        characterColliderBlocker = transform.GetChild(2).GetChild(2).GetChild(0).GetComponent<CapsuleCollider>();

        DisableCharacterCollision();
    }

    public void Move(float horizontal, float vertical)
    {
        EnemyManager.Instance.animatorHandler.SetFloat("Horizontal", horizontal, 0.2f, Time.deltaTime);
        EnemyManager.Instance.animatorHandler.SetFloat("Vertical", vertical, 0.2f, Time.deltaTime);
    }

    #region Run
    
    public Tuple<float, float> RunAroundTarget(EnemyManager manager)
    {
        float verticalMovement = 0f;
        float horizontalMovement = SnapRunHorizontal(UnityEngine.Random.Range(-1, 1));

        return new Tuple<float, float>(verticalMovement, horizontalMovement);
    }
    public Tuple<float, float> RunTowardsTarget(EnemyManager manager)
    {
        float verticalMovement = 1f;
        float horizontalMovement = SnapRunHorizontal(UnityEngine.Random.Range(-1, 1));
        
        return new Tuple<float, float>(verticalMovement, horizontalMovement);
    }
    public Tuple<float, float> RunAwayFromTarget(EnemyManager manager)
    {
        float verticalMovement = -1f;
        float horizontalMovement = SnapRunHorizontal(UnityEngine.Random.Range(-1, 1));
        
        return new Tuple<float, float>(verticalMovement, horizontalMovement);
    }
    private float SnapRunHorizontal(float value)
    {
        if (value >= -1 && value < 0)
            return -1f;
        return 1f;
    }

    #endregion

    #region Walk

    public Tuple<float, float> WalkAroundTarget(EnemyManager manager)
    {
        float verticalMovement = 0f;
        float horizontalMovement = SnapWalkHorizontal(UnityEngine.Random.Range(-1, 1));
        
        return new Tuple<float, float>(verticalMovement, horizontalMovement);
    }
    public Tuple<float, float> WalkTowardsTarget(EnemyManager manager)
    {
        float verticalMovement = 0.5f;
        float horizontalMovement = SnapWalkHorizontal(UnityEngine.Random.Range(-1, 1));
        
        return new Tuple<float, float>(verticalMovement, horizontalMovement);
    }
    public Tuple<float, float> WalkAwayFromTarget(EnemyManager manager)
    {
        float verticalMovement = -0.5f;
        float horizontalMovement = SnapWalkHorizontal(UnityEngine.Random.Range(-1, 1));
        
        return new Tuple<float, float>(verticalMovement, horizontalMovement);   
    }
    private float SnapWalkHorizontal(float value)
    {
        if (value >= -1 && value < 0)
            return -0.5f;
        return 0.5f;
    }

    #endregion

}
