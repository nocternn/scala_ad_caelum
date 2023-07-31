using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterAction : Action
{
    #region Attributes

    [Header("Specific Properties")]
    public int chanceToBePerformed;
    public bool toBePerformed;
    public Enums.CharacterActionType type { get; protected set; }

    #endregion
    
    public override void PerformAction(PlayerManager player, bool playAnimation = true)
    {
        // Action has been performed
        toBePerformed = false;
        
        base.PerformAction(player, playAnimation);
    }
    
    public override void PerformAction(EnemyManager enemy, bool playAnimation = true)
    {
        // Action has been performed
        toBePerformed = false;
        
        base.PerformAction(enemy, playAnimation);
    }
}
