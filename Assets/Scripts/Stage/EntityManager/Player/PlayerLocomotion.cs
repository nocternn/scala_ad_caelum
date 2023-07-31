using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : CharacterLocomotion
{
	public new Rigidbody rigidbody;
	
	private void Awake()
	{
		rigidbody = GetComponent<Rigidbody>();
	}

	public void Initialize()
	{
		characterCollider = transform.GetComponent<CapsuleCollider>();
		characterColliderBlocker = transform.GetChild(2).GetChild(3).GetChild(0).GetComponent<CapsuleCollider>();

		DisableCharacterCollision();
	}
	
	public void HandleAllMovements(float delta)
	{
		if (PlayerManager.Instance.isInteracting)
			return;
		
		HandleMovement(delta);
		HandleDodging(delta);
		
		if (PlayerManager.Instance.canRotate)
			HandleRotation(delta);
	}
	
	#region Locomotion
	private Vector3 normalVector;
	private Vector3 targetPosition;
	
	private void HandleMovement(float delta)
	{
		Vector3 moveDirection = GetTargetDirection(true);
		rigidbody.velocity = Vector3.ProjectOnPlane(moveDirection * movementSpeed, normalVector);
	}

	private void HandleRotation(float delta)
	{
		if (PlayerManager.Instance.IsLockingOnTarget())
		{
			if (PlayerManager.Instance.isAiming)
			{
				HandleAimedRotation(delta);
			}
			else
			{
				HandleLockedRotation(delta);
			}
		}
		else
		{
			HandleStandardRotation(delta);
		}
	}
	private void HandleStandardRotation(float delta)
	{
		Vector3 targetDirection = GetTargetDirection(true);
		if (targetDirection == Vector3.zero)
			targetDirection = transform.forward;

		Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
		Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * delta);

		transform.rotation = playerRotation;
	}
	private void HandleAimedRotation(float delta)
	{
		Quaternion targetRotation = Quaternion.Euler(0, CameraManager.Instance.GetEulerAngles().y, 0);
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * delta);
	}
	private void HandleLockedRotation(float delta)
	{
		Quaternion targetRotation;
			
		if (PlayerManager.Instance.inputHandler.dodgeFlag)
		{
			Vector3 targetDirection = GetTargetDirection(false);
			if (targetDirection == Vector3.zero)
				targetDirection = transform.forward;

			targetRotation = Quaternion.LookRotation(targetDirection);
		}
		else
		{
			Vector3 rotationDirection = Vector3.zero;
			rotationDirection = CameraManager.Instance.GetPosition("lockOn") - transform.position;
			rotationDirection.Normalize();
			rotationDirection.y = 0;
				
			targetRotation = Quaternion.LookRotation(rotationDirection);
		}
			
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * delta);
	}
	#endregion
	
	#region Actions
	private void HandleDodging(float delta)
	{
		if (!PlayerManager.Instance.inputHandler.dodgeFlag)
			return;

		PlayerManager.Instance.PerformAction(Enums.CharacterActionType.Dodge);

		PlayerManager.Instance.inputHandler.dodgeInput = false;
	}
	#endregion

	private Vector3 GetTargetDirection(bool isUsingMainCamera)
	{
		Vector3 targetDirection = Vector3.zero;
		if (isUsingMainCamera)
		{
			targetDirection  = CameraManager.Instance.GetDirection("forward") * PlayerManager.Instance.GetMovementInput("vertical");
			targetDirection += CameraManager.Instance.GetDirection("right") * PlayerManager.Instance.GetMovementInput("horizontal");
		}
		else
		{
			targetDirection  = CameraManager.Instance.GetDirection("forward") * PlayerManager.Instance.GetMovementInput("vertical");
			targetDirection += CameraManager.Instance.GetDirection("right") * PlayerManager.Instance.GetMovementInput("horizontal");
		}
		targetDirection.Normalize();
		targetDirection.y = 0;

		return targetDirection;
	}
}
