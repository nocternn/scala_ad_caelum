using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponSlotManager : CharacterWeaponSlotManager
{
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
                if (!PlayerManager.Instance.isTwoHanding)
                        return;
                base.LoadTwoHandIK();
                PlayerManager.Instance.animatorHandler.SetHandIK(
                        leftHandIkTarget, rightHandIkTarget,
                        PlayerManager.Instance.isTwoHanding);
        }
        public void SetUsedWeaponType()
        {
                string currentWeaponType = GetCurrentWeaponType();
                
                PlayerManager.Instance.animatorHandler.SetUsedWeaponType(currentWeaponType, weaponTypes);

                if (currentWeaponType.Equals(weaponTypes[0])
                    || currentWeaponType.Equals(weaponTypes[1])
                    || currentWeaponType.Equals(weaponTypes[4]))
                {
                        PlayerManager.Instance.isTwoHanding = true;
                }
                else
                {
                        PlayerManager.Instance.isTwoHanding = false;
                }
        }
}
