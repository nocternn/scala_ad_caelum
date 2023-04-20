using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
	private PlayerManager _playerManager;
    private WeaponSlotManager _weaponSlotManager;

    public WeaponItem leftWeapon;
    public WeaponItem rightWeapon;

    void Awake()
    {
        _weaponSlotManager = GetComponent<WeaponSlotManager>();
    }

    void Start()
    {
        if (_weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true))
		{
			SetUsedWeaponType(leftWeapon);
        }
		if (_weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false))
		{
			SetUsedWeaponType(rightWeapon);
        }
    }
    
    public void SetManager(PlayerManager manager)
    {
	    _playerManager = manager;
    }

	void SetUsedWeaponType(WeaponItem weaponItem)
	{
		string[] weaponTypes = new[] { "Pistol", "Greatsword", "Gauntlet", "Katana", "Scythe" };
		foreach (string weaponType in weaponTypes)
		{
			if (weaponItem.itemName.Equals(weaponType))
			{
				_playerManager.SetUsedWeaponType(weaponType);
				break;
			}
		}
	}
}
