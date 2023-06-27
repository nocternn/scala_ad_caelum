using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandlerAim : CameraHandler
{
    #region Attributes
    public float verticalAimSpeed;
    public float horizontalAimSpeed;

    #endregion
    
    public override void Initialize()
    {
        base.Initialize();
        horizontalAimSpeed = 50f;
        verticalAimSpeed = 50f;
    }

    public override void FollowTarget(float delta)
    {
        transform.position = Vector3.SmoothDamp(transform.position, currentLockOnTarget.position,
                ref _followVelocity, delta * followSpeed);
        HandleCollisions(delta);
    }

    public override void HandleRotation(float delta, float horizontalInput, float verticalInput)
    {
        Quaternion targetRotationX;
        Quaternion targetRotationY;

        _horizontalAngle += horizontalInput * horizontalAimSpeed * delta;
        _verticalAngle   -= verticalInput * verticalAimSpeed * delta;

        targetRotationY = Quaternion.Euler(new Vector3(0, _horizontalAngle, 0));
        targetRotationY = Quaternion.Slerp(transform.rotation, targetRotationY, 1);
        transform.localRotation = targetRotationY;
        
        targetRotationX = Quaternion.Euler(new Vector3(_verticalAngle, 0, 0));
        targetRotationX = Quaternion.Slerp(cameraTransform.rotation, targetRotationX, 1);
        cameraTransform.localRotation = targetRotationX;
    }
    
    public override void HandleLockOn()
    {
        nearestLockOnTarget = PlayerManager.Instance.aimTarget;
    }
}
