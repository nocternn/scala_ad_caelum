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
                if (PlayerManager.Instance.isTwoHanding)
                {
                        base.LoadTwoHandIK();
                        PlayerManager.Instance.animatorHandler.SetHandIK(
                                leftHandIkTarget, rightHandIkTarget,
                                PlayerManager.Instance.isTwoHanding);
                }
        }

        #endregion

        #region Setters
        
        public void SetUsedWeaponType()
        {
                WeaponItem weapon = GetCurrentWeapon();

                PlayerManager.Instance.animatorHandler.SetUsedWeaponType(Dictionaries.WeaponTypePlayer[weapon.type]);
                PlayerManager.Instance.isTwoHanding = (weapon.handlingType == Enums.WeaponHandlingType.Both);
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
