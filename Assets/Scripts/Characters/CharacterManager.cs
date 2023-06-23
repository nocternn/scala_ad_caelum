using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterManager : MonoBehaviour
{
    public RigBuilder rigBuilder;

    public Transform lockOnTransform;
    public bool isInteracting;
    public bool isTwoHanding;
    public bool isAiming;
    public bool canRotate;

    public MultiAimConstraint aim;

    protected virtual void Awake()
    {
        lockOnTransform = transform.GetChild(2).GetChild(0);
    }
    
    public virtual void ToggleAim()
    {
        aim.enabled = isAiming;
        aim.gameObject.SetActive(isAiming);
        rigBuilder.Build();
    }
}
