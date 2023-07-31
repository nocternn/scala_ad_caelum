using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Weapon Actions/Fire Projectile")]
public class FireAction : WeaponAction
{
    [Header("Projectiles")]
    [SerializeField] private GameObject _ammo;

    [Header("Specific Properties")]
    [SerializeField] private bool _hasShot;

    protected override void Awake()
    {
        type = Enums.WeaponActionType.Shoot;
    }

    public override void PerformAction(PlayerManager player, bool playAnimation = true)
    {
        // Get weapon
        WeaponItem weapon = player.weaponSlotManager.GetCurrentWeapon();

        if (!_hasShot)
        {
            // Get instantion location
            AmmoInstantiationLocation instantiationLocation =
                player.weaponSlotManager.rightHandSlot.GetComponentInChildren<AmmoInstantiationLocation>();

            // Instantiate ammo
            GameObject ammo = Instantiate(
                weapon.ammo.model,
                instantiationLocation.transform.position,
                CameraManager.Instance.GetRotation("pivot")
            );
            RangedDamageCollider damageCollider = ammo.GetComponentInChildren<RangedDamageCollider>();
            // Set player ammo
            _ammo = ammo;
            _ammo.gameObject.SetActive(false);

            // Enable damage
            damageCollider.ammoItem = weapon.ammo;
            player.weaponSlotManager.SetDamageCollider(false, damageCollider);
            player.weaponSlotManager.EnableDamageCollider();

            // Fire projectile
            if (playAnimation)
                player.PlayTargetAnimation(_animation, true);
        }
        else
        {
            // Set ammo direction
            _ammo.gameObject.SetActive(true);
            if (player.isAiming)
            {
                _ammo.transform.LookAt(player.aimTarget);
            }
            else
            {
                _ammo.transform.rotation = Quaternion.Euler(
                    CameraManager.Instance.GetEulerAngles("pivot").x,
                    player.lockOnTransform.eulerAngles.y,
                    0
                );
            }

            // Set ammo velocity
            Rigidbody rigidbody = _ammo.GetComponent<Rigidbody>();
            rigidbody.AddForce(_ammo.transform.forward * weapon.ammo.forwardVelocity);
            rigidbody.AddForce(_ammo.transform.up * weapon.ammo.upwardVelocity);
            rigidbody.useGravity = weapon.ammo.useGravity;
            rigidbody.mass = weapon.ammo.mass;
            _ammo.transform.parent = null;
        }

        _hasShot = !_hasShot;
        base.PerformAction(player, playAnimation);
    }

    public override void PerformAction(EnemyManager enemy, bool playAnimation = true)
    {
        // Get weapon
        WeaponItem weapon = enemy.weaponSlotManager.GetCurrentWeapon();
        
        if (!_hasShot)
        {
            // Get instantion location
            AmmoInstantiationLocation instantiationLocation;
            instantiationLocation = enemy.weaponSlotManager.rightHandSlot.GetComponent<AmmoInstantiationLocation>();
            if (instantiationLocation == null)
                instantiationLocation =
                    enemy.weaponSlotManager.rightHandSlot.GetComponentInChildren<AmmoInstantiationLocation>();

            // Instantiate ammo
            GameObject ammo = Instantiate(weapon.ammo.model, instantiationLocation.transform);
            RangedDamageCollider damageCollider = ammo.GetComponentInChildren<RangedDamageCollider>();
            // Set enemy ammo
            _ammo = ammo;
            _ammo.gameObject.SetActive(false);

            // Enable damage
            damageCollider.ammoItem = weapon.ammo;
            damageCollider.tag = "Enemy";
            enemy.weaponSlotManager.SetDamageCollider(false, damageCollider);
            enemy.weaponSlotManager.EnableDamageCollider();

            // Fire projectile
            if (playAnimation)
                enemy.animatorHandler.PlayTargetAnimation(_animation, true);
        }
        else
        {
            // Set ammo direction
            _ammo.gameObject.SetActive(true);
            _ammo.transform.rotation = Quaternion.LookRotation(enemy.weaponSlotManager.rightHandSlot.transform.up);

            // Set ammo velocity
            Rigidbody rigidbody = _ammo.GetComponent<Rigidbody>();
            rigidbody.AddForce(_ammo.transform.forward * weapon.ammo.forwardVelocity);
            rigidbody.AddForce(_ammo.transform.up * weapon.ammo.upwardVelocity);
            rigidbody.useGravity = weapon.ammo.useGravity;
            rigidbody.mass = weapon.ammo.mass;
            _ammo.transform.parent = null;
        }

        _hasShot = !_hasShot;
        base.PerformAction(enemy, playAnimation);
    }
}
