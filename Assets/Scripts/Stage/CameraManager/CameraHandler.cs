using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CameraHandler : MonoBehaviour
{
    #region Attributes

    [Header("Objects")]
    public new Camera camera;
    public Transform targetTransform;
    public Transform cameraTransform;
    public Transform cameraPivotTransform;
    [SerializeField] protected Vector3 _cameraTransformPosition;

    [Header("Properties - Follow")]
    public float horizontalSpeed;
    public float verticalSpeed;
    public float followSpeed;
    [SerializeField] protected float _defaultPosition;
    [SerializeField] protected Vector3 _followVelocity = Vector3.zero;
    
    [Header("Properties - Collision")]
    public float cameraSphereRadius;
    public float cameraCollisionOffset;
    public float minCollisionOffset;
    [SerializeField] protected LayerMask _ignoreLayers;

    [Header("Properties - Lock On")]
    public Transform nearestLockOnTarget;
    public Transform currentLockOnTarget;
    public List<CharacterManager> availableTargets = new List<CharacterManager>();
        
    [Header("Properties - View")]
    public float minHorizontalAngle;
    public float maxHorizontalAngle;
    public float minVerticalAngle;
    public float maxVerticalAngle;
    [SerializeField] protected float _horizontalAngle;
    [SerializeField] protected float _verticalAngle;

    #endregion

    public virtual void Initialize()
    {
        camera = cameraTransform.GetComponent<Camera>();
        camera.enabled = false;
        
        _defaultPosition = cameraTransform.localPosition.z;
        _ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
        
        // Follow
        horizontalSpeed = 150f;
        verticalSpeed = 150f;
        followSpeed = 1f;
        
        // Collision
        cameraSphereRadius = 0.2f;
        cameraCollisionOffset = 0.2f;
        minCollisionOffset = 0.2f;
        
        // Lock on
        minHorizontalAngle = -50;
        maxHorizontalAngle = 50;
        minVerticalAngle = -35;
        maxVerticalAngle = 35;
    }

    public abstract void FollowTarget(float delta);
    public abstract void HandleRotation(float delta, float horizontalInput, float verticalInput);
    
    public void HandleCollisions(float delta)
    {
        float targetPosition = _defaultPosition;
        RaycastHit hit;
        
        Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
        direction.Normalize();

        if (Physics.SphereCast(cameraPivotTransform.position, cameraSphereRadius, direction,
                out hit, Mathf.Abs(_defaultPosition), _ignoreLayers))
        {
            float distance = Vector3.Distance(cameraPivotTransform.position, hit.point);
            targetPosition = -(distance - cameraCollisionOffset);
        }

        if (Mathf.Abs(targetPosition) < minCollisionOffset)
        {
            targetPosition = -minCollisionOffset;
        }

        _cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, delta / 0.2f);
        cameraTransform.localPosition = _cameraTransformPosition;
    }
    
    public virtual void HandleLockOn()
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
                && (viewableAngle > minHorizontalAngle && viewableAngle < maxHorizontalAngle))
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
}
