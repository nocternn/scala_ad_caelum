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

	#region Setters

	public override void SetHandIK(LeftHandIKTarget leftHandTarget, RightHandIKTarget rightHandTarget, bool isTwoHanding)
	{
		base.SetHandIK(leftHandTarget, rightHandTarget, isTwoHanding);
		PlayerManager.Instance.rigBuilder.Build();
	}

	#endregion
	
	#region Getters


	#endregion
}
