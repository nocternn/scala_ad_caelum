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
