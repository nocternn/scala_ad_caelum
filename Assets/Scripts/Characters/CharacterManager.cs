using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterManager : MonoBehaviour
{
    [Header("Helpers")]
    public RigBuilder rigBuilder;
    public Rig rigLayer;
    public Transform lockOnTransform;
    
    [Header("Properties")]
    public bool isInteracting;
    public bool isTwoHanding;
    public bool isAiming;
    public bool canRotate;
    public bool isHit;
    [SerializeField] protected bool _hasDied;
    [SerializeField] protected bool _isActive;
    [SerializeField] protected CharacterAction[] _actions;

    public Enums.CharacterType characterType;

    protected virtual void Awake()
    {
        lockOnTransform = transform.GetChild(2).GetChild(0);
    }

    public virtual void ToggleActive(bool state)
    {
        _isActive = state;
    }
    
    public void PerformAction(Enums.ActionType type)
    {
        foreach (var action in _actions)
        {
            if (action.type == type)
            {
                switch (characterType)
                {
                    case Enums.CharacterType.Playable:
                        action.PerformAction(PlayerManager.Instance);
                        break;
                    default:
                        action.PerformAction(EnemyManager.Instance);
                        break;
                }
                break;
            }
        }
    }

    public virtual CharacterAction[] GetActions()
    {
        return _actions;
    }
}
