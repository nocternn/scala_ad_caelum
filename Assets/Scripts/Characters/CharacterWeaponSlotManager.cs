using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWeaponSlotManager : MonoBehaviour
{
    #region Attributes

    public WeaponHolderSlot leftHandSlot;
    public WeaponHolderSlot rightHandSlot;

    protected DamageCollider leftHandDamageCollider;
    protected DamageCollider rightHandDamageCollider;

    protected LeftHandIKTarget leftHandIkTarget;
    protected RightHandIKTarget rightHandIkTarget;

    public WeaponItem leftHandWeapon;
    public WeaponItem rightHandWeapon;

    public bool isUsingLeftHand;
    public bool isUsingRightHand;

    public string[] weaponTypes;

    #endregion

    public void Initialize(Transform character)
    {
        WeaponHolderSlot[] weaponHolderSlots = character.GetComponentsInChildren<WeaponHolderSlot>();
        foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
        {
            if (weaponSlot.isLeftHandSlot)
            {
                leftHandSlot = weaponSlot;
            }
            else if (weaponSlot.isRightHandSlot)
            {
                rightHandSlot = weaponSlot;
            }
        }
    }

    #region Loaders

    public virtual bool LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft, bool isRight)
    {
        bool isLoaded = false;

        if (isLeft)
        {
            isLoaded = leftHandSlot.LoadWeaponModel(weaponItem);
            if (isLoaded)
            {
                leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            }
        }
        if (isRight)
        {
            isLoaded = rightHandSlot.LoadWeaponModel(weaponItem);
            if (isLoaded)
            {
                rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            }
        }

        if (isLoaded)
        {
            isUsingLeftHand = isLeft;
            isUsingRightHand = isRight;
        }

        return isLoaded;
    }

    public virtual void LoadTwoHandIK()
    {
        leftHandIkTarget = rightHandSlot.currentWeaponModel.GetComponentInChildren<LeftHandIKTarget>();
        rightHandIkTarget = rightHandSlot.currentWeaponModel.GetComponentInChildren<RightHandIKTarget>();
    }

    #endregion

    #region Setters

    public void SetDamageCollider(bool isLeft, DamageCollider damageCollider)
    {
        if (isLeft)
        {
            leftHandDamageCollider = damageCollider;
        }
        else
        {
            rightHandDamageCollider = damageCollider;
        }
    }

    #endregion

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
        if (leftHandDamageCollider != null)
            leftHandDamageCollider.Enable();
        if (rightHandDamageCollider != null)
            rightHandDamageCollider.Enable();
    }

    public void DisableDamageCollider()
    {
        if (leftHandDamageCollider != null)
            leftHandDamageCollider.Disable();
        if (rightHandDamageCollider != null)
            rightHandDamageCollider.Disable();
    }
        
    #endregion
}
