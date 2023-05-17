using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorHandler : CharacterAnimatorHandler
{
	private PlayerManager _manager;

	/*
	private void OnAnimatorMove()
	{
		if (_manager.isInteracting)
			return;

		float delta = Time.deltaTime;
		_manager.SetPlayerDrag(0);

		Vector3 deltaPosition = _manager.GetAnimatorDeltaPosition();
		deltaPosition.y = 0;
		_manager.SetPlayerVelocity(deltaPosition / delta);
	}
	*/
	
	public void SetManager(PlayerManager manager)
	{
		_manager = manager;
	}

	#region Play Animation

	public void PlayTargetWeaponBasedAnimation(string targetAnimation, bool isInteracting, string[] weaponTypes)
	{
		foreach (string weaponType in weaponTypes)
		{
			if (_animator.GetBool(String.Format("isUsing{0}", weaponType)))
			{
				PlayTargetAnimation(String.Format("{0}_{1}", weaponType.ToLower(), targetAnimation), isInteracting);
				break;
			}
		}
	}

	#endregion
	
	#region Setters

	public void SetUsedWeaponType(string currentWeaponType, string[] weaponTypes)
	{
		foreach (string weaponType in weaponTypes)
		{
			_animator.SetBool(String.Format("isUsing{0}", weaponType), false);
		}
		_animator.SetBool(String.Format("isUsing{0}", currentWeaponType), true);
	}

	#endregion
	#region Getters


	#endregion

	#region Toggles

	public void EnableCombo()
	{
		_animator.SetBool("canDoCombo", true);
	}
	public void DisableCombo()
	{
		_animator.SetBool("canDoCombo", false);
	}

	#endregion
}
