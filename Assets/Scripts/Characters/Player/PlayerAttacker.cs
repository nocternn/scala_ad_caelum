using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        if (lastAttack.Equals(currentWeapon.basicAttack01))
        {
            _manager.PlayTargetAnimation(currentWeapon.basicAttack02, true);
            lastAttack = currentWeapon.basicAttack02;
        }
        else if (lastAttack.Equals(currentWeapon.basicAttack02))
        {
            _manager.PlayTargetAnimation(currentWeapon.basicAttack03, true);
            lastAttack = currentWeapon.basicAttack03;
        }
        else if (lastAttack.Equals(currentWeapon.basicAttack03))
        {
            _manager.PlayTargetAnimation(currentWeapon.basicAttack04, true);
            lastAttack = currentWeapon.basicAttack04;
        }
    }

	public void HandleActiveAttack()
    {
        bool hasEnoughSP = _manager.stats.currentSkillPoints >= currentWeapon.activeCost;
        bool canUseSkill = currentWeapon.skillActive.isUsable;

        if (hasEnoughSP && canUseSkill)
        {
            currentWeapon.skillActive.isUsable = false;
            Task.Delay((int)Mathf.Round(1000 * currentWeapon.activeCooldown)).ContinueWith(t =>
            {
                currentWeapon.skillActive.isUsable = true;
            });

            _manager.stats.currentSkillPoints -= currentWeapon.activeCost;
            currentWeapon.skillActive.UseSkill();

            lastAttack = currentWeapon.activeAttack;
        }
    }
    
    public void HandleBasicAttack()
    {
        _manager.PlayTargetAnimation(currentWeapon.basicAttack01, true);
        
        // Shoot bullet if weapon is pistol
        if (currentWeapon.name.Equals(_manager.weaponSlotManager.weaponTypes[0]))
        {
            ShootBullet();
        }
        
        lastAttack = currentWeapon.basicAttack01;
    }

    public void HandleChargedAttack()
    {
        _manager.PlayTargetAnimation(currentWeapon.chargedAttack, true);
        lastAttack = currentWeapon.chargedAttack;
    }

	public void HandleUltimateAttack()
    {
        bool hasEnoughSP = _manager.stats.currentSkillPoints >= currentWeapon.ultimateCost;
        bool canUseSkill = currentWeapon.skillUltimate.isUsable;

        if (hasEnoughSP && canUseSkill)
        {
            currentWeapon.skillUltimate.isUsable = false;
            Task.Delay((int)Mathf.Round(1000 * currentWeapon.activeCooldown)).ContinueWith(t =>
            {
                currentWeapon.skillUltimate.isUsable = true;
            });

            _manager.stats.currentSkillPoints -= currentWeapon.ultimateCost;
            currentWeapon.skillUltimate.UseSkill();

            lastAttack = currentWeapon.ultimateAttack;
        }
    }

	public bool IsBasicAttack()
	{
		return lastAttack.Contains("basic");
	}
    
    public void ShootBullet()
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
