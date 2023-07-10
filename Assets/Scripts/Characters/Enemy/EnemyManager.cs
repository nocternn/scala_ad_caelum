using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class EnemyManager : CharacterManager
{
    #region Attributes
    
    public static EnemyManager Instance { get; private set; }
    
    [Header("Components")]
    public EnemyAnimatorHandler animatorHandler;
    public EnemyLocomotion locomotion;
    public EnemyStats stats;
    public EnemyWeaponSlotManager weaponSlotManager;
    public NavMeshAgent navMeshAgent;
    public new Rigidbody rigidbody;

    [Header("A.I. Settings - Components")]
    public Transform currentEnemy;
    public CharacterManager currentTarget;
    public EnemyState currentState;

	[Header("A.I. Settings - Actions")]
	public bool isBlocking;

    #endregion
    
    #region Lifecycle

    protected override void Awake()
    {
        base.Awake();
        
        Instance = this;

        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        locomotion = GetComponent<EnemyLocomotion>();
        rigidbody  = GetComponent<Rigidbody>();

        navMeshAgent.enabled = false;
        rigidbody.isKinematic = false;

        locomotion.Initialize();
		
		_hasDied = false;
		ToggleActive(false);
    }
    
    private void Update()
    {
        if (_hasDied || !_isActive)
            return;
        
        HandleRecoveryTime();
    }
    
    private void FixedUpdate()
    {
        if (_hasDied || !_isActive)
            return;
        
        float delta = Time.deltaTime;

        isInteracting = animatorHandler.GetBool("isInteracting");
        isBlocking    = animatorHandler.GetBool("isBlocking");
        canRotate     = animatorHandler.GetBool("canRotate");
        
        HandleStateMachine();
    }

    #endregion
    
    #region Handlers

    private void HandleStateMachine()
    {
        if (currentState != null)
        {
            EnemyState nextState = currentState.Tick(this);
            if (nextState != null)
            {
				nextState.UpdateVision(this);
        		currentState = nextState;
            }
        }
    }

    private void HandleRecoveryTime()
    {
        if (stats.currentRecoveryTime > 0)
            stats.currentRecoveryTime -= Time.deltaTime;

        if (isInteracting)
        {
            if (stats.currentRecoveryTime <= 0)
                isInteracting = false;
        }
    }

    #endregion

    #region Setters

    public void SetEnemyType(string enemyName)
    {
        Transform enemies = transform.Find("EnemyModel").transform;
        for (int i = 0; i < Dictionaries.EnemyType.Count; i++)
        {
            Transform enemy = enemies.GetChild(i);
            
            if (enemy.name.Equals(enemyName))
            {
                currentEnemy = enemy;
                currentEnemy.gameObject.SetActive(true);
                
                rigBuilder = currentEnemy.GetComponent<RigBuilder>();
                
                animatorHandler = currentEnemy.GetComponent<EnemyAnimatorHandler>();
                animatorHandler.Initialize();
                animatorHandler.SetEnemyType(enemyName);
                
                stats = enemy.GetComponent<EnemyStats>();
                stats.CalculateCritChance(StageManager.Instance.id);
                
                weaponSlotManager = currentEnemy.GetComponent<EnemyWeaponSlotManager>();
                weaponSlotManager.Initialize();
                weaponSlotManager.SetUsedWeaponType();
                weaponSlotManager.LoadTwoHandIK();

                var foundRigs = GameObject.FindObjectsOfType<Rig>(true);
                foreach (Rig foundRig in foundRigs)
                {
                    if (foundRig.transform.parent.name.Equals(enemyName))
                    {
                        rigLayer = foundRig; 
                        break;
                    }
                }

				if (weaponSlotManager.GetCurrentWeapon().combatType == Enums.WeaponCombatType.Ranged)
					PerformAction(Enums.ActionType.Aim);

                break;
            }
        }
    }

    #endregion

    public void TakeDamage(int damage)
    {
        stats.TakeDamage(damage);
        animatorHandler.PlayTargetAnimation(String.Format("{0}_damage", currentEnemy.name.ToLower()), true);

        if (stats.currentHealth == 0)
        {
            animatorHandler.PlayTargetAnimation(String.Format("{0}_death", currentEnemy.name.ToLower()), true);
            _hasDied = true;
            StageManager.Instance.EndStageWin();
        }
    }

    public void ResetNavMeshAgent()
    {
        navMeshAgent.transform.localPosition = Vector3.zero;
        navMeshAgent.transform.localRotation = Quaternion.identity;
    }

	public override void ToggleActive(bool state)
    {
        base.ToggleActive(state);

		if (currentEnemy != null)
		{
			Renderer[] renderers = currentEnemy.GetComponentsInChildren<Renderer>();
        	foreach (var renderer in renderers)
        	{
            	renderer.enabled = state;
        	}
		}
    }
}
