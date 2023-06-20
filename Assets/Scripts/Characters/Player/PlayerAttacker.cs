using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    [SerializeField] private PlayerManager _manager;
    
    private string lastAttack;
    public WeaponItem currentWeapon;

    public void SetManager(PlayerManager manager)
    {
        _manager = manager;
        currentWeapon = manager.weaponSlotManager.GetCurrentWeapon();
    }

    public void HandleWeaponCombo()
    {
        if (!_manager.inputHandler.comboFlag)
            return;
        
        _manager.animatorHandler.SetBool("canDoCombo", false);
        if (lastAttack.Equals(currentWeapon.basic_attack_01))
        {
            _manager.PlayTargetAnimation(currentWeapon.basic_attack_02, true);
            lastAttack = currentWeapon.basic_attack_02;
        }
        else if (lastAttack.Equals(currentWeapon.basic_attack_02))
        {
            _manager.PlayTargetAnimation(currentWeapon.basic_attack_03, true);
            lastAttack = currentWeapon.basic_attack_03;
        }
        else if (lastAttack.Equals(currentWeapon.basic_attack_03))
        {
            _manager.PlayTargetAnimation(currentWeapon.basic_attack_04, true);
            lastAttack = currentWeapon.basic_attack_04;
        }
    }

	public void HandleActiveAttack()
    {
		Debug.Log("Use Active");
/*
        _manager.PlayTargetAnimation(currentWeapon.active_attack, true);
*/
    }
    
    public void HandleBasicAttack()
    {
        _manager.PlayTargetAnimation(currentWeapon.basic_attack_01, true);
        
        // Shoot bullet if weapon is pistol
        if (currentWeapon.name.Equals(_manager.weaponSlotManager.weaponTypes[0]))
        {
            ShootBullet();
        }
        
        lastAttack = currentWeapon.basic_attack_01;
    }

    public void HandleChargedAttack()
    {
        _manager.PlayTargetAnimation(currentWeapon.charged_attack, true);
        lastAttack = currentWeapon.charged_attack;
    }

	public void HandleUltimateAttack()
    {
		Debug.Log("Use Ultimate");
/*
        _manager.PlayTargetAnimation(currentWeapon.ultimate_attack, true);
*/
    }

	public bool IsBasicAttack()
	{
		return lastAttack.Contains("basic");
	}
    
    private void ShootBullet()
    {
        // Get instantion location
        AmmoInstantiationLocation instantiationLocation =
            _manager.weaponSlotManager.rightHandSlot.GetComponentInChildren<AmmoInstantiationLocation>();

        // Instantiate ammo
        GameObject ammo = Instantiate(
            currentWeapon.ammo.model,
            instantiationLocation.transform.position,
            _manager.GetCameraRotation("pivot")
        );
        Rigidbody rigidbody = ammo.GetComponent<Rigidbody>();
        RangedDamageCollider damageCollider = ammo.GetComponentInChildren<RangedDamageCollider>();
            
        // Set ammo velocity
        if (_manager.isAiming)
        {
            Ray ray = _manager.GetCamera().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hitpoint;

            if (Physics.Raycast(ray, out hitpoint, 100.0f))
            {
                ammo.transform.LookAt(hitpoint.point);
            }
            else
            {
                ammo.transform.rotation = Quaternion.Euler(
                    _manager.GetCameraLocalEulerAngles().x,
                    _manager.lockOnTransform.eulerAngles.y,
                    0
                );
            }
        }
        else
        {
            ammo.transform.rotation = Quaternion.Euler(
                _manager.GetCameraEulerAngles("pivot").x,
                _manager.lockOnTransform.eulerAngles.y,
                0
            );
        }
        rigidbody.AddForce(ammo.transform.forward * currentWeapon.ammo.forwardVelocity);
        rigidbody.AddForce(ammo.transform.up * currentWeapon.ammo.upwardVelocity);
        rigidbody.useGravity = currentWeapon.ammo.useGravity;
        rigidbody.mass = currentWeapon.ammo.mass;
        ammo.transform.parent = null;

        // Enable damage
        damageCollider.ammoItem = currentWeapon.ammo;
        _manager.weaponSlotManager.SetDamageCollider(false, damageCollider);
        _manager.weaponSlotManager.EnableDamageCollider();
    }
}
