using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Character Action/Dodge")]
public class DodgeAction : CharacterAction
{
    [Header("Specific Properties")]
    [SerializeField] private string _backwardAnimation;
    [SerializeField] private string _leftAnimation;
    [SerializeField] private string _rightAnimation;
    
    protected override void Awake()
    {
        type = Enums.ActionType.Dodge;
    }
    
    public override void PerformAction(PlayerManager player, bool playAnimation = true)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection  = CameraManager.Instance.GetDirection("forward") * player.GetMovementInput("vertical");
        moveDirection += CameraManager.Instance.GetDirection("right") * player.GetMovementInput("horizontal");

        if (player.inputHandler.moveAmount > 0)
        {
            if (playAnimation)
				player.PlayTargetAnimation(_animation, true);
			
            moveDirection.y = 0;
			player.transform.rotation = Quaternion.LookRotation(moveDirection);
        }
        else
        {
			if (playAnimation)
            	player.PlayTargetAnimation(_backwardAnimation, true);
        }
        
        base.PerformAction(player, playAnimation);
    }
    
    public override void PerformAction(EnemyManager enemy, bool playAnimation = true)
    {
        Tuple<float, float> movement = ((EnemyStateCombatStance)enemy.currentState).GetMovementValues();
        
        // Item1 = horizontal movement; Item2 = vertical movement;
        if (movement.Item1 > 0)
        {
            if (playAnimation)
                enemy.animatorHandler.PlayTargetAnimation(_rightAnimation, true);
        }
        else if (movement.Item1 < 0)
        {
            if (playAnimation)
                enemy.animatorHandler.PlayTargetAnimation(_leftAnimation, true);
        }
        else if (movement.Item2 > 0)
        {
            if (playAnimation)
                enemy.animatorHandler.PlayTargetAnimation(_animation, true);
        }
        else
        {
            if (playAnimation)
                enemy.animatorHandler.PlayTargetAnimation(_backwardAnimation, true);
        }
        
        base.PerformAction(enemy, playAnimation);
    }
}
