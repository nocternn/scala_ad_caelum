using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class PlayerInputHandler : MonoBehaviour
{
	#region Attributes

	[SerializeField] private PlayerControls _playerControls;
    
	[Header("Inputs")]
	public Vector2 movementInput;
	public Vector2 cameraInput;
	public float triggerInput = 0;
	public bool dodgeInput;
	public bool attackActiveInput, attackBasicInput, attackChargedInput, attackUltimateInput;
	public bool lockOnInput;
	public bool interactInput;
    
	[Header("Movement Input Data")]
	public float verticalMovementInput;
	public float horizontalMovementInput;
	public float moveAmount;

	[Header("Camera Input Data")]
	public float verticalCameraInput;
	public float horizontalCameraInput;

	[Header("Flags")]
	public bool dodgeFlag;
	public bool comboFlag;
	public bool lockOnFlag;

	#endregion

	#region Triggers

	private void OnEnable()
	{
		if (_playerControls == null)
		{
			_playerControls = new PlayerControls();
            
			// Movement
			_playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
			_playerControls.PlayerMovement.Movement.canceled += _ => movementInput = new Vector2(0, 0);

			// Camera
			_playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
			_playerControls.PlayerMovement.Camera.canceled += _ => movementInput = new Vector2(0, 0);
            
			// Action - Attacks
			_playerControls.PlayerActions.Attack.performed += context => {
				if (context.interaction is HoldInteraction)
				{
					attackChargedInput = true;
				}
				else
				{
					attackBasicInput = true;
				}
			};
			_playerControls.PlayerActions.AttackActive.performed += _ => attackActiveInput = true;
			_playerControls.PlayerActions.AttackUltimate.performed += _ => attackUltimateInput = true;
            
			// Action - Lock on target
			_playerControls.PlayerActions.LockOn.performed += _ => lockOnInput = true;
			
			// Action - Interact with object
			_playerControls.PlayerActions.Interact.performed += _ => interactInput = true;
		}

		_playerControls.Enable();
	}

	private void OnDisable()
	{
		_playerControls.Disable();
	}

	#endregion

	#region Handlers
    
    private void HandleMovementInput()
    {
        verticalMovementInput   = movementInput.y;
        horizontalMovementInput = movementInput.x;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalMovementInput) + Mathf.Abs(verticalMovementInput));
    }

    private void HandleCameraInput()
    {
        verticalCameraInput   = cameraInput.y;
        horizontalCameraInput = cameraInput.x;
    }

    private void HandleDodgeInput()
    {
        dodgeInput = _playerControls.PlayerActions.Dodge.phase == UnityEngine.InputSystem.InputActionPhase.Performed;
        if (dodgeInput)
        {
	        dodgeFlag = true;
        }
        else
        {
	        dodgeFlag = false;
        }
    }

    private void HandleAttackInput()
    {
	    if (attackActiveInput)
		{
			PlayerManager.Instance.HandleAttack("active");
		}
		else if (attackBasicInput)
		{
			if (PlayerManager.Instance.canDoCombo)
			{
				comboFlag = true;
				PlayerManager.Instance.HandleAttack("combo");
				comboFlag = false;
			}
			else
			{
				if (PlayerManager.Instance.isInteracting)
					return;
				PlayerManager.Instance.HandleAttack("basic");
			}
		}
		else if (attackChargedInput)
		{
			if (!PlayerManager.Instance.stats.IsFullCharge())
				return;
			PlayerManager.Instance.HandleAttack("charged");
			PlayerManager.Instance.stats.UpdateAttackCharge(false);
		}
		else if (attackUltimateInput)
		{
			PlayerManager.Instance.HandleAttack("ultimate");
		}
    }

    public void HandleLockOnInput()
    {
	    bool   usingPistol       = false;
	    string currentWeaponType = PlayerManager.Instance.weaponSlotManager.GetCurrentWeaponType();
	    if (currentWeaponType.Equals(PlayerManager.Instance.weaponSlotManager.weaponTypes[0]))
		    usingPistol = true;

	    if (lockOnInput && lockOnFlag == false) // Lock on
	    {
		    lockOnInput = false;

		    if (usingPistol)
			{
			    PlayerManager.Instance.isAiming = true;
            	CameraManager.Instance.SetCamera(Enums.CameraType.Aim);
			}
			else
			{
            	CameraManager.Instance.SetCamera(Enums.CameraType.Locked);
			}

		    CameraManager.Instance.currentCamera.ClearLockOnTargets();
		    PlayerManager.Instance.HandleLockOn();
	    }
	    else if (lockOnInput && lockOnFlag)	// Lock off
	    {
		    lockOnInput = false;
		    lockOnFlag = false;
		    
		    if (usingPistol)
			    PlayerManager.Instance.isAiming = false;
		    
		    CameraManager.Instance.currentCamera.ClearLockOnTargets();
            CameraManager.Instance.SetCamera(Enums.CameraType.Standard);
	    }

	    if (usingPistol)
	    {
		    PlayerManager.Instance.ToggleAim();
		    PlayerManager.Instance.ToggleCrosshair();
	    }
    }
    
    private void HandleInteractInput()
    {
	    if (interactInput)
	    {
		    interactInput = false;
		    StageManager.Instance.Interact();
	    }
    }

    public void HandleAllInputs(float delta)
    {
	    HandleMovementInput();
	    HandleCameraInput();

	    HandleDodgeInput();
	    HandleAttackInput();
	    HandleLockOnInput();
	    HandleInteractInput();
    }

    #endregion
}
