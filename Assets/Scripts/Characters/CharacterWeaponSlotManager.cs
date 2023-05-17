using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWeaponSlotManager : MonoBehaviour
{
    protected WeaponHolderSlot leftHandSlot;
    protected WeaponHolderSlot rightHandSlot;

    protected DamageCollider leftHandDamageCollider;
    protected DamageCollider rightHandDamageCollider;

    public WeaponItem leftHandWeapon;
    public WeaponItem rightHandWeapon;
    
    public bool isUsingLeftHand;
    public bool isUsingRightHand;

    public string[] weaponTypes;
    
    protected void Initialize(Transform character)
    {
        WeaponHolderSlot[] weaponHolderSlots = character.GetComponentsInChildren<WeaponHolderSlot>();
        foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
        {
            if (weaponSlot.isLeftHandSlot)
            {
                leftHandSlot = weaponSlot;
                LoadWeaponOnSlot(leftHandWeapon, true, false);
            }
            else if (weaponSlot.isRightHandSlot)
            {
                rightHandSlot = weaponSlot;
                LoadWeaponOnSlot(rightHandWeapon, false, true);
            }
        }
    }
    
    public bool LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft, bool isRight)
    {
        isUsingLeftHand = isLeft;
        isUsingRightHand = isRight;
        
        bool isLoaded = false;

        if (isLeft)
        {
            isLoaded = leftHandSlot.LoadWeaponModel(weaponItem);
            if (isLoaded)
                leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        }
        if (isRight)
        {
            isLoaded = rightHandSlot.LoadWeaponModel(weaponItem);
            if (isLoaded)
                rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        }

        return isLoaded;
    }

    #region Getters

    public WeaponItem GetCurrentWeapon()
    {
        if (isUsingLeftHand)
            return leftHandWeapon;
        return rightHandWeapon;
    }
        
    public string GetCurrentWeaponType()
    {
        WeaponItem weaponItem = GetCurrentWeapon();
        foreach (string weaponType in weaponTypes)
        {
            if (weaponItem.name.Equals(weaponType))
            {
                return weaponType;
            }
        }
        return null;
    }

    #endregion
    
    #region Handle Damage Colliders

    public void EnableDamageCollider()
    {
        if (isUsingLeftHand)
        {
            leftHandDamageCollider.Enable();
        }
        else if (isUsingRightHand)
        {
            rightHandDamageCollider.Enable();
        }
    }

    public void DisableDamageCollider()
    {
        if (isUsingLeftHand)
        {
            leftHandDamageCollider.Disable();
        }
        else if (isUsingRightHand)
        {
            rightHandDamageCollider.Disable();
        }
    }
        
    #endregion
}
