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

    public override EnemyState Tick(EnemyManager manager)
    {
		if (!_getWeaponFlag)
		{
			_weapon = manager.weaponSlotManager.GetCurrentWeapon();
			_getWeaponFlag = true;
		}

		attackState.hasPerformedAttack = false;
        
        if (manager.isInteracting)	// If the A.I. is performing an action then stop all movements
        {
            manager.locomotion.Move(0, 0);
            return this;
        }
        manager.locomotion.Move(horizontalMovement, verticalMovement);

        // If the A.I. is too far from the target, return it to its pursue target state
        if (_distanceFromTarget > manager.stats.maxAttackRange
            || !idleState.IsTargetVisible(manager, manager.currentTarget, _viewableAngle))
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
					DecideCirclingActionMelee(manager);
					break;
				case Enums.WeaponCombatType.Ranged:
					DecideCirclingActionRanged(manager);
					break;
				default:
					break;
			}
        }

		// If the A.I. is currently doing nothing, roll for a character action
		if (manager.stats.currentRecoveryTime <= 0)
		{
			RollForActionChance(manager);
			PerformAction(manager);
		}
		
		// Keep rotating towards the target
        pursueTargetState.HandleRotation(manager);

        // If no character action is performed,
		if (!manager.isInteracting)
		{
			// If there is a viable attack, then perform it
        	if (manager.stats.currentRecoveryTime <= 0 && attackState.currentAttack != null)
            {
           		randomDestinationSet = false;
            	return attackState;
        	}
			// Else, randomly get a new attack
			GetNewAttack(manager);
		}

		return this;
    }

	#region Movement

	private void DecideCirclingActionMelee(EnemyManager manager)
	{
		Tuple<float, float> result;
        
		float chance = UnityEngine.Random.Range(-1, 1);
		if (_distanceFromTarget >= manager.stats.maxAttackRange / 2)
		{
			if (chance < 0)
			{
				result = manager.locomotion.RunAroundTarget(manager);
			}
			else
			{
				result = manager.locomotion.RunTowardsTarget(manager);
			}
		}
		else
		{
			if (chance < 0)
			{
				result = manager.locomotion.WalkAroundTarget(manager);
			}
			else
			{
				result = manager.locomotion.WalkTowardsTarget(manager);
			}
		}

		verticalMovement = result.Item1;
		horizontalMovement = result.Item2;
	}

	private void DecideCirclingActionRanged(EnemyManager manager)
	{
		Tuple<float, float> result;
        
		float chance = UnityEngine.Random.Range(-1, 1);
		if (_distanceFromTarget <= manager.stats.maxAttackRange / 2)
		{
			if (chance < 0)
			{
				result = manager.locomotion.RunAroundTarget(manager);
			}
			else
			{
				result = manager.locomotion.RunAwayFromTarget(manager);
			}
		}
		else
		{
			if (chance < 0)
			{
				result = manager.locomotion.WalkAroundTarget(manager);
			}
			else
			{
				result = manager.locomotion.WalkAwayFromTarget(manager);
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

	private void RollForActionChance(EnemyManager manager)
	{
		CharacterAction[] actions = manager.GetActions();
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

	private void PerformAction(EnemyManager manager)
	{
		// Get all actions that are marked as performeable
		CharacterAction[] actions = Array.FindAll(manager.GetActions(), action => action.toBePerformed);
		if (actions.Length > 0)
		{
			// Select random action from the array above and perform it
			int selectedActionIndex = UnityEngine.Random.Range(0, actions.Length - 1);
			actions[selectedActionIndex].PerformAction(manager);
			
			Debug.Log(actions[selectedActionIndex]);

			// Reset other actions
			foreach (var action in actions)
				action.toBePerformed = false;
		}
	}

	#endregion

    private void GetNewAttack(EnemyManager manager)
    {
	    // Don't retrieve actives, ultimates and charges
	    WeaponAction[] actions = Array.FindAll(
			    _weapon.GetActions(),
			    action => action.type == Enums.ActionType.Basic || action.type == Enums.ActionType.Shoot
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
