using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateCombatStance : EnemyState
{
	[Header("Weapon")]
	[SerializeField] private WeaponItem _weapon;
	[SerializeField] private bool _getWeaponFlag;

    [Header("A.I. Settings - Components")]
    [SerializeField] private EnemyStateIdle idleState;
    [SerializeField] private EnemyStatePursueTarget pursueTargetState;
    [SerializeField] private EnemyStateAttack attackState;

    [Header("A.I. Settings - Combat Stance")]
    [SerializeField] private float horizontalMovement, verticalMovement;
    [SerializeField] private bool randomDestinationSet;

    public override EnemyState Tick()
    {
		if (!_getWeaponFlag)
		{
			_weapon = EnemyManager.Instance.weaponSlotManager.GetCurrentWeapon();
			_getWeaponFlag = true;
		}

		attackState.hasPerformedAttack = false;
        
        if (EnemyManager.Instance.isInteracting)	// If the A.I. is performing an action then stop all movements
        {
            EnemyManager.Instance.locomotion.Move(0, 0);
            return this;
        }
        EnemyManager.Instance.locomotion.Move(horizontalMovement, verticalMovement);

        // If the A.I. is too far from the target, return it to its pursue target state
        if (_distanceFromTarget > EnemyManager.Instance.stats.maxAttackRange
            || !idleState.IsTargetVisible(EnemyManager.Instance.currentTarget, _viewableAngle))
        {
            randomDestinationSet = false;
            return pursueTargetState;
        }

		// Randomize the walking pattern of the A.I. so that it circles the target
        if (!randomDestinationSet)
        {
            randomDestinationSet = true;

			switch (_weapon.combatType)
			{
				case Enums.WeaponCombatType.Melee:
					DecideCirclingActionMelee();
					break;
				case Enums.WeaponCombatType.Ranged:
					DecideCirclingActionRanged();
					break;
				default:
					break;
			}
        }

		// If the A.I. is currently doing nothing, roll for a character action
		if (EnemyManager.Instance.stats.currentRecoveryTime <= 0)
		{
			RollForActionChance();
			PerformAction();
		}
		
		// Keep rotating towards the target
        pursueTargetState.HandleRotation();

        // If no character action is performed,
		if (!EnemyManager.Instance.isInteracting)
		{
			// If there is a viable attack, then perform it
        	if (EnemyManager.Instance.stats.currentRecoveryTime <= 0 && attackState.currentAttack != null)
            {
           		randomDestinationSet = false;
            	return attackState;
        	}
			// Else, randomly get a new attack
			GetNewAttack();
		}

		return this;
    }

	#region Movement

	private void DecideCirclingActionMelee()
	{
		Tuple<float, float> result;
        
		float chance = UnityEngine.Random.Range(-1, 1);
		if (_distanceFromTarget >= EnemyManager.Instance.stats.maxAttackRange / 2)
		{
			if (chance < 0)
			{
				result = EnemyManager.Instance.locomotion.RunAroundTarget();
			}
			else
			{
				result = EnemyManager.Instance.locomotion.RunTowardsTarget();
			}
		}
		else
		{
			if (chance < 0)
			{
				result = EnemyManager.Instance.locomotion.WalkAroundTarget();
			}
			else
			{
				result = EnemyManager.Instance.locomotion.WalkTowardsTarget();
			}
		}

		verticalMovement = result.Item1;
		horizontalMovement = result.Item2;
	}

	private void DecideCirclingActionRanged()
	{
		Tuple<float, float> result;
        
		float chance = UnityEngine.Random.Range(-1, 1);
		if (_distanceFromTarget <= EnemyManager.Instance.stats.maxAttackRange / 2)
		{
			if (chance < 0)
			{
				result = EnemyManager.Instance.locomotion.RunAroundTarget();
			}
			else
			{
				result = EnemyManager.Instance.locomotion.RunAwayFromTarget();
			}
		}
		else
		{
			if (chance < 0)
			{
				result = EnemyManager.Instance.locomotion.WalkAroundTarget();
			}
			else
			{
				result = EnemyManager.Instance.locomotion.WalkAwayFromTarget();
			}
		}

		verticalMovement = result.Item1;
		horizontalMovement = result.Item2;
	}

	public Tuple<float, float> GetMovementValues()
	{
		return new Tuple<float, float>(horizontalMovement, verticalMovement);
	}

	#endregion
    
	#region Actions

	private void RollForActionChance()
	{
		CharacterAction[] actions = EnemyManager.Instance.GetActions();
		int chance = UnityEngine.Random.Range(0, 100);

		foreach(var action in actions)
		{
			if (chance <= action.chanceToBePerformed)
			{
				action.toBePerformed = true;
			}
			else
			{
				action.toBePerformed = false;
			}
		}
	}

	private void PerformAction()
	{
		// Get all actions that are marked as performeable
		CharacterAction[] actions = Array.FindAll(EnemyManager.Instance.GetActions(), action => action.toBePerformed);
		if (actions.Length > 0)
		{
			// Select random action from the array above and perform it
			int selectedActionIndex = UnityEngine.Random.Range(0, actions.Length - 1);
			actions[selectedActionIndex].PerformAction(EnemyManager.Instance);

			// Reset other actions
			foreach (var action in actions)
				action.toBePerformed = false;
		}
	}

	#endregion

    private void GetNewAttack()
    {
	    // Don't retrieve actives, ultimates and charges
	    WeaponAction[] actions = Array.FindAll(
			    _weapon.GetActions(),
			    action => action.type == Enums.WeaponActionType.Basic || action.type == Enums.WeaponActionType.Shoot
		    );
	    int maxScore = 0;
        foreach (WeaponAction attack in actions)
        {
            if ((_distanceFromTarget >= attack.minDistance)
                && (_distanceFromTarget <= attack.maxDistance)
                && (_viewableAngle >= attack.minAngle && _viewableAngle <= attack.maxAngle))
            {
                maxScore += attack.score;
            }
        }

        int randomScore = UnityEngine.Random.Range(0, maxScore);
        int tempScore = 0;
        
        foreach (WeaponAction attack in actions)
        {
            if ((_distanceFromTarget >= attack.minDistance)
                && (_distanceFromTarget <= attack.maxDistance)
                && (_viewableAngle >= attack.minAngle && _viewableAngle <= attack.maxAngle))
            {
                if (attackState.currentAttack != null)
                    return;

                tempScore += attack.score;
                if (tempScore > randomScore)
                    attackState.currentAttack = attack;
            }
        }
    }

	
}
