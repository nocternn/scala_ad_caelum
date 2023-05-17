using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : CharacterManager
{
    #region Attributes

    public static string[] Types = new[] { "Archer", "Spearwoman", "Crossbowman", "Ninja", "Paladin" };

    [Header("Components")]
    public EnemyAnimatorHandler animatorHandler;
    public EnemyStats stats;
    public EnemyWeaponSlotManager weaponSlotManager;
    public NavMeshAgent navMeshAgent;
    public new Rigidbody rigidbody;

    [Header("A.I. Settings - Components")]
    public Transform currentEnemy;
    public CharacterManager currentTarget;
    public EnemyState currentState;

    [Header("A.I. Settings")]
    public float distanceFromTarget;
    public float viewableAngle;
    public float currentRecoveryTime = 0;
    public float maxAttackRange = 10;
    public float minDetectionAngle = -50;
    public float maxDetectionAngle = 50;

    #endregion
    
    #region Handlers

    private void HandleStateMachine()
    {
        if (currentState != null)
        {
            EnemyState nextState = currentState.Tick(this);
            if (nextState != null)
            {
                SwitchToNextState(nextState);
            }
        }
    }

    private void HandleRecoveryTime()
    {
        if (currentRecoveryTime > 0)
            currentRecoveryTime -= Time.deltaTime;

        if (isInteracting)
        {
            if (currentRecoveryTime <= 0)
                isInteracting = false;
        }
    }

    #endregion
    #region Updates
    
    private void Update()
    {
        HandleRecoveryTime();
    }
    
    private void FixedUpdate()
    {
        float delta = Time.deltaTime;

        isInteracting = animatorHandler.GetBool("isInteracting");
        canRotate     = animatorHandler.GetBool("canRotate");
        
        HandleStateMachine();
    }

    #endregion

    private void SwitchToNextState(EnemyState nextState)
    {
        currentState = nextState;
    }

    public void Initialize()
    {
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        rigidbody = GetComponent<Rigidbody>();

        navMeshAgent.enabled = false;
        rigidbody.isKinematic = false;
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
                
                animatorHandler = currentEnemy.GetComponent<EnemyAnimatorHandler>();
                animatorHandler.Initialize();
                animatorHandler.SetManager(this);
                animatorHandler.SetEnemyType(EnemyManager.Types[i]);
                
                stats = enemy.GetComponent<EnemyStats>();
                stats.CalculateCritChance(_stage.id);
                
                weaponSlotManager = currentEnemy.GetComponent<EnemyWeaponSlotManager>();
                weaponSlotManager.Initialize();
                weaponSlotManager.SetManager(this);

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
            animatorHandler.PlayTargetAnimation(String.Format("{0}_death", currentEnemy.name.ToLower()), true);
    }

    public void ResetNavMeshAgent()
    {
        navMeshAgent.transform.localPosition = Vector3.zero;
        navMeshAgent.transform.localRotation = Quaternion.identity;
    }

    public void UpdateAISettings()
    {
        if (currentTarget == null)
            return;
        
        Vector3 targetDirection = currentTarget.transform.position - transform.position;
        
        distanceFromTarget = Vector3.Distance(currentTarget.transform.position, transform.position);
        viewableAngle = Vector3.Angle(targetDirection, transform.position);
    }

    public bool IsInAttackRange()
    {
        UpdateAISettings();
        if (distanceFromTarget > maxAttackRange)
            return false;
        if (viewableAngle < minDetectionAngle || viewableAngle > maxDetectionAngle)
            return false;
        return true;
    }
}
