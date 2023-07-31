using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandlerStandard : CameraHandler
{
    public override void FollowTarget(float delta)
    {
        transform.position = Vector3.SmoothDamp(transform.position, targetTransform.position,
            ref _followVelocity, delta * followSpeed);
        HandleCollisions(delta);
    }

    public override void HandleRotation(float delta, float horizontalInput, float verticalInput)
    {
        _horizontalAngle  += horizontalInput * horizontalSpeed * delta;
        
        _verticalAngle -= verticalInput * verticalSpeed * delta;
        _verticalAngle  = Mathf.Clamp(_verticalAngle, minVerticalAngle, maxVerticalAngle);

        Vector3 rotation = Vector3.zero;
        rotation.y = _horizontalAngle;

        transform.rotation = Quaternion.Euler(rotation);
        
        rotation = Vector3.zero;
        rotation.x = _verticalAngle;
        
        cameraPivotTransform.localRotation = Quaternion.Euler(rotation);
    }
}