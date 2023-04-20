using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Random;

public class EnemyManager : CharacterManager
{
    public static EnemyManager singleton;
    public static string[] Types = new[] { "Archer", "Spearwoman", "Crossbowman", "Ninja", "Paladin" };

    public Transform currentEnemy;
    
    [Header("Components")]
    public EnemyAnimatorHandler animatorHandler;
    public EnemyLocomotion locomotion; 
    public EnemyStats stats;

    [Header("Attacks")]
    public EnemyActionAttack currentAttack;

    #region Handlers

    private void HandleCurrentAction()
    {
        if (locomotion.currentTarget == null)
        {
            locomotion.HandleDetection();
        }
        else
        {
            locomotion.distanceFromTarget = Vector3.Distance(locomotion.currentTarget.transform.position, transform.position);
            if (locomotion.distanceFromTarget > stats.stoppingDistance)
            {
                locomotion.HandleMovement();
                locomotion.HandleRotation();
                locomotion.ResetNavMesh();
            }
            else
            {
                locomotion.HandleRotation();
                locomotion.ResetNavMesh();
                AttackTarget();
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
        
        HandleCurrentAction();
    }

    #endregion
    
    public void Initialize()
    {
        singleton = this;
        
        locomotion = GetComponent<EnemyLocomotion>();
        
        locomotion.SetManager(singleton);
    }

    public void SetEnemyType(string enemyName)
    {
        Transform enemies = transform.Find("EnemyModel").transform;
        for (int i = 0; i < EnemyManager.Types.Length; i++)
        {
            Transform enemy = enemies.GetChild(i);
            enemy.gameObject.SetActive(false);
            
            if (enemy.name.Equals(EnemyManager.Types[i]))
            {
                currentEnemy = enemy;
                currentEnemy.gameObject.SetActive(true);
                
                animatorHandler = currentEnemy.GetComponent<EnemyAnimatorHandler>();
                animatorHandler.SetManager(singleton);
                animatorHandler.SetEnemyType(EnemyManager.Types[i]);
                
                stats = enemy.GetComponent<EnemyStats>();

                break;
            }
        }
    }
    
    public void TakeDamage(int damage)
    {
        stats.TakeDamage(damage);
        animatorHandler.PlayTargetAnimation(String.Format("{0}_damage", currentEnemy.name.ToLower()), true);
    }

    #region Attacks

    private void GetNewAttack()
    {
        Vector3 targetsDirection = locomotion.currentTarget.transform.position - transform.position;
        float viewableAngle = Vector3.Angle(targetsDirection, transform.position);
        
        locomotion.distanceFromTarget = Vector3.Distance(locomotion.currentTarget.transform.position, transform.position);

        int maxScore = 0;
        foreach (EnemyActionAttack attack in stats.enemyAttacks)
        {
            if ((locomotion.distanceFromTarget >= attack.minDistance)
                && (locomotion.distanceFromTarget <= attack.maxDistance)
                && (viewableAngle >= attack.minAngle && viewableAngle <= attack.maxAngle))
            {
                maxScore += attack.score;
            }
        }

        int randomScore = Range(0, maxScore);
        int tempScore = 0;
        
        foreach (EnemyActionAttack attack in stats.enemyAttacks)
        {
            if ((locomotion.distanceFromTarget >= attack.minDistance)
                && (locomotion.distanceFromTarget <= attack.maxDistance)
                && (viewableAngle >= attack.minAngle && viewableAngle <= attack.maxAngle))
            {
                if (currentAttack != null)
                    return;

                tempScore += attack.score;
                if (tempScore > randomScore)
                    currentAttack = attack;
            }
        }
    }

    private void AttackTarget()
    {
        if (isInteracting)
            return;
        
        if (currentAttack == null)
        {
            GetNewAttack();
        }
        else
        {
            isInteracting = true;
            stats.currentRecoveryTime = currentAttack.recoveryTime;
            animatorHandler.PlayTargetAnimation(currentAttack.animation, true);
            currentAttack = null;
        }
    }

    #endregion
}
