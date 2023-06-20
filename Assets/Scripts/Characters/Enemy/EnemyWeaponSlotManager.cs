using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponSlotManager : CharacterWeaponSlotManager
{
    private EnemyManager _manager;

    public void Initialize()
    {
        Initialize(transform);
        weaponTypes = new[] { "Bow", "Spear", "Crossbow", "Katana", "Sword" };
        
        LoadWeaponOnSlot(leftHandWeapon, true, false);
        LoadWeaponOnSlot(rightHandWeapon, false, true);
    }
    
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
        if (!_manager.isTwoHanding)
            return;
        base.LoadTwoHandIK();
        _manager.animatorHandler.SetHandIK(leftHandIkTarget, rightHandIkTarget, _manager.isTwoHanding);
    }
    
    public void SetManager(EnemyManager manager)
    {
        _manager = manager;
    }
    
    public void SetUsedWeaponType()
    {
        string currentWeaponType = GetCurrentWeaponType();

        // Toggle two-handing for spear and crossbow
        if (currentWeaponType.Equals(weaponTypes[1]) || currentWeaponType.Equals(weaponTypes[2]))
        {
            _manager.isTwoHanding = true;
        }
        else
        {
            _manager.isTwoHanding = false;
        }

        // Toggle aim for bow and crossbow
        if (currentWeaponType.Equals(weaponTypes[0]) || currentWeaponType.Equals(weaponTypes[2]))
        {
            _manager.isAiming = true;
        }
        else
        {
            _manager.isAiming = false;
        }
    }
    
    public void ShootAmmo()
    {
        string currentWeaponType = GetCurrentWeaponType();
        WeaponItem weapon;
        if (currentWeaponType.Equals(weaponTypes[0]))
        {
            weapon = leftHandWeapon;
        }
        else
        {
            weapon = rightHandWeapon;
        }
        
        // Get instantion location
        AmmoInstantiationLocation instantiationLocation;
        instantiationLocation = rightHandSlot.GetComponent<AmmoInstantiationLocation>();
        if (instantiationLocation == null)
            instantiationLocation = rightHandSlot.GetComponentInChildren<AmmoInstantiationLocation>();
        
        GameObject ammo = Instantiate(weapon.ammo.model, instantiationLocation.transform);
        Rigidbody rigidbody = ammo.GetComponent<Rigidbody>();
        RangedDamageCollider damageCollider = ammo.GetComponentInChildren<RangedDamageCollider>();
        
		// Set ammo direction
		ammo.transform.rotation = Quaternion.LookRotation(rightHandSlot.transform.up);

        // Set ammo velocity
        rigidbody.AddForce(ammo.transform.forward * weapon.ammo.forwardVelocity);
        rigidbody.AddForce(ammo.transform.up * weapon.ammo.upwardVelocity);
        rigidbody.useGravity = weapon.ammo.useGravity;
        rigidbody.mass = weapon.ammo.mass;
        ammo.transform.parent = null;

        // Enable damage
        damageCollider.ammoItem = weapon.ammo;
        _manager.weaponSlotManager.SetDamageCollider(false, damageCollider);
    }
}
