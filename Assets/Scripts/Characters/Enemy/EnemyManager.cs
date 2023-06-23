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

    [Header("Properties")]
    [SerializeField] private bool hasDied = false;
    public static string[] Types = new[] { "Archer", "Spearwoman", "Crossbowman", "Ninja", "Paladin" };

    #endregion
    
    #region Lifecycle

    protected override void Awake()
    {
        base.Awake();
        
        Instance = this;
        Instance.gameObject.SetActive(false);
    }
    
    private void Update()
    {
        if (hasDied)
            return;
        
        HandleRecoveryTime();
    }
    
    private void FixedUpdate()
    {
        if (hasDied)
            return;
        
        float delta = Time.deltaTime;

        isInteracting = animatorHandler.GetBool("isInteracting");
        canRotate     = animatorHandler.GetBool("canRotate");
        
        HandleStateMachine();
    }

    #endregion
    
    #region Handlers

    private void HandleStateMachine()
    {
        if (currentState != null)
        {
            currentState.SetManager(this);

            EnemyState nextState = currentState.Tick(this);
            if (nextState != null)
            {
                SwitchToNextState(nextState);
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


    private void SwitchToNextState(EnemyState nextState)
    {
        currentState = nextState;
    }

    public void Initialize()
    {
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        locomotion = GetComponent<EnemyLocomotion>();
        rigidbody  = GetComponent<Rigidbody>();

        navMeshAgent.enabled = false;
        rigidbody.isKinematic = false;

        locomotion.Initialize();
    }

    #region Setters

    public void SetEnemyType(string enemyName)
    {
        Transform enemies = transform.Find("EnemyModel").transform;
        for (int i = 0; i < EnemyManager.Types.Length; i++)
        {
            Transform enemy = enemies.GetChild(i);
            
            if (enemy.name.Equals(enemyName))
            {
                currentEnemy = enemy;
                currentEnemy.gameObject.SetActive(true);
                
                rigBuilder = currentEnemy.GetComponent<RigBuilder>();
                
                animatorHandler = currentEnemy.GetComponent<EnemyAnimatorHandler>();
                animatorHandler.Initialize();
                animatorHandler.SetEnemyType(EnemyManager.Types[i]);
                
                stats = enemy.GetComponent<EnemyStats>();
                stats.CalculateCritChance(StageManager.Instance.id);
                
                weaponSlotManager = currentEnemy.GetComponent<EnemyWeaponSlotManager>();
                weaponSlotManager.Initialize();
                weaponSlotManager.SetUsedWeaponType();
                weaponSlotManager.LoadTwoHandIK();

                var foundAims = GameObject.FindObjectsOfType<MultiAimConstraint>(true);
                foreach (MultiAimConstraint foundAim in foundAims)
                {
                    if (foundAim.transform.parent.parent.name.Equals(enemyName))
                    {
                        aim = foundAim; 
                        break;
                    }
                }

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
            hasDied = true;
            StageManager.Instance.EndStageWin();
        }
    }

    public void ResetNavMeshAgent()
    {
        navMeshAgent.transform.localPosition = Vector3.zero;
        navMeshAgent.transform.localRotation = Quaternion.identity;
    }
}
