using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterManager : MonoBehaviour
{
    protected StageManager _stage;
    
    public RigBuilder rigBuilder;

    public Transform lockOnTransform;
    public bool isInteracting;
    public bool isTwoHanding;
    public bool isAiming;
    public bool canRotate;

    public MultiAimConstraint aim;
    
    public void SetManager(StageManager stage)
    {
        _stage = stage;
    }
    
    public virtual void ToggleAim()
    {
        aim.enabled = isAiming;
        aim.gameObject.SetActive(isAiming);
        rigBuilder.Build();
    }
}
