using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    #region Attributes
    
    public static CameraManager Instance { get; private set; }

    public CameraHandler currentCamera;

    [SerializeField] private CameraHandlerFree freeCamera;
    [SerializeField] private CameraHandlerAim aimCamera;
    [SerializeField] private CameraHandlerLocked lockedCamera;
    [SerializeField] private CameraHandlerStandard standardCamera;

    #endregion

    private void Awake()
    {
        Instance = this;

        freeCamera.Initialize();
        aimCamera.Initialize();
        lockedCamera.Initialize();
        standardCamera.Initialize();
        
        currentCamera = freeCamera;
        currentCamera.camera.enabled = true;
    }

    public void SetCamera(Enums.CameraType type)
    {
        currentCamera.camera.enabled = false;
        switch (type)
        {
            case Enums.CameraType.Aim:
                currentCamera = aimCamera;
                break;
            case Enums.CameraType.Locked:
                currentCamera = lockedCamera;
                break;
            case Enums.CameraType.Standard:
                currentCamera = standardCamera;
                break;
            default:
                currentCamera = freeCamera;
                break;
        }
        currentCamera.camera.enabled = true;
    }
    
    #region Getters

    public Vector3 GetDirection(string direction = "none")
    {
        if (direction.Equals("forward"))
            return currentCamera.cameraTransform.forward;
        if (direction.Equals("right"))
            return currentCamera.cameraTransform.right;
        return Vector3.zero;
    }

    public Quaternion GetRotation(string target = "camera")
    {
        if (target.Equals("pivot"))
            return currentCamera.cameraPivotTransform.rotation;
        if (target.Equals("lockOn"))
            return currentCamera.currentLockOnTarget.rotation;
        return currentCamera.cameraTransform.rotation;
    }
    
    public Vector3 GetEulerAngles(string target = "camera")
    {
        if (target.Equals("pivot"))
            return currentCamera.cameraPivotTransform.eulerAngles;
        if (target.Equals("lockOn"))
            return currentCamera.currentLockOnTarget.eulerAngles;
        return currentCamera.cameraTransform.eulerAngles;
    }
    public Vector3 GetLocalEulerAngles(string target = "camera")
    {
        if (target.Equals("pivot"))
            return currentCamera.cameraPivotTransform.localEulerAngles;
        if (target.Equals("lockOn"))
            return currentCamera.currentLockOnTarget.localEulerAngles;
        return currentCamera.cameraTransform.localEulerAngles;
    }
    
    public Vector3 GetPosition(string target = "camera")
    {
        if (target.Equals("pivot"))
            return currentCamera.cameraPivotTransform.position;
        if (target.Equals("lockOn"))
            return currentCamera.currentLockOnTarget.position;
        return currentCamera.cameraTransform.position;
    }

    #endregion
}
