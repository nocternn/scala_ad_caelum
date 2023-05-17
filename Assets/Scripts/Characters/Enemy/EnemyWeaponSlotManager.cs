using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponSlotManager : CharacterWeaponSlotManager
{
    private EnemyManager _manager;

    public void Initialize()
    {
        Initialize(transform);
        weaponTypes = new[] { "Bow", "Spear", "Crossbow", "Katana", "Sword" };
    }
    
    public void SetManager(EnemyManager manager)
    {
        _manager = manager;
        //_manager.SetUsedWeaponType(GetCurrentWeaponType());
    }
}
