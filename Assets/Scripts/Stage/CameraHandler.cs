using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    #region Attributes
    
    public static CameraHandler singleton;
    private PlayerManager _playerManager;
    
    private Vector3 cameraTransformPosition;
    private Vector3 cameraFollowVelocity = Vector3.zero;
    private LayerMask ignoreLayers;
    
    [Header("Objects")]
    public Transform targetTransform;
    public Transform cameraTransform;
    public Transform cameraPivotTransform;

    [Header("Properties - Follow")]
    public float horizontalSpeed;
    public float horizontalAimSpeed;
    public float verticalSpeed;
    public float verticalAimSpeed;
    public float followSpeed;
    
    private float defaultPosition;
    private float horizontalAngle;
    private float verticalAngle;
    
    [Header("Properties - Collision")]
    public float cameraSphereRadius;
    public float cameraCollisionOffset;
    public float minimumCollisionOffset;

    [Header("Properties - Lock On")]
    public float minimumViewableAngle;
    public float maximumViewableAngle;
    public Transform nearestLockOnTarget;
    public Transform currentLockOnTarget;
    public List<CharacterManager> availableTargets = new List<CharacterManager>();
        
    [Header("Pivots")]
    public float minimumVerticalAngle;
    public float maximumVerticalAngle;
    public float lockedPivotPosition;
    public float unlockedPivotPosition;

    #endregion
    
    void Awake()
    {
        singleton = this;
        
        defaultPosition = cameraTransform.localPosition.z;
        ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
        
        // Follow
        horizontalSpeed = 150f;
        horizontalAimSpeed = 50f;
        verticalSpeed = 150f;
        verticalAimSpeed = 50f;
        followSpeed = 1f;
        
        // Collision
        cameraSphereRadius = 0.2f;
        cameraCollisionOffset = 0.2f;
        minimumCollisionOffset = 0.2f;
        
        // Lock on
        minimumViewableAngle = -50;
        maximumViewableAngle = 50;
        
        // Pivot
        minimumVerticalAngle = -35;
        maximumVerticalAngle = 35;
        lockedPivotPosition = 5f;
        unlockedPivotPosition = 2f;
    }
    
    public void SetPlayerManager(PlayerManager manager)
    {
        _playerManager = manager;
    }

    public void FollowTarget(float delta)
    {
        Vector3 targetPosition;
        if (_playerManager.isAiming)
        {
            targetPosition = Vector3.SmoothDamp(transform.position, currentLockOnTarget.position,
                ref cameraFollowVelocity, delta * followSpeed);
        }
        else
        {
            targetPosition = Vector3.SmoothDamp(transform.position, targetTransform.position,
                ref cameraFollowVelocity, delta * followSpeed);
        }
        transform.position = targetPosition;
        HandleCollisions(delta);
    }

    public void HandleRotation(float delta, float horizontalCameraInput, float verticalCameraInput)
    {
        if (!_playerManager.IsLockingOnTarget())
        {
            HandleStandardRotation(delta, horizontalCameraInput, verticalCameraInput);
        }
        else
        {
            if (_playerManager.isAiming)
            {
                HandleAimedRotation(delta, horizontalCameraInput, verticalCameraInput);
            }
            else
            {
                HandleLockedRotation();
            }
        }
    }
    private void HandleStandardRotation(float delta, float horizontalCameraInput, float verticalCameraInput)
    {
        horizontalAngle  += horizontalCameraInput * horizontalSpeed * delta;
        
        verticalAngle -= verticalCameraInput * verticalSpeed * delta;
        verticalAngle  = Mathf.Clamp(verticalAngle, minimumVerticalAngle, maximumVerticalAngle);

        Vector3 rotation = Vector3.zero;
        rotation.y = horizontalAngle;

        transform.rotation = Quaternion.Euler(rotation);
        
        rotation = Vector3.zero;
        rotation.x = verticalAngle;
        
        cameraPivotTransform.localRotation = Quaternion.Euler(rotation);
    }
    private void HandleAimedRotation(float delta, float horizontalCameraInput, float verticalCameraInput)
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        cameraPivotTransform.rotation = Quaternion.Euler(0, 0, 0);

        Quaternion targetRotationX;
        Quaternion targetRotationY;

        horizontalAngle += horizontalCameraInput * horizontalAimSpeed * delta;
        verticalAngle   -= verticalCameraInput * verticalAimSpeed * delta;

        targetRotationY = Quaternion.Euler(new Vector3(0, horizontalAngle, 0));
        targetRotationY = Quaternion.Slerp(transform.rotation, targetRotationY, 1);
        transform.localRotation = targetRotationY;
        
        targetRotationX = Quaternion.Euler(new Vector3(verticalAngle, 0, 0));
        targetRotationX = Quaternion.Slerp(cameraTransform.rotation, targetRotationX, 1);
        cameraTransform.localRotation = targetRotationX;
    }
    private void HandleLockedRotation()
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
    
    
    public void HandleCollisions(float delta)
    {
        float targetPosition = defaultPosition;
        RaycastHit hit;
        
        Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
        direction.Normalize();

        if (Physics.SphereCast(cameraPivotTransform.position, cameraSphereRadius, direction,
                out hit, Mathf.Abs(defaultPosition), ignoreLayers))
        {
            float distance = Vector3.Distance(cameraPivotTransform.position, hit.point);
            targetPosition = -(distance - cameraCollisionOffset);
        }

        if (Mathf.Abs(targetPosition) < minimumCollisionOffset)
        {
            targetPosition = -minimumCollisionOffset;
        }

        cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, delta / 0.2f);
        cameraTransform.localPosition = cameraTransformPosition;
    }

    public void HandleLockOn()
    {
        if (_playerManager.isAiming)
        {
            nearestLockOnTarget = _playerManager.aimTarget;
            return;
        }
        
        float shortestDistance = Mathf.Infinity;

        Collider[] colliders = Physics.OverlapSphere(targetTransform.position, 26);
        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterManager character = colliders[i].GetComponent<CharacterManager>();
            if (character == null)
                continue;

            Vector3 lockTargetDirection = character.transform.position - targetTransform.position;
            // In case we only want to lock on targets within range
            // float distanceFromTarget = Vector3.Distance(targetTransform.position, character.transform.position);
            //----
            float viewableAngle = Vector3.Angle(lockTargetDirection, cameraTransform.forward);

            if ((character.transform.root != targetTransform.transform.root)
                && (viewableAngle > minimumViewableAngle && viewableAngle < maximumViewableAngle))
            {
                availableTargets.Add(character);
            }
        }

        for (int i = 0; i < availableTargets.Count; i++)
        {
            float distanceFromTarget = Vector3.Distance(targetTransform.position, availableTargets[i].transform.position);
            if (distanceFromTarget < shortestDistance)
            {
                shortestDistance = distanceFromTarget;
                nearestLockOnTarget = availableTargets[i].lockOnTransform;
            }
        }
    }

    public void ClearLockOnTargets()
    {
        availableTargets.Clear();
        nearestLockOnTarget = null;
        currentLockOnTarget = null;
    }

    public void SetCameraHeight(float delta)
    {
        Vector3 velocity = Vector3.zero;
        Vector3 newLockedPosition = new Vector3(0, lockedPivotPosition);
        Vector3 newUnlockedPosition = new Vector3(0, unlockedPivotPosition);

        if (_playerManager.isAiming)
        {
            cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(
                cameraPivotTransform.transform.localPosition, newUnlockedPosition,
                ref velocity, delta);
        }
        else
        {
            cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(
                cameraPivotTransform.transform.localPosition, newLockedPosition,
                ref velocity, delta);
        }
    }
}
