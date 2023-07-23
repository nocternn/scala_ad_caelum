using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerManager : CharacterManager
{
    #region Attributes
    
    public static PlayerManager Instance { get; private set; }
    
    [Header("Components")]
    public PlayerAnimatorHandler animatorHandler;
    public PlayerAttacker attacker;
    public PlayerInputHandler inputHandler;
    public PlayerLocomotion locomotion;
    public PlayerStats stats;
    public PlayerWeaponSlotManager weaponSlotManager;
    
    [Header("Helpers")]
    public Transform aimTarget;
    
    [Header("Properties")]
    public bool canDoCombo;

    #endregion

    #region Lifecycle
    
    protected override void Awake()
    {
        base.Awake();
        Instance = this;
        
        var foundRigs = GameObject.FindObjectsOfType<Rig>(true);
        foreach (Rig foundRig in foundRigs)
        {
            if (foundRig.transform.root.name.Equals("Player"))
            {
                rigLayer = foundRig;
                break;
            }
        }
        aimTarget = transform.GetChild(2).GetChild(1);

        animatorHandler   = GetComponent<PlayerAnimatorHandler>();
        attacker          = GetComponent<PlayerAttacker>();
        inputHandler      = GetComponent<PlayerInputHandler>();
        locomotion        = GetComponent<PlayerLocomotion>();
        stats             = GetComponent<PlayerStats>();
        weaponSlotManager = GetComponent<PlayerWeaponSlotManager>();

        rigBuilder = GetComponent<RigBuilder>();
        rigBuilder.Clear();
        
        animatorHandler.Initialize();
        weaponSlotManager.Initialize();
        locomotion.Initialize();
        
        ToggleActive(false);
    }
    
    private void Update()
    {
        if (_hasDied || !_isActive)
            return;
        
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
        if (_hasDied || !_isActive)
            return;
        
        float delta = Time.deltaTime;

        isInteracting = animatorHandler.GetBool("isInteracting");
        canRotate     = animatorHandler.GetBool("canRotate");
        canDoCombo    = animatorHandler.GetBool("canDoCombo");
        
        locomotion.HandleAllMovements(delta);
        
        if (CameraManager.Instance != null)
        {
            CameraManager.Instance.currentCamera.FollowTarget(delta);
            CameraManager.Instance.currentCamera.HandleRotation(delta, inputHandler.horizontalCameraInput, inputHandler.verticalCameraInput);
        }
    }

    private void LateUpdate()
    {
        if (_hasDied || !_isActive)
            return;
        
        inputHandler.attackActiveInput = false;
        inputHandler.attackBasicInput = false;
        inputHandler.attackChargedInput = false;
        inputHandler.attackUltimateInput = false;
    }
    #endregion

    #region Setters

    public void SetWeapon(WeaponItem weapon)
    {
        weaponSlotManager.LoadWeaponOnSlot(weapon);
        weaponSlotManager.SetUsedWeaponType();
        weaponSlotManager.LoadTwoHandIK();
    }
    
    public override void ToggleActive(bool state)
    {
        base.ToggleActive(state);
        
        Renderer[] renderers = transform.GetChild(1).GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers)
        {
            renderer.enabled = state;
        }
    }
    
    #endregion
    
    #region Getters

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
        return (inputHandler.lockOnFlag || CameraManager.Instance.currentCamera.currentLockOnTarget != null);
    }

    #endregion

    #region Middlewares

    public void PlayTargetAnimation(string targetAnimation, bool isInteracting, bool isWeaponBased = false)
    {
        if (isWeaponBased)
        {
            animatorHandler.PlayTargetWeaponBasedAnimation(targetAnimation, isInteracting,
                weaponSlotManager.GetCurrentWeapon());
        }
        else
        {
            animatorHandler.PlayTargetAnimation(targetAnimation, isInteracting);
        }
    }
    
    public void HandleAttack(Enums.ActionType attack)
    {
        switch (attack)
        {
            case Enums.ActionType.Basic:
                attacker.HandleBasicAttack();
                break;
            case Enums.ActionType.Charged:
                attacker.HandleChargedAttack();
                break;
            case Enums.ActionType.Active:
                attacker.HandleActiveAttack();
                break;
            case Enums.ActionType.Ultimate:
                attacker.HandleUltimateAttack();
                break;
            case Enums.ActionType.Shoot:
                attacker.HandleShootAttack();
                break;
            case Enums.ActionType.Combo:
                attacker.HandleWeaponCombo();
                break;
            default:
                Debug.Log("[PlayerManager::HandleAttack] Invalid attack type.");
                break;
        }
    }

    public void HandleLockOn()
    {
        CameraManager.Instance.currentCamera.HandleLockOn();

        if (CameraManager.Instance.currentCamera.nearestLockOnTarget != null)
        {
            CameraManager.Instance.currentCamera.currentLockOnTarget = CameraManager.Instance.currentCamera.nearestLockOnTarget;
            inputHandler.lockOnFlag = true;
        }
		else
		{
            CameraManager.Instance.SetCamera(Enums.CameraType.Standard);
		}
    }

    #endregion
}
