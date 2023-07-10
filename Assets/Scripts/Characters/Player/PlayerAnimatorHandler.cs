using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerAnimatorHandler : CharacterAnimatorHandler
{
	/*
	private void OnAnimatorMove()
	{
		if (PlayerManager.Instance.isInteracting)
			return;

		float delta = Time.deltaTime;
		PlayerManager.Instance.SetPlayerDrag(0);

		Vector3 deltaPosition = PlayerManager.Instance.GetAnimatorDeltaPosition();
		deltaPosition.y = 0;
		PlayerManager.Instance.SetPlayerVelocity(deltaPosition / delta);
	}
	*/

	public override void Initialize()
	{
		base.Initialize();
	}
	
	#region Play Animation

	public void PlayTargetWeaponBasedAnimation(string targetAnimation, bool isInteracting, WeaponItem weapon)
	{
		string weaponType = Dictionaries.WeaponTypePlayer[weapon.type];
		if (_animator.GetBool(String.Format("isUsing{0}", weaponType)))
		{
			PlayTargetAnimation(String.Format("{0}_{1}", weaponType.ToLower(), targetAnimation), isInteracting);
		}
	}

	#endregion
	
	#region Setters

	public void SetUsedWeaponType(string currentWeaponType)
	{
		foreach(KeyValuePair<Enums.WeaponType, string> type in Dictionaries.WeaponTypePlayer)
		{
			_animator.SetBool(String.Format("isUsing{0}", type.Value), false);
		}
		_animator.SetBool(String.Format("isUsing{0}", currentWeaponType), true);
	}

	public override void SetHandIK(LeftHandIKTarget leftHandTarget, RightHandIKTarget rightHandTarget, bool isTwoHanding)
	{
		base.SetHandIK(leftHandTarget, rightHandTarget, isTwoHanding);
		PlayerManager.Instance.rigBuilder.Build();
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
