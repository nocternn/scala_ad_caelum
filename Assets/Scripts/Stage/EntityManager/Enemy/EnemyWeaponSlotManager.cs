using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponSlotManager : CharacterWeaponSlotManager
{
    public void Initialize()
    {
        Initialize(transform);
    }

    #region Loaders
    
    public override bool LoadWeaponOnSlot(WeaponItem weaponItem)
    {
        bool isLoaded = base.LoadWeaponOnSlot(weaponItem);
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
        if (EnemyManager.Instance.isTwoHanding)
        {
            base.LoadTwoHandIK();
            EnemyManager.Instance.animatorHandler.SetHandIK(leftHandIkTarget, rightHandIkTarget, EnemyManager.Instance.isTwoHanding);
        }
    }

    #endregion

    #region Setters

    public void SetUsedWeaponType()
    {
        WeaponItem weapon = GetCurrentWeapon();
        
        if (StageManager.Instance.isLocalBattle)
            EnemyManager.Instance.animatorHandler.SetUsedWeaponType(Dictionaries.WeaponTypePlayer[weapon.type]);
        EnemyManager.Instance.isTwoHanding = (weapon.handlingType == Enums.WeaponHandlingType.Both);
        EnemyManager.Instance.isAiming = (weapon.combatType == Enums.WeaponCombatType.Ranged);
    }

    public override void ToggleShooting()
    {
        WeaponAction[] actions = GetCurrentWeapon().GetActions();
        foreach (var action in actions)
        {
            if (action.type == Enums.WeaponActionType.Shoot)
                action.PerformAction(EnemyManager.Instance);
        }
    }

    #endregion
}
