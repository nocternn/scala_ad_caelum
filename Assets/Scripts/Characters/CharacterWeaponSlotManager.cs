using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWeaponSlotManager : MonoBehaviour
{
    #region Attributes

    [Header("Holder Slots")]
    public WeaponHolderSlot leftHandSlot;
    public WeaponHolderSlot rightHandSlot;

    [Header("Damage Colliders")]
    protected DamageCollider leftHandDamageCollider;
    protected DamageCollider rightHandDamageCollider;

    [Header("Hand IKs")]
    protected LeftHandIKTarget leftHandIkTarget;
    protected RightHandIKTarget rightHandIkTarget;

    [Header("Weapons")]
    public WeaponItem leftHandWeapon;
    public WeaponItem rightHandWeapon;
    
    [Header("Properties")]
    public bool isUsingLeftHand;
    public bool isUsingRightHand;

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

    public void SetDamage(int damage)
    {
        if (isUsingLeftHand)
        {
            leftHandWeapon.atk = damage;
        }
        else
        {
            rightHandWeapon.atk = damage;
        }
    }
    
    public virtual void ToggleShooting()
    {
        // Empty
    }

    #endregion

    #region Getters

    public WeaponItem GetCurrentWeapon()
    {
        if (isUsingLeftHand)
            return leftHandWeapon;
        return rightHandWeapon;
    }

    public int GetDamage()
    {
        if (isUsingLeftHand)
            return leftHandWeapon.atk;
        return rightHandWeapon.atk;
    }

    public int GetCrit()
    {
        if (isUsingLeftHand)
            return leftHandWeapon.crt;
        return rightHandWeapon.crt;
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
