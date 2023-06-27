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
}
