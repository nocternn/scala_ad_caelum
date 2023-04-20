using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlotManager : MonoBehaviour
{
        private PlayerManager _playerManager;
        
        private WeaponHolderSlot leftHandSlot;
        private WeaponHolderSlot rightHandSlot;

        private DamageCollider leftHandDamageCollider;
        private DamageCollider rightHandDamageCollider;

        void Awake()
        {
                WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
                foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
                {
                        if (weaponSlot.isLeftHandSlot)
                        {
                                leftHandSlot = weaponSlot;
                        }
                        else if (weaponSlot.isRightHandSlot)
                        {
                                rightHandSlot = weaponSlot;
                        }
                }
        }
        
        public void SetManager(PlayerManager manager)
        {
                _playerManager = manager;
        }

        public bool LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
        {
                bool isLoaded = false;
                
                if (isLeft)
                {
                        isLoaded = leftHandSlot.LoadWeaponModel(weaponItem);
                        if (isLoaded)
                        {
                                _playerManager.UpdateUI(weaponItem);
                                leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
                        }
                }
                else
                {
                        isLoaded = rightHandSlot.LoadWeaponModel(weaponItem);
                        if (isLoaded)
                        {
                                _playerManager.UpdateUI(weaponItem);
                                rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
                        }
                }

                return isLoaded;
        }

        #region Handle Damage Colliders

        public void EnableLeftDamageCollider()
        {
                leftHandDamageCollider.Enable();
        }
        public void EnableRightDamageCollider()
        {
                rightHandDamageCollider.Enable();
        }

        public void DisableLeftDamageCollider()
        {
                leftHandDamageCollider.Disable();
        }
        public void DisableRightDamageCollider()
        {
                rightHandDamageCollider.Disable();
        }
        
        #endregion

}
