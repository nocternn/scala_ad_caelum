using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponAction : Action
{
    #region Attributes

    [Header("Attack - Properties")]
    public int score;
    public Enums.WeaponActionType type { get; protected set; }
    
    [Header("Attack - Distance")]
    public int minDistance;
    public int maxDistance;

    [Header("Attack - Angle")]
    public int minAngle;
    public int maxAngle;
    
    #endregion

    public virtual string GetDescription()
    {
        return "";
    }
    
    public override void PerformAction(PlayerManager player, bool playAnimation = true)
    {
        base.PerformAction(player, playAnimation);
    }
    
    public override void PerformAction(EnemyManager enemy, bool playAnimation = true)
    {
        base.PerformAction(enemy, playAnimation);
    }
}
