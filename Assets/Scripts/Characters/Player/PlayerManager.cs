using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    public static PlayerManager singleton;

	private StageManager _stage;
    
    private PlayerAnimatorHandler _animatorHandler;
    private PlayerAttacker _attacker;
    private PlayerInputHandler _inputHandler;
    private PlayerInventory _inventory;
    private PlayerLocomotion _locomotion;
    private PlayerStats _stats;
    private WeaponSlotManager _weaponSlotManager;

    [SerializeField] private HUDManager _hudManager;

    public bool canDoCombo;
    
    public void Initialize()
    {
        singleton = this;

		_stage.cameraHandler.SetPlayerManager(singleton);

        _animatorHandler   = GetComponent<PlayerAnimatorHandler>();
        _attacker          = GetComponent<PlayerAttacker>();
        _inputHandler      = GetComponent<PlayerInputHandler>();
        _inventory         = GetComponent<PlayerInventory>();
        _locomotion        = GetComponent<PlayerLocomotion>();
        _stats             = GetComponent<PlayerStats>();
        _weaponSlotManager = GetComponent<WeaponSlotManager>();

        _animatorHandler.SetManager(singleton);
        _attacker.SetManager(singleton);
        _inputHandler.SetManager(singleton);
        _inventory.SetManager(singleton);
        _locomotion.SetManager(singleton);
        _stats.SetManager(singleton);
        _weaponSlotManager.SetManager(singleton);
    }

    #region Setters
	public void SetManager(StageManager stage)
	{
		_stage = stage;
	}

    public void SetPlayerVelocity(Vector3 velocity)
    {
        _locomotion.rigidbody.velocity = velocity;
    }
    public void SetPlayerDrag(float drag)
    {
        _locomotion.rigidbody.drag = drag;
    }
    public void EndDodge()
    {
        _inputHandler.dodgeInput = false;
    }
    public void SetUsedWeaponType(string trigger)
    {
        _animatorHandler.SetUsedWeaponType(trigger);
    }

    public void SetAnimatorBool(string key, bool value)
    {
        _animatorHandler.SetBool(key, value);
    }
    
    public void UpdateAttackCharge(bool isGain)
    {
        _stats.UpdateAttackCharge(isGain);
    }
        
    #endregion
    #region Getters

    public Vector3 GetCameraValue(string direction)
    {
        if (direction.Equals("forward"))
            return _stage.cameraHandler.cameraTransform.forward;
        if (direction.Equals("right"))
            return _stage.cameraHandler.cameraTransform.right;
        return Vector3.zero;
    }
    public Vector3 GetLockOnTargetPosition()
    {
        return _stage.cameraHandler.currentLockOnTarget.position;
    }
    
    public float GetMovementInput(string direction)
    {
        if (direction.Equals("vertical"))
            return _inputHandler.verticalMovementInput;
        if (direction.Equals("horizontal"))
            return _inputHandler.horizontalMovementInput;
        return 0;
    }
    public float GetMoveAmount()
    {
        return _inputHandler.moveAmount;
    }
    public bool IsPerformingCombo()
    {
        return _inputHandler.comboFlag;
    }
    public bool IsDodging()
    {
        return _inputHandler.dodgeFlag;
    }
    public bool IsLockingOnTarget()
    {
        return (_inputHandler.lockOnFlag || _stage.cameraHandler.currentLockOnTarget != null);
    }
    public Vector3 GetAnimatorDeltaPosition()
    {
        return _animatorHandler.GetDeltaPosition();
    }
    
    public WeaponItem GetCurrentWeapon()
    {
        if (_inventory.leftWeapon != null)
            return _inventory.leftWeapon;
        return _inventory.rightWeapon;
    }
    
    public bool IsAttackFullCharge()
    {
        return _stats.IsFullCharge();
    }
    #endregion

    #region Middlewares

    public void PlayTargetAnimation(string targetAnimation, bool isInteracting, bool isWeaponBased = false)
    {
        if (isWeaponBased)
        {
            _animatorHandler.PlayTargetWeaponBasedAnimation(targetAnimation, isInteracting);
        }
        else
        {
            _animatorHandler.PlayTargetAnimation(targetAnimation, isInteracting);
        }
    }
    
    public void HandleAttack(string tag, WeaponItem weapon)
    {
        if (tag.Equals("active"))
        {
            _attacker.HandleActiveAttack(weapon);
        }
        else if (tag.Equals("basic"))
        {
            _attacker.HandleBasicAttack(weapon);
        }
        else if (tag.Equals("charged"))
        {
            _attacker.HandleChargedAttack(weapon);
        }
        else if (tag.Equals("combo"))
        {
            _attacker.HandleWeaponCombo(weapon);
        }
        else if (tag.Equals("ultimate"))
        {
            _attacker.HandleUltimateAttack(weapon);
        }
    }
	public bool IsBasicAttack()
	{
		return _attacker.IsBasicAttack();
	}

    public void UpdateUI(WeaponItem weapon)
    {
        _hudManager.UpdateUI(weapon);
    }
    public void UpdateUI(string tag, int currentValue, int maxValue)
    {
        _hudManager.UpdateUI(tag, currentValue, maxValue);
    }

	public void TakeDamage(int damage)
	{
		_stats.TakeDamage(damage);
	}

    public void HandleLockOn()
    {
        _stage.cameraHandler.HandleLockOn();

        if (_stage.cameraHandler.nearestLockOnTarget != null)
        {
            _stage.cameraHandler.currentLockOnTarget = _stage.cameraHandler.nearestLockOnTarget;
            _inputHandler.lockOnFlag = true;
        }
    }
    public void ClearLockOnTargets()
    {
        _stage.cameraHandler.ClearLockOnTargets();
    }

    public void SetCameraHeight()
    {
        _stage.cameraHandler.SetCameraHeight(Time.deltaTime);
    }
    #endregion

    #region Updates
    private void Update()
    {
        _inputHandler.HandleAllInputs(Time.deltaTime);

        if (IsLockingOnTarget())
        {
            _animatorHandler.UpdateAnimatorValues(_inputHandler.horizontalMovementInput, _inputHandler.verticalMovementInput);
        }
        else
        {
            _animatorHandler.UpdateAnimatorValues(0, _inputHandler.moveAmount);
        }
    }

    private void FixedUpdate()
    {
        float delta = Time.deltaTime;

        isInteracting = _animatorHandler.GetBool("isInteracting");
        canRotate     = _animatorHandler.GetBool("canRotate");
        canDoCombo    = _animatorHandler.GetBool("canDoCombo");
        
        _locomotion.HandleAllMovements(delta);
        
        if (_stage.cameraHandler != null)
        {
            _stage.cameraHandler.FollowTarget(delta);
            _stage.cameraHandler.HandleRotation(delta, _inputHandler.horizontalCameraInput, _inputHandler.verticalCameraInput);
        }
    }

    private void LateUpdate()
    {
        _inputHandler.attackActiveInput = false;
        _inputHandler.attackBasicInput = false;
        _inputHandler.attackChargedInput = false;
        _inputHandler.attackUltimateInput = false;
    }
    #endregion

}
