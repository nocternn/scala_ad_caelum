using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon")]
public class WeaponItem : Item
{
    #region Attributes

    [Header("Specific Item Information")]
    public AmmoItem ammo;
    public Enums.WeaponType type;
    public Enums.WeaponCombatType combatType;
    public GameObject model;
    public Sprite secondaryIcon;
    public int id;

    [Header("Stats")]
    public int atk;
    public int crt;

    [Header("Actions")]
    [SerializeField] private WeaponAction[] _actions;
    
    #endregion

    public string GetName()
    {
        return name;
    }

    public string GetDescription()
    {
        string updatedDescription = description;
        foreach (var action in _actions)
        {
            string actionDescription = action.GetDescription();
            if (actionDescription.Length > 0)
                updatedDescription += "\n" + action.GetDescription();
        }

        return updatedDescription;
    }

    public int GetSkillCost(Enums.ActionType actionType)
    {
        foreach (var action in _actions)
        {
            switch (action.type)
            {
                case Enums.ActionType.Active:
                    if (action.type == actionType)
                        return ((ActiveAttackAction)action).cost;
                    return 0;
                case Enums.ActionType.Ultimate:
                    if (action.type == actionType)
                        return ((UltimateAttackAction)action).cost;
                    return 0;
                default:
                    continue;
            }
        }
        return 0;
    }
    
    public Tuple<bool, int> GetSkillStatus(Enums.ActionType actionType)
    {
        bool cooldownStatus = false;
        int currentCooldown = -1;
        
        foreach (var action in _actions)
        {
            if (action.type == actionType)
            {
                switch (action.type)
                {
                case Enums.ActionType.Active:
                    cooldownStatus = ((ActiveAttackAction)action).skill.onCooldown;
                    currentCooldown = ((ActiveAttackAction)action).skill.currentCooldown;
                    break;
                case Enums.ActionType.Ultimate:
                    cooldownStatus = ((UltimateAttackAction)action).skill.onCooldown;
                    currentCooldown = ((UltimateAttackAction)action).skill.currentCooldown;
                    break;
                }
            }
        }
        
        return new Tuple<bool, int>(cooldownStatus, currentCooldown);
    }

    public WeaponAction[] GetActions()
    {
        return _actions;
    }

    public Tuple<bool, int, Enums.ActionType> PerformAttack(
        Enums.CharacterType character, Enums.ActionType attack,
        int id = 0,
        bool playAnimation = true)
    {
        bool performedStatus = false;
        int performedID = -1;
        Enums.ActionType performedType = Enums.ActionType.None;
        
        foreach (var action in _actions)
        {
            if (action.type == attack && action.id == id)
            {
                switch (character)
                {
                    case Enums.CharacterType.Playable:
                        action.PerformAction(PlayerManager.Instance, playAnimation);
                        break;
                    default:
                        action.PerformAction(EnemyManager.Instance, playAnimation);
                        break;
                }

                performedStatus = true;
                performedID = action.id;
                performedType = action.type;
            }
        }
        
        return new Tuple<bool, int, Enums.ActionType>(performedStatus, performedID, performedType);
    }
    
    
}
