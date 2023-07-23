using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Weapon Actions/Fire Projectile")]
public class FireAction : WeaponAction
{
    [Header("Projectiles")]
    [SerializeField] private GameObject _playerAmmo;
    [SerializeField] private GameObject _enemyAmmo;
    
    [Header("Specific Properties")]
    [SerializeField] private bool _playerHasShot;
    [SerializeField] private bool _enemyHasShot;
    
    protected override void Awake()
    {
        type = Enums.ActionType.Shoot;
    }
    
    public override void PerformAction(PlayerManager player, bool playAnimation = true)
    {
        // Get weapon
        WeaponItem weapon = player.weaponSlotManager.GetCurrentWeapon();
        
        if (!_playerHasShot)
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
            _playerAmmo = ammo;
            _playerAmmo.gameObject.SetActive(false);
        
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
            _playerAmmo.gameObject.SetActive(true);
            if (player.isAiming)
            {
                Ray ray = CameraManager.Instance.currentCamera.camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                RaycastHit hitpoint;

                if (Physics.Raycast(ray, out hitpoint, 100.0f))
                {
                    _playerAmmo.transform.LookAt(hitpoint.point);
                }
                else
                {
                    _playerAmmo.transform.rotation = Quaternion.Euler(
                        CameraManager.Instance.GetLocalEulerAngles().x,
                        player.lockOnTransform.eulerAngles.y,
                        0
                    );
                }
            }
            else
            {
                _playerAmmo.transform.rotation = Quaternion.Euler(
                    CameraManager.Instance.GetEulerAngles("pivot").x,
                    player.lockOnTransform.eulerAngles.y,
                    0
                );
            }
        
            // Set ammo velocity
            Rigidbody rigidbody = _playerAmmo.GetComponent<Rigidbody>();
            rigidbody.AddForce(_playerAmmo.transform.forward * weapon.ammo.forwardVelocity);
            rigidbody.AddForce(_playerAmmo.transform.up * weapon.ammo.upwardVelocity);
            rigidbody.useGravity = weapon.ammo.useGravity;
            rigidbody.mass = weapon.ammo.mass;
            _playerAmmo.transform.parent = null;
        }

        _playerHasShot = !_playerHasShot;
        base.PerformAction(player, playAnimation);
    }

    public override void PerformAction(EnemyManager enemy, bool playAnimation = true)
    {
        // Get weapon
        WeaponItem weapon = enemy.weaponSlotManager.GetCurrentWeapon();
        
        if (!_enemyHasShot)
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
            _enemyAmmo = ammo;
            _enemyAmmo.gameObject.SetActive(false);

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
            _enemyAmmo.gameObject.SetActive(true);
            _enemyAmmo.transform.rotation = Quaternion.LookRotation(enemy.weaponSlotManager.rightHandSlot.transform.up);

            // Set ammo velocity
            Rigidbody rigidbody = _enemyAmmo.GetComponent<Rigidbody>();
            rigidbody.AddForce(_enemyAmmo.transform.forward * weapon.ammo.forwardVelocity);
            rigidbody.AddForce(_enemyAmmo.transform.up * weapon.ammo.upwardVelocity);
            rigidbody.useGravity = weapon.ammo.useGravity;
            rigidbody.mass = weapon.ammo.mass;
            _enemyAmmo.transform.parent = null;
        }

        _enemyHasShot = !_enemyHasShot;
        base.PerformAction(enemy, playAnimation);
    }
}
