using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerManager _manager;
    private PlayerControls _playerControls;
    
    [Header("Inputs")]
    public Vector2 movementInput;
    public Vector2 cameraInput;
    public float triggerInput = 0;
    public bool dodgeInput;
    public bool attackActiveInput, attackBasicInput, attackChargedInput, attackUltimateInput;
    public bool lockOnInput;
    
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
        }

        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }
    
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
	    WeaponItem currentWeapon = _manager.weaponSlotManager.GetCurrentWeapon();
	    if (attackActiveInput)
		{
			_manager.HandleAttack("active", currentWeapon);
		}
		else if (attackBasicInput)
		{
			if (_manager.canDoCombo)
			{
				comboFlag = true;
				_manager.HandleAttack("combo", currentWeapon);
				comboFlag = false;
			}
			else
			{
				if (_manager.isInteracting)
					return;
				_manager.HandleAttack("basic", currentWeapon);
			}
		}
		else if (attackChargedInput)
		{
			if (!_manager.stats.IsFullCharge())
				return;
			_manager.HandleAttack("charged", currentWeapon);
			_manager.stats.UpdateAttackCharge(false);
		}
		else if (attackUltimateInput)
		{
			_manager.HandleAttack("ultimate", currentWeapon);
		}
    }

    private void HandleLockOnInput()
    {
	    if (lockOnInput && lockOnFlag == false) // Lock on
	    {
		    lockOnInput = false;
		    
		    _manager.ClearLockOnTargets();
		    _manager.HandleLockOn();
	    }
	    else if (lockOnInput && lockOnFlag)	// Lock off
	    {
		    lockOnInput = false;
		    lockOnFlag = false;

		    _manager.ClearLockOnTargets();
	    }
    }
    
    public void SetManager(PlayerManager manager)
    {
	    _manager = manager;
    }

    public void HandleAllInputs(float delta)
    {
	    HandleMovementInput();
	    HandleCameraInput();

	    HandleDodgeInput();
	    HandleAttackInput();
	    HandleLockOnInput();
    }
}
