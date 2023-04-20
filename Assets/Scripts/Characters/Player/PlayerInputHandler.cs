using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerManager _playerManager;
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
	    if (attackActiveInput)
		{
			_playerManager.HandleAttack("active", _playerManager.GetCurrentWeapon());
		}
		else if (attackBasicInput)
		{
			if (_playerManager.canDoCombo)
			{
				comboFlag = true;
				_playerManager.HandleAttack("combo", _playerManager.GetCurrentWeapon());
				comboFlag = false;
			}
			else
			{
				if (_playerManager.isInteracting)
					return;
				_playerManager.HandleAttack("basic", _playerManager.GetCurrentWeapon());
			}
		}
		else if (attackChargedInput)
		{
			if (!_playerManager.IsAttackFullCharge())
				return;
			_playerManager.HandleAttack("charged", _playerManager.GetCurrentWeapon());
			_playerManager.UpdateAttackCharge(false);
		}
		else if (attackUltimateInput)
		{
			_playerManager.HandleAttack("ultimate", _playerManager.GetCurrentWeapon());
		}
    }

    private void HandleLockOnInput()
    {
	    if (lockOnInput && lockOnFlag == false) // Lock on
	    {
		    lockOnInput = false;
		    
		    _playerManager.ClearLockOnTargets();
		    _playerManager.HandleLockOn();
	    }
	    else if (lockOnInput && lockOnFlag)	// Lock off
	    {
		    lockOnInput = false;
		    lockOnFlag = false;

		    _playerManager.ClearLockOnTargets();
	    }
    }
    
    public void SetManager(PlayerManager manager)
    {
	    _playerManager = manager;
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
