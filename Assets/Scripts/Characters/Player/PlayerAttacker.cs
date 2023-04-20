using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    private PlayerManager _playerManager;
    
    private string lastAttack;

    public void SetManager(PlayerManager manager)
    {
        _playerManager = manager;
    }

    public void HandleWeaponCombo(WeaponItem weaponItem)
    {
        if (!_playerManager.IsPerformingCombo())
            return;
        
        _playerManager.SetAnimatorBool("canDoCombo", false);
        if (lastAttack.Equals(weaponItem.basic_attack_01))
        {
            _playerManager.PlayTargetAnimation(weaponItem.basic_attack_02, true);
            lastAttack = weaponItem.basic_attack_02;
        }
        else if (lastAttack.Equals(weaponItem.basic_attack_02))
        {
            _playerManager.PlayTargetAnimation(weaponItem.basic_attack_03, true);
            lastAttack = weaponItem.basic_attack_03;
        }
        else if (lastAttack.Equals(weaponItem.basic_attack_03))
        {
            _playerManager.PlayTargetAnimation(weaponItem.basic_attack_04, true);
            lastAttack = weaponItem.basic_attack_04;
        }
    }

	public void HandleActiveAttack(WeaponItem weaponItem)
    {
		Debug.Log("Use Active");
/*
        _playerManager.PlayTargetAnimation(weaponItem.active_attack, true);
*/
    }
    
    public void HandleBasicAttack(WeaponItem weaponItem)
    {
        _playerManager.PlayTargetAnimation(weaponItem.basic_attack_01, true);
        lastAttack = weaponItem.basic_attack_01;
    }

    public void HandleChargedAttack(WeaponItem weaponItem)
    {
        _playerManager.PlayTargetAnimation(weaponItem.charged_attack, true);
        lastAttack = weaponItem.charged_attack;
    }

	public void HandleUltimateAttack(WeaponItem weaponItem)
    {
		Debug.Log("Use Ultimate");
/*
        _playerManager.PlayTargetAnimation(weaponItem.ultimate_attack, true);
*/
    }

	public bool IsBasicAttack()
	{
		return lastAttack.Contains("basic");
	}
}
