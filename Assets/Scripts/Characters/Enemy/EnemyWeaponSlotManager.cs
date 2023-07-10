using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponSlotManager : CharacterWeaponSlotManager
{
    public void Initialize()
    {
        Initialize(transform);
        
        LoadWeaponOnSlot(leftHandWeapon, true, false);
        LoadWeaponOnSlot(rightHandWeapon, false, true);
    }

    #region Loaders
    
    public override bool LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft, bool isRight)
    {
        bool isLoaded = base.LoadWeaponOnSlot(weaponItem, isLeft, isRight);
/*
        string weaponType = GetCurrentWeaponType();
        if ((weaponType.Equals(weaponTypes[0]) || weaponType.Equals(weaponTypes[2])) && rightHandDamageCollider == null)
        {
            rightHandDamageCollider = weaponItem.ammo.model.GetComponentInChildren<DamageCollider>();
        }
*/
        return isLoaded;
    }
    
    public override void LoadTwoHandIK()
    {
        if (!EnemyManager.Instance.isTwoHanding)
            return;
        base.LoadTwoHandIK();
        EnemyManager.Instance.animatorHandler.SetHandIK(leftHandIkTarget, rightHandIkTarget, EnemyManager.Instance.isTwoHanding);
    }

    #endregion

    #region Setters

    public void SetUsedWeaponType()
    {
        WeaponItem weapon = GetCurrentWeapon();
        
        // Toggle two-handing for spear and crossbow
        if (weapon.type == Enums.WeaponType.Crossbow || weapon.type == Enums.WeaponType.Spear)
        {
            EnemyManager.Instance.isTwoHanding = true;
        }
        else
        {
            EnemyManager.Instance.isTwoHanding = false;
        }

        // Toggle aim for bow and crossbow
        if (weapon.type == Enums.WeaponType.Bow || weapon.type == Enums.WeaponType.Crossbow)
        {
            EnemyManager.Instance.isAiming = true;
        }
        else
        {
            EnemyManager.Instance.isAiming = false;
        }
    }

    public override void ToggleShooting()
    {
        WeaponAction[] actions = GetCurrentWeapon().GetActions();
        foreach (var action in actions)
        {
            if (action.type == Enums.ActionType.Shoot)
                action.PerformAction(EnemyManager.Instance);
        }
    }

    #endregion
}
