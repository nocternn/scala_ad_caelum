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
    public Transform skillsHolder;
    
    [Header("Properties")]
    public bool canDoCombo;
    public bool isHit = false;

    #endregion

    #region Lifecycle
    
    protected override void Awake()
    {
        var foundAims = GameObject.FindObjectsOfType<MultiAimConstraint>(true);
        foreach (MultiAimConstraint foundAim in foundAims)
        {
            if (foundAim.transform.root.name.Equals("Player"))
            {
                aim = foundAim; 
                break;
            }
        }
        aimTarget    = transform.GetChild(2).GetChild(1);
        skillsHolder = transform.GetChild(2).GetChild(2);
        
        Instance = this;
        Instance.gameObject.SetActive(false);
    }
    
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
        
        if (CameraManager.Instance != null)
        {
            CameraManager.Instance.currentCamera.FollowTarget(delta);
            CameraManager.Instance.currentCamera.HandleRotation(delta, inputHandler.horizontalCameraInput, inputHandler.verticalCameraInput);
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

    public void Initialize()
    {
        animatorHandler   = GetComponent<PlayerAnimatorHandler>();
        attacker          = GetComponent<PlayerAttacker>();
        inputHandler      = GetComponent<PlayerInputHandler>();
        locomotion        = GetComponent<PlayerLocomotion>();
        stats             = GetComponent<PlayerStats>();
        weaponSlotManager = GetComponent<PlayerWeaponSlotManager>();

        rigBuilder = GetComponent<RigBuilder>();
        
        animatorHandler.Initialize();
        weaponSlotManager.Initialize();
        attacker.Initialize();
        locomotion.Initialize();

        stats.CalculateCritChance(StageManager.Instance.id);
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

        weapon.SetSkills(skillsHolder.gameObject);
        attacker.currentWeapon = weapon;
    }

    public void ToggleCrosshair()
    {
        aimTarget.gameObject.SetActive(isAiming);
        HUDManager.Instance.hudCombat.ToggleCrosshair(isAiming);
    }
    
    #endregion
    
    #region Getters

    public Vector3 GetCameraDirection(string direction = "none")
    {
        if (direction.Equals("forward"))
            return CameraManager.Instance.currentCamera.cameraTransform.forward;
        if (direction.Equals("right"))
            return CameraManager.Instance.currentCamera.cameraTransform.right;
        return Vector3.zero;
    }

    public Quaternion GetCameraRotation(string target = "camera")
    {
        if (target.Equals("pivot"))
            return CameraManager.Instance.currentCamera.cameraPivotTransform.rotation;
        if (target.Equals("lockOn"))
            return CameraManager.Instance.currentCamera.currentLockOnTarget.rotation;
        return CameraManager.Instance.currentCamera.cameraTransform.rotation;
    }
    
    public Vector3 GetCameraEulerAngles(string target = "camera")
    {
        if (target.Equals("pivot"))
            return CameraManager.Instance.currentCamera.cameraPivotTransform.eulerAngles;
        if (target.Equals("lockOn"))
            return CameraManager.Instance.currentCamera.currentLockOnTarget.eulerAngles;
        return CameraManager.Instance.currentCamera.cameraTransform.eulerAngles;
    }
    public Vector3 GetCameraLocalEulerAngles(string target = "camera")
    {
        if (target.Equals("pivot"))
            return CameraManager.Instance.currentCamera.cameraPivotTransform.localEulerAngles;
        if (target.Equals("lockOn"))
            return CameraManager.Instance.currentCamera.currentLockOnTarget.localEulerAngles;
        return CameraManager.Instance.currentCamera.cameraTransform.localEulerAngles;
    }
    
    public Vector3 GetCameraPosition(string target = "camera")
    {
        if (target.Equals("pivot"))
            return CameraManager.Instance.currentCamera.cameraPivotTransform.position;
        if (target.Equals("lockOn"))
            return CameraManager.Instance.currentCamera.currentLockOnTarget.position;
        return CameraManager.Instance.currentCamera.cameraTransform.position;
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
        return (inputHandler.lockOnFlag || CameraManager.Instance.currentCamera.currentLockOnTarget != null);
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
