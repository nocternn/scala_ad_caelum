using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : CharacterLocomotion
{
	private PlayerManager _manager;
	private Transform _camera;

	public new Rigidbody rigidbody;
	
	private void Awake()
	{
		_camera = Camera.main.transform;

		rigidbody = GetComponent<Rigidbody>();
	}

	public void SetManager(PlayerManager manager)
	{
		_manager = manager;
	}

	public void HandleAllMovements(float delta)
	{
		if (_manager.isInteracting)
			return;
		
		HandleMovement(delta);
		HandleDodging(delta);
		
		if (_manager.canRotate)
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
		if (_manager.IsLockingOnTarget())
		{
			Quaternion targetRotation;
			
			if (_manager.IsDodging())
			{
				Vector3 targetDirection = GetTargetDirection(false);
				if (targetDirection == Vector3.zero)
					targetDirection = transform.forward;

				targetRotation = Quaternion.LookRotation(targetDirection);
			}
			else
			{
				Vector3 rotationDirection = Vector3.zero;
				rotationDirection = _manager.GetLockOnTargetPosition() - transform.position;
				rotationDirection.Normalize();
				rotationDirection.y = 0;
				
				targetRotation = Quaternion.LookRotation(rotationDirection);
			}
			
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * delta);
		}
		else
		{
			Vector3 targetDirection = GetTargetDirection(true);
			if (targetDirection == Vector3.zero)
				targetDirection = transform.forward;

			Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
			Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * delta);

			transform.rotation = playerRotation;
		}
	}
	#endregion
	
	#region Actions
	private void HandleDodging(float delta)
	{
		if (!_manager.IsDodging())
			return;
		
		Vector3 moveDirection = Vector3.zero;
		moveDirection  = _camera.forward * _manager.GetMovementInput("vertical");
		moveDirection += _camera.right * _manager.GetMovementInput("horizontal");

		if (_manager.GetMoveAmount() > 0)
		{
			_manager.PlayTargetAnimation("Rolling", true);
			
			moveDirection.y = 0;
			transform.rotation = Quaternion.LookRotation(moveDirection);
		}
		else
		{
			_manager.PlayTargetAnimation("Backstep", true);
		}

		_manager.EndDodge();
	}
	#endregion

	private Vector3 GetTargetDirection(bool isUsingMainCamera)
	{
		Vector3 targetDirection = Vector3.zero;
		if (isUsingMainCamera)
		{
			targetDirection  = _camera.forward * _manager.GetMovementInput("vertical");
			targetDirection += _camera.right * _manager.GetMovementInput("horizontal");
		}
		else
		{
			targetDirection  = _manager.GetCameraValue("forward") * _manager.GetMovementInput("vertical");
			targetDirection += _manager.GetCameraValue("right") * _manager.GetMovementInput("horizontal");
		}
		targetDirection.Normalize();
		targetDirection.y = 0;

		return targetDirection;
	}
}
