using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponSlotManager : CharacterWeaponSlotManager
{
        public void Initialize()
        {
                Initialize(transform);
        }

        #region Loaders

        public override bool LoadWeaponOnSlot(WeaponItem weaponItem)
        {
                bool isLoaded = base.LoadWeaponOnSlot(weaponItem);

                if (isLoaded && weaponItem.type == Enums.WeaponType.Pistol && rightHandDamageCollider == null)
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

        #endregion

        #region Setters
        
        public void SetUsedWeaponType()
        {
                WeaponItem weapon = GetCurrentWeapon();

                PlayerManager.Instance.animatorHandler.SetUsedWeaponType(Dictionaries.WeaponTypePlayer[weapon.type]);
                
                if (weapon.type == Enums.WeaponType.Pistol
                    || weapon.type == Enums.WeaponType.Greatsword
                    || weapon.type == Enums.WeaponType.Scythe)
                {
                        PlayerManager.Instance.isTwoHanding = true;
                }
                else
                {
                        PlayerManager.Instance.isTwoHanding = false;
                }
        }
        
        public override void ToggleShooting()
        {
                WeaponAction[] actions = GetCurrentWeapon().GetActions();
                foreach (var action in actions)
                {
                        if (action.type == Enums.ActionType.Shoot)
                                action.PerformAction(PlayerManager.Instance);
                }
        }

        #endregion
}
