using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterAnimatorHandler : MonoBehaviour
{
    [SerializeField] protected Animator _animator;
    
    protected int _horizontal;
    protected int _vertical;

    protected RigBuilder _rigBuilder;
    public TwoBoneIKConstraint leftHandConstraint;
    public TwoBoneIKConstraint rightHandConstraint;

    public virtual void Initialize()
    {
        _animator = GetComponent<Animator>();
        _animator.SetBool("canRotate", true);
        
        _horizontal = Animator.StringToHash("Horizontal");
        _vertical   = Animator.StringToHash("Vertical");
    }

    public void PlayTargetAnimation(string targetAnimation, bool isInteracting)
    {
        _animator.applyRootMotion = isInteracting;
        _animator.SetBool("isInteracting", isInteracting);
        _animator.CrossFade(targetAnimation, 0.2f);
    }
    
    public void UpdateAnimatorValues(float horizontalMovement, float verticalMovement)
    {
        // Animation Snapping
        float snappedHorizontal = GetSnappedValue(horizontalMovement);
        float snappedVertical = GetSnappedValue(verticalMovement);

        _animator.SetFloat(_horizontal, snappedHorizontal, 0.1f, Time.deltaTime);
        _animator.SetFloat(_vertical, snappedVertical, 0.1f, Time.deltaTime);
    }

    #region Setters

    public void SetSpeed(float speed)
    {
        _animator.speed = speed;
    }

    public void SetBool(string key, bool value)
    {
        _animator.SetBool(key, value);
    }
    
    public void SetFloat(string key, float value, float dampTime, float deltaTime)
    {
        _animator.SetFloat(key, value, dampTime, deltaTime);
    }

    public virtual void SetHandIK(LeftHandIKTarget leftHandTarget, RightHandIKTarget rightHandTarget, bool isTwoHanding)
    {
        if (isTwoHanding)
        {
            rightHandConstraint.data.target = rightHandTarget.transform;
            rightHandConstraint.data.targetPositionWeight = 1;
            rightHandConstraint.data.targetRotationWeight = 1;

            leftHandConstraint.data.target = leftHandTarget.transform;
            leftHandConstraint.data.targetPositionWeight = 1;
            leftHandConstraint.data.targetRotationWeight = 1;
        }
        else
        {
            rightHandConstraint.data.target = null;
            leftHandConstraint.data.target = null;
        }
    }

    public virtual void EraseHandIK()
    {
        
    }

    #endregion
    
    #region Getters

    public Vector3 GetDeltaPosition()
    {
        return _animator.deltaPosition;
    }
    
    public bool GetBool(string key)
    {
        return _animator.GetBool(key);
    }

    public float GetFloat(string key)
    {
        return _animator.GetFloat(key);
    }

    private float GetSnappedValue(float movement)
    {
        if (movement > 0 && movement <= 1)
        {
            return 1;
        }
        else if (movement < 0 && movement >= -1)
        {
            return -1;
        }
        return 0;
    }
    
    #endregion

}
