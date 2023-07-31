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
    public bool canRotate;
    public bool isAiming;
    public bool isHit;
    public bool isInteracting;
    public bool isTwoHanding;
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
    
    public void PerformAction(Enums.CharacterActionType type)
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

    public void Die()
    {
        _hasDied = true;
    }

    public bool Died()
    {
        return _hasDied;
    }
}
