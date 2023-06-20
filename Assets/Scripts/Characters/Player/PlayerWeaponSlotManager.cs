using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponSlotManager : CharacterWeaponSlotManager
{
        private PlayerManager _manager;

        public void Initialize()
        {
                Initialize(transform);
                weaponTypes = new[] { "Pistol", "Greatsword", "Gauntlet", "Katana", "Scythe" };
        }

        public override bool LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft, bool isRight)
        {
                bool isLoaded = base.LoadWeaponOnSlot(weaponItem, isLeft, isRight);

                string weaponType = GetCurrentWeaponType();
                if (isLoaded && weaponType.Equals(weaponTypes[0]) && rightHandDamageCollider == null)
                {
                        rightHandDamageCollider = weaponItem.ammo.model.GetComponentInChildren<DamageCollider>();
                }

                return isLoaded;
        }
        
        public override void LoadTwoHandIK()
        {
                if (!_manager.isTwoHanding)
                        return;
                base.LoadTwoHandIK();
                _manager.animatorHandler.SetHandIK(leftHandIkTarget, rightHandIkTarget, _manager.isTwoHanding);
        }
        
        public void SetManager(PlayerManager manager)
        {
                _manager = manager;
        }

        public void SetUsedWeaponType()
        {
                string currentWeaponType = GetCurrentWeaponType();
                
                _manager.animatorHandler.SetUsedWeaponType(currentWeaponType, weaponTypes);

                if (currentWeaponType.Equals(weaponTypes[0])
                    || currentWeaponType.Equals(weaponTypes[1])
                    || currentWeaponType.Equals(weaponTypes[4]))
                {
                        _manager.isTwoHanding = true;
                }
                else
                {
                        _manager.isTwoHanding = false;
                }
        }
}
