using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    public PlayerAnimatorHandler animatorHandler;
    public PlayerAttacker attacker;
    public PlayerInputHandler inputHandler;
    public PlayerLocomotion locomotion;
    public PlayerStats stats;
    public PlayerWeaponSlotManager weaponSlotManager;

    public bool canDoCombo;
    public bool isHit = false;

    public void Initialize()
    {
        _stage.camera.SetPlayerManager(this);

        animatorHandler   = GetComponent<PlayerAnimatorHandler>();
        attacker          = GetComponent<PlayerAttacker>();
        inputHandler      = GetComponent<PlayerInputHandler>();
        locomotion        = GetComponent<PlayerLocomotion>();
        stats             = GetComponent<PlayerStats>();
        weaponSlotManager = GetComponent<PlayerWeaponSlotManager>();

        animatorHandler.Initialize();
        weaponSlotManager.Initialize();

        animatorHandler.SetManager(this);
        attacker.SetManager(this);
        inputHandler.SetManager(this);
        locomotion.SetManager(this);
        stats.SetManager(this);
        weaponSlotManager.SetManager(this);
        
        stats.CalculateCritChance(_stage.id);
    }

    #region Setters

    #endregion
    
    #region Getters

    public Vector3 GetCameraValue(string direction)
    {
        if (direction.Equals("forward"))
            return _stage.camera.cameraTransform.forward;
        if (direction.Equals("right"))
            return _stage.camera.cameraTransform.right;
        return Vector3.zero;
    }
    
    public float GetMovementInput(string direction)
    {
        if (direction.Equals("vertical"))
            return inputHandler.verticalMovementInput;
        if (direction.Equals("horizontal"))
            return inputHandler.horizontalMovementInput;
        return 0;
    }
    
    public bool IsLockingOnTarget()
    {
        return (inputHandler.lockOnFlag || _stage.camera.currentLockOnTarget != null);
    }
    public Vector3 GetLockOnTargetPosition()
    {
        return _stage.camera.currentLockOnTarget.position;
    }
    
    #endregion

    #region Middlewares

    public void PlayTargetAnimation(string targetAnimation, bool isInteracting, bool isWeaponBased = false)
    {
        if (isWeaponBased)
        {
            animatorHandler.PlayTargetWeaponBasedAnimation(targetAnimation, isInteracting,
                weaponSlotManager.weaponTypes);
        }
        else
        {
            animatorHandler.PlayTargetAnimation(targetAnimation, isInteracting);
        }
    }
    
    public void HandleAttack(string tag, WeaponItem weapon)
    {
        if (tag.Equals("active"))
        {
            attacker.HandleActiveAttack(weapon);
        }
        else if (tag.Equals("basic"))
        {
            attacker.HandleBasicAttack(weapon);
        }
        else if (tag.Equals("charged"))
        {
            attacker.HandleChargedAttack(weapon);
        }
        else if (tag.Equals("combo"))
        {
            attacker.HandleWeaponCombo(weapon);
        }
        else if (tag.Equals("ultimate"))
        {
            attacker.HandleUltimateAttack(weapon);
        }
    }

    public void UpdateUI(WeaponItem weapon)
    {
        _stage.hud.UpdateUI(weapon);
    }

    public void HandleLockOn()
    {
        _stage.camera.HandleLockOn();

        if (_stage.camera.nearestLockOnTarget != null)
        {
            _stage.camera.currentLockOnTarget = _stage.camera.nearestLockOnTarget;
            inputHandler.lockOnFlag = true;
        }
    }

    public void ClearLockOnTargets()
    {
        _stage.camera.ClearLockOnTargets();
    }

    public void SetCameraHeight()
    {
        _stage.camera.SetCameraHeight(Time.deltaTime);
    }

    #endregion

    #region Updates
    
    private void Update()
    {
        inputHandler.HandleAllInputs(Time.deltaTime);

        if (IsLockingOnTarget())
        {
            animatorHandler.UpdateAnimatorValues(inputHandler.horizontalMovementInput, inputHandler.verticalMovementInput);
        }
        else
        {
            animatorHandler.UpdateAnimatorValues(0, inputHandler.moveAmount);
        }
    }

    private void FixedUpdate()
    {
        float delta = Time.deltaTime;

        isInteracting = animatorHandler.GetBool("isInteracting");
        canRotate     = animatorHandler.GetBool("canRotate");
        canDoCombo    = animatorHandler.GetBool("canDoCombo");
        
        locomotion.HandleAllMovements(delta);
        
        if (_stage.camera != null)
        {
            _stage.camera.FollowTarget(delta);
            _stage.camera.HandleRotation(delta, inputHandler.horizontalCameraInput, inputHandler.verticalCameraInput);
        }
    }

    private void LateUpdate()
    {
        inputHandler.attackActiveInput = false;
        inputHandler.attackBasicInput = false;
        inputHandler.attackChargedInput = false;
        inputHandler.attackUltimateInput = false;
    }
    #endregion

}
