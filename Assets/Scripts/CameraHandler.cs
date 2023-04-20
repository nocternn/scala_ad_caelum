using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
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
    public float lookSpeed = 0.1f;
    public float followSpeed = 0.1f;
    public float pivotSpeed = 0.03f;
    
    private float defaultPosition;
    private float lookAngle;
    private float pivotAngle;
    
    [Header("Properties - Collision")]
    public float cameraSphereRadius = 0.2f;
    public float cameraCollisionOffset = 0.2f;
    public float minimumCollisionOffset = 0.2f;

    [Header("Properties - Lock On")]
    public float minimumViewableAngle = -50;
    public float maximumViewableAngle = 50;
    public Transform nearestLockOnTarget;
    public Transform currentLockOnTarget;
    public List<CharacterManager> availableTargets = new List<CharacterManager>();
        
    [Header("Pivots")]
    public float minimumPivot = -35;
    public float maximumPivot = 35;
    public float lockedPivotPosition = 5.0f;
    public float unlockedPivotPosition = 5.0f;
    
    void Awake()
    {
        singleton = this;

        defaultPosition = cameraTransform.localPosition.z;
        ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
    }
    
    public void SetPlayerManager(PlayerManager manager)
    {
        _playerManager = manager;
    }

    public void FollowTarget(float delta)
    {
        transform.position = Vector3.SmoothDamp
            (transform.position, targetTransform.position, ref cameraFollowVelocity, delta / followSpeed);
        HandleCollisions(delta);
    }

    public void HandleRotation(float delta, float horizontalCameraInput, float verticalCameraInput)
    {
        if (!_playerManager.IsLockingOnTarget())
        {
            lookAngle  += (horizontalCameraInput * lookSpeed) / delta;
        
            pivotAngle -= (verticalCameraInput * pivotSpeed) / delta;
            pivotAngle  = Mathf.Clamp(pivotAngle, minimumPivot, maximumPivot);

            Vector3 rotation = Vector3.zero;
            rotation.y = lookAngle;

            transform.rotation = Quaternion.Euler(rotation);
        
            rotation = Vector3.zero;
            rotation.x = pivotAngle;
        
            cameraPivotTransform.localRotation = Quaternion.Euler(rotation);
        }
        else
        {
            //float velocity = 0;

            Vector3 direction = currentLockOnTarget.position - transform.position;
            direction.Normalize();
            direction.y = 0;

            transform.rotation = Quaternion.LookRotation(direction);

            direction = currentLockOnTarget.position - cameraPivotTransform.position;
            direction.Normalize();

            Vector3 eulerAngle = Quaternion.LookRotation(-direction).eulerAngles;
            eulerAngle.y = 0;

            cameraPivotTransform.localEulerAngles = eulerAngle;
        }
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

        if (currentLockOnTarget != null)
        {
            cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(
                cameraPivotTransform.transform.localPosition, newLockedPosition,
                ref velocity, delta);
        }
        else
        {
            cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(
                cameraPivotTransform.transform.localPosition, newUnlockedPosition,
                ref velocity, delta);
        }
    }
}
