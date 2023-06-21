using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerManager : CharacterManager
{
    #region Attributes
    
    public PlayerAnimatorHandler animatorHandler;
    public PlayerAttacker attacker;
    public PlayerInputHandler inputHandler;
    public PlayerLocomotion locomotion;
    public PlayerStats stats;
    public PlayerWeaponSlotManager weaponSlotManager;

    public bool canDoCombo;
    public bool isHit = false;

    public Transform aimTarget;

    #endregion

    public void Initialize()
    {
        _stage.camera.SetPlayerManager(this);

        animatorHandler   = GetComponent<PlayerAnimatorHandler>();
        attacker          = GetComponent<PlayerAttacker>();
        inputHandler      = GetComponent<PlayerInputHandler>();
        locomotion        = GetComponent<PlayerLocomotion>();
        stats             = GetComponent<PlayerStats>();
        weaponSlotManager = GetComponent<PlayerWeaponSlotManager>();

        rigBuilder = GetComponent<RigBuilder>();
        
        animatorHandler.Initialize();
        weaponSlotManager.Initialize();

        animatorHandler.SetManager(this);
        attacker.SetManager(this);
        inputHandler.SetManager(this);
        locomotion.SetManager(this);
        stats.SetManager(this);
        weaponSlotManager.SetManager(this);

        locomotion.Initialize();

        stats.CalculateCritChance(_stage.id);

        var foundAims = GameObject.FindObjectsOfType<MultiAimConstraint>(true);
        foreach (MultiAimConstraint foundAim in foundAims)
        {
            if (foundAim.transform.root.name.Equals("Player"))
            {
                aim = foundAim; 
                break;
            }
        }
        aimTarget = transform.Find("Helpers").Find("AimTarget").transform;
    }

    #region Setters

    public void SetWeapon(WeaponItem weapon)
    {
        if (weapon.name.Equals(weaponSlotManager.weaponTypes[2])) // Gauntlet - left hand
        {
            weaponSlotManager.leftHandWeapon = weapon;
            weaponSlotManager.rightHandWeapon = null;
        }
        else // All other weapons - right hand
        {
            weaponSlotManager.leftHandWeapon = null;
            weaponSlotManager.rightHandWeapon = weapon;
        }
        
        weaponSlotManager.LoadWeaponOnSlot(weaponSlotManager.leftHandWeapon, true, false);
        weaponSlotManager.LoadWeaponOnSlot(weaponSlotManager.rightHandWeapon, false, true);

        weaponSlotManager.SetUsedWeaponType();
        weaponSlotManager.LoadTwoHandIK();

        weapon.SetSkills(transform.Find("Helpers").Find("SkillsHolder").gameObject, _stage.player, _stage.enemy);
        attacker.currentWeapon = weapon;
    }

    public void SetCameraHeight()
    {
        _stage.camera.SetCameraHeight(Time.deltaTime);
    }

    public void ToggleCrosshair()
    {
        aimTarget.gameObject.SetActive(isAiming);
        _stage.hud.hudCombat.ToggleCrosshair(isAiming);
    }
    
    #endregion
    
    #region Getters

    public Camera GetCamera()
    {
        return _stage.camera.cameraTransform.GetComponent<Camera>();
    }

    public Vector3 GetCameraDirection(string direction = "none")
    {
        if (direction.Equals("forward"))
            return _stage.camera.cameraTransform.forward;
        if (direction.Equals("right"))
            return _stage.camera.cameraTransform.right;
        return Vector3.zero;
    }

    public Quaternion GetCameraRotation(string target = "camera")
    {
        if (target.Equals("pivot"))
            return _stage.camera.cameraPivotTransform.rotation;
        if (target.Equals("lockOn"))
            return _stage.camera.currentLockOnTarget.rotation;
        return _stage.camera.cameraTransform.rotation;
    }
    
    public Vector3 GetCameraEulerAngles(string target = "camera")
    {
        if (target.Equals("pivot"))
            return _stage.camera.cameraPivotTransform.eulerAngles;
        if (target.Equals("lockOn"))
            return _stage.camera.currentLockOnTarget.eulerAngles;
        return _stage.camera.cameraTransform.eulerAngles;
    }
    public Vector3 GetCameraLocalEulerAngles(string target = "camera")
    {
        if (target.Equals("pivot"))
            return _stage.camera.cameraPivotTransform.localEulerAngles;
        if (target.Equals("lockOn"))
            return _stage.camera.currentLockOnTarget.localEulerAngles;
        return _stage.camera.cameraTransform.localEulerAngles;
    }
    
    public Vector3 GetCameraPosition(string target = "camera")
    {
        if (target.Equals("pivot"))
            return _stage.camera.cameraPivotTransform.position;
        if (target.Equals("lockOn"))
            return _stage.camera.currentLockOnTarget.position;
        return _stage.camera.cameraTransform.position;
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
    
    public void HandleAttack(string tag)
    {
        if (tag.Equals("active"))
        {
            attacker.HandleActiveAttack();
        }
        else if (tag.Equals("basic"))
        {
            attacker.HandleBasicAttack();
        }
        else if (tag.Equals("charged"))
        {
            attacker.HandleChargedAttack();
        }
        else if (tag.Equals("combo"))
        {
            attacker.HandleWeaponCombo();
        }
        else if (tag.Equals("ultimate"))
        {
            attacker.HandleUltimateAttack();
        }
    }

    public void UpdateUI(WeaponItem weapon)
    {
        _stage.hud.UpdateUI(weapon);
    }

    public void OpenDoor()
    {
        _stage.hud.hudStage.OpenDoor();
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

    public void Interact()
    {
        _stage.Interact();
    }

    public void Lose()
    {
        _stage.EndStageLoss();
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
