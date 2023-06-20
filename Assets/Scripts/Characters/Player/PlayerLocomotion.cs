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

	public void Initialize()
	{
		characterCollider = _manager.transform.GetComponent<CapsuleCollider>();
		characterColliderBlocker = _manager.transform.Find("Helpers")
			.Find("CombatColliders").Find("CharacterColliderBlocker").GetComponent<CapsuleCollider>();

		DisableCharacterCollision();
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
			if (_manager.isAiming)
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
		Quaternion targetRotation = Quaternion.Euler(0, _manager.GetCameraEulerAngles().y, 0);
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * delta);
	}
	private void HandleLockedRotation(float delta)
	{
		Quaternion targetRotation;
			
		if (_manager.inputHandler.dodgeFlag)
		{
			Vector3 targetDirection = GetTargetDirection(false);
			if (targetDirection == Vector3.zero)
				targetDirection = transform.forward;

			targetRotation = Quaternion.LookRotation(targetDirection);
		}
		else
		{
			Vector3 rotationDirection = Vector3.zero;
			rotationDirection = _manager.GetCameraPosition("lockOn") - transform.position;
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
		if (!_manager.inputHandler.dodgeFlag)
			return;
		
		Vector3 moveDirection = Vector3.zero;
		moveDirection  = _camera.forward * _manager.GetMovementInput("vertical");
		moveDirection += _camera.right * _manager.GetMovementInput("horizontal");

		if (_manager.inputHandler.moveAmount > 0)
		{
			_manager.PlayTargetAnimation("Rolling", true);
			
			moveDirection.y = 0;
			transform.rotation = Quaternion.LookRotation(moveDirection);
		}
		else
		{
			_manager.PlayTargetAnimation("Backstep", true);
		}

		_manager.inputHandler.dodgeInput = false;
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
			targetDirection  = _manager.GetCameraDirection("forward") * _manager.GetMovementInput("vertical");
			targetDirection += _manager.GetCameraDirection("right") * _manager.GetMovementInput("horizontal");
		}
		targetDirection.Normalize();
		targetDirection.y = 0;

		return targetDirection;
	}
}
