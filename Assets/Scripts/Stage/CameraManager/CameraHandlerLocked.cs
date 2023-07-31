using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandlerLocked : CameraHandler
{
    public override void FollowTarget(float delta)
    {
        transform.position = Vector3.SmoothDamp(transform.position, targetTransform.position,
            ref _followVelocity, delta * followSpeed);
        HandleCollisions(delta);
    }

    public override void HandleRotation(float delta, float horizontalInput, float verticalInput)
    {
        Vector3 direction = currentLockOnTarget.position - transform.position;
        direction.Normalize();
        direction.y = 0;

        transform.rotation = Quaternion.LookRotation(direction);

        direction = currentLockOnTarget.position - cameraPivotTransform.position;
        direction.Normalize();
        direction.y = 0;

        Vector3 eulerAngles = Quaternion.LookRotation(-direction).eulerAngles;
        eulerAngles.y = 0;

        cameraPivotTransform.localEulerAngles = eulerAngles;
    }
}