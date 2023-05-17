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
        
        public void SetManager(PlayerManager manager)
        {
                _manager = manager;
                _manager.animatorHandler.SetUsedWeaponType(GetCurrentWeaponType(), weaponTypes);
        }
}
