using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Weapon Actions/Fire Projectile")]
public class FireAction : WeaponAction
{
    protected override void Awake()
    {
        type = Enums.ActionType.Shoot;
    }
    
    public override void PerformAction(PlayerManager player, bool playAnimation = true)
    {
        // Get instantion location
        AmmoInstantiationLocation instantiationLocation =
            player.weaponSlotManager.rightHandSlot.GetComponentInChildren<AmmoInstantiationLocation>();
        // Get weapon
        WeaponItem weapon = player.weaponSlotManager.GetCurrentWeapon();

        // Instantiate ammo
        GameObject ammo = Instantiate(
            weapon.ammo.model,
            instantiationLocation.transform.position,
            player.GetCameraRotation("pivot")
        );
        Rigidbody rigidbody = ammo.GetComponent<Rigidbody>();
        RangedDamageCollider damageCollider = ammo.GetComponentInChildren<RangedDamageCollider>();
        
        // Fire projectile
        if (playAnimation)
            player.PlayTargetAnimation(_animation, true);
        
        // Set ammo velocity
        if (player.isAiming)
        {
            Ray ray = CameraManager.Instance.currentCamera.camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hitpoint;

            if (Physics.Raycast(ray, out hitpoint, 100.0f))
            {
                ammo.transform.LookAt(hitpoint.point);
            }
            else
            {
                ammo.transform.rotation = Quaternion.Euler(
                    player.GetCameraLocalEulerAngles().x,
                    player.lockOnTransform.eulerAngles.y,
                    0
                );
            }
        }
        else
        {
            ammo.transform.rotation = Quaternion.Euler(
                player.GetCameraEulerAngles("pivot").x,
                player.lockOnTransform.eulerAngles.y,
                0
            );
        }
        rigidbody.AddForce(ammo.transform.forward * weapon.ammo.forwardVelocity);
        rigidbody.AddForce(ammo.transform.up * weapon.ammo.upwardVelocity);
        rigidbody.useGravity = weapon.ammo.useGravity;
        rigidbody.mass = weapon.ammo.mass;
        ammo.transform.parent = null;

        // Enable damage
        damageCollider.ammoItem = weapon.ammo;
        player.weaponSlotManager.SetDamageCollider(false, damageCollider);
        player.weaponSlotManager.EnableDamageCollider();
        
        base.PerformAction(player, playAnimation);
    }
    
    public override void PerformAction(EnemyManager enemy, bool playAnimation = true)
    {
        // Get instantion location
        AmmoInstantiationLocation instantiationLocation;
        instantiationLocation = enemy.weaponSlotManager.rightHandSlot.GetComponent<AmmoInstantiationLocation>();
        if (instantiationLocation == null)
            instantiationLocation = enemy.weaponSlotManager.rightHandSlot.GetComponentInChildren<AmmoInstantiationLocation>();
        // Get weapon
        WeaponItem weapon = enemy.weaponSlotManager.GetCurrentWeapon();
        
        GameObject ammo = Instantiate(weapon.ammo.model, instantiationLocation.transform);
        Rigidbody rigidbody = ammo.GetComponent<Rigidbody>();
        RangedDamageCollider damageCollider = ammo.GetComponentInChildren<RangedDamageCollider>();
        
        // Set ammo direction
        ammo.transform.rotation = Quaternion.LookRotation(enemy.weaponSlotManager.rightHandSlot.transform.up);

        // Set ammo velocity
        rigidbody.AddForce(ammo.transform.forward * weapon.ammo.forwardVelocity);
        rigidbody.AddForce(ammo.transform.up * weapon.ammo.upwardVelocity);
        rigidbody.useGravity = weapon.ammo.useGravity;
        rigidbody.mass = weapon.ammo.mass;
        ammo.transform.parent = null;

        // Enable damage
        damageCollider.ammoItem = weapon.ammo;
        enemy.weaponSlotManager.SetDamageCollider(false, damageCollider);
        
        base.PerformAction(enemy, playAnimation);
    }
}
