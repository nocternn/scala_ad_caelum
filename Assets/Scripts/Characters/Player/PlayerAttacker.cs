using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    private string lastAttack;
    public WeaponItem currentWeapon;

    public void Initialize()
    {
        currentWeapon = PlayerManager.Instance.weaponSlotManager.GetCurrentWeapon();
    }

    public void HandleWeaponCombo()
    {
        if (!PlayerManager.Instance.inputHandler.comboFlag)
            return;
        
        PlayerManager.Instance.animatorHandler.SetBool("canDoCombo", false);
        if (lastAttack.Equals(currentWeapon.basicAttack01))
        {
            PlayerManager.Instance.PlayTargetAnimation(currentWeapon.basicAttack02, true);
            lastAttack = currentWeapon.basicAttack02;
        }
        else if (lastAttack.Equals(currentWeapon.basicAttack02))
        {
            PlayerManager.Instance.PlayTargetAnimation(currentWeapon.basicAttack03, true);
            lastAttack = currentWeapon.basicAttack03;
        }
        else if (lastAttack.Equals(currentWeapon.basicAttack03))
        {
            PlayerManager.Instance.PlayTargetAnimation(currentWeapon.basicAttack04, true);
            lastAttack = currentWeapon.basicAttack04;
        }
    }

	public void HandleActiveAttack()
    {
        bool hasEnoughSP = PlayerManager.Instance.stats.currentSkillPoints >= currentWeapon.activeCost;
        bool canUseSkill = !currentWeapon.skillActive.onCooldown;

        if (hasEnoughSP && canUseSkill)
        {
            PlayerManager.Instance.stats.currentSkillPoints -= currentWeapon.activeCost;
            
            currentWeapon.skillActive.UseSkill();
            currentWeapon.skillActive.Cooldown();

            lastAttack = currentWeapon.activeAttack;
        }
    }
    
    public void HandleBasicAttack()
    {
        PlayerManager.Instance.PlayTargetAnimation(currentWeapon.basicAttack01, true);
        
        // Shoot bullet if weapon is pistol
        if (currentWeapon.name.Equals(PlayerManager.Instance.weaponSlotManager.weaponTypes[0]))
        {
            ShootBullet();
        }
        
        lastAttack = currentWeapon.basicAttack01;
    }

    public void HandleChargedAttack()
    {
        PlayerManager.Instance.PlayTargetAnimation(currentWeapon.chargedAttack, true);
        lastAttack = currentWeapon.chargedAttack;
    }

	public void HandleUltimateAttack()
    {
        bool hasEnoughSP = PlayerManager.Instance.stats.currentSkillPoints >= currentWeapon.ultimateCost;
        bool canUseSkill = !currentWeapon.skillUltimate.onCooldown;

        if (hasEnoughSP && canUseSkill)
        {
            PlayerManager.Instance.stats.currentSkillPoints -= currentWeapon.ultimateCost;
            
            currentWeapon.skillUltimate.UseSkill();
            currentWeapon.skillUltimate.Cooldown();

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
            PlayerManager.Instance.weaponSlotManager.rightHandSlot.GetComponentInChildren<AmmoInstantiationLocation>();

        // Instantiate ammo
        GameObject ammo = Instantiate(
            currentWeapon.ammo.model,
            instantiationLocation.transform.position,
            PlayerManager.Instance.GetCameraRotation("pivot")
        );
        Rigidbody rigidbody = ammo.GetComponent<Rigidbody>();
        RangedDamageCollider damageCollider = ammo.GetComponentInChildren<RangedDamageCollider>();
            
        // Set ammo velocity
        if (PlayerManager.Instance.isAiming)
        {
            Ray ray = CameraHandler.Instance.camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hitpoint;

            if (Physics.Raycast(ray, out hitpoint, 100.0f))
            {
                ammo.transform.LookAt(hitpoint.point);
            }
            else
            {
                ammo.transform.rotation = Quaternion.Euler(
                    PlayerManager.Instance.GetCameraLocalEulerAngles().x,
                    PlayerManager.Instance.lockOnTransform.eulerAngles.y,
                    0
                );
            }
        }
        else
        {
            ammo.transform.rotation = Quaternion.Euler(
                PlayerManager.Instance.GetCameraEulerAngles("pivot").x,
                PlayerManager.Instance.lockOnTransform.eulerAngles.y,
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
        PlayerManager.Instance.weaponSlotManager.SetDamageCollider(false, damageCollider);
        PlayerManager.Instance.weaponSlotManager.EnableDamageCollider();
    }
}
